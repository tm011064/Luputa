using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeGenerationTool.Components
{
  enum ScriptType
  {
    Table,
    Constraint,
    Index,
    StoredProcedure,
    Function,
    Default,
    Check,
    DropTable,
    DropStoredProcedure,
    DropFunction,
    DropConstraint,
    DropIndex,
    DropDefault,
    DropCheck
  }
  class ScriptInfo
  {
    public ScriptType ScriptType { get; set; }
    public string FileName { get; set; }
    public string Text { get; set; }
    public string FormattedText { get; set; }
    public string Name { get; set; }
    public HashSet<string> ReferencedFunctions { get; set; }

    public ScriptInfo(ScriptType scriptType, string fileName, string text)
      : this(scriptType, fileName, text, null, null) { }
    public ScriptInfo(ScriptType scriptType, string fileName, string text, string name, string formattedText)
    {
      this.ScriptType = scriptType;
      this.FileName = fileName;
      this.Text = text;
      this.Name = name;
      this.FormattedText = formattedText;
      this.ReferencedFunctions = new HashSet<string>();
    }
  }

  public class MessageEventArgs : EventArgs
  {
    public string Message { get; set; }
    public MessageEventArgs(string message)
    {
      this.Message = message;
    }
  }

  public class DatabaseScriptGenerator
  {
    public event EventHandler<MessageEventArgs> NewMessage;
    private void RaiseMessageEvent(string message)
    {
      if (this.NewMessage != null)
        this.NewMessage(this, new MessageEventArgs(message));
    }

    private string ScriptObject(Urn[] urns, Scripter scripter)
    {
      StringCollection sc = scripter.Script(urns);
      StringBuilder sb = new StringBuilder();

      foreach (string str in sc)
      {
        sb.Append(str + Environment.NewLine + "GO" +
          Environment.NewLine + Environment.NewLine);
      }

      return sb.ToString();
    }

    private void AppendTableScripts(Server server, Database database
      , Dictionary<ScriptType, List<ScriptInfo>> scriptInfos)
    {
      RaiseMessageEvent("Start building table scripts");

      Scripter scripter = new Scripter(server);
      Scripter scripter2 = new Scripter(server);

      ScriptingOptions so = new ScriptingOptions();
      so.IncludeIfNotExists = true;
      so.IncludeHeaders = false;
      so.Permissions = false;
      so.ExtendedProperties = false;
      so.ScriptDrops = false;
      so.IncludeDatabaseContext = false;
      so.NoCollation = true;
      so.NoFileGroup = true;
      so.NoIdentities = false;
      so.Indexes = true;
      so.DriAll = true;
      so.DriChecks = true;
      scripter.Options = so;


      ScriptingOptions so2 = new ScriptingOptions();
      so2.IncludeIfNotExists = true;
      so2.ScriptDrops = false;
      scripter2.Options = so2;

      server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
      database.Tables.Refresh();
      StringCollection sc;

      foreach (Table table in database.Tables)
      {
        if (!table.IsSystemObject)
        {
          RaiseMessageEvent("Generate scripts for table " + table.Name);
          scriptInfos[ScriptType.DropTable].Add(new ScriptInfo(
            ScriptType.DropTable
            , "dbo.drop.table." + table.Name + ".sql"
            , "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + table.Name + @"]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[" + table.Name + @"];
END
GO"));

          sc = scripter.Script(new Urn[] { table.Urn });
          StringBuilder sb = new StringBuilder();

          #region do tables, indexes and constraints
          foreach (string str in sc)
          {
            if (str == "SET ANSI_NULLS ON"
              || str == "SET QUOTED_IDENTIFIER ON")
            {
              continue;
            }

            if (str.Contains("CREATE TABLE"))
            {
              scriptInfos[ScriptType.Table].Add(new ScriptInfo(
                ScriptType.Table
                , "dbo.table." + table.Name + ".sql"
                , str + Environment.NewLine + "GO"));
            }
            else if (str.StartsWith("IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].["))
            {
              int index = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[".Length;

              string name = str.Substring(index, str.IndexOf(']', index) - index);
              scriptInfos[ScriptType.DropConstraint].Add(new ScriptInfo(
                ScriptType.DropConstraint
                , "dbo.drop.constraint." + name + ".sql"
                , "IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[" + name + @"]') AND parent_object_id = OBJECT_ID(N'[dbo].[" + table.Name + @"]'))
BEGIN
  ALTER TABLE [dbo].[" + table.Name + @"] DROP CONSTRAINT " + name + @"
END
GO"));

              scriptInfos[ScriptType.Constraint].Add(new ScriptInfo(
                ScriptType.Constraint
                , "dbo.constraint." + str.Substring(index, str.IndexOf(']', index) - index) + ".sql"
                , str + Environment.NewLine + "GO"));
            }
            else if (str.StartsWith("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].["))
            {
              int index = "IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[".Length;

              ScriptInfo scriptInfo = scriptInfos[ScriptType.Constraint].Find(c => c.FileName == "dbo.constraint." + str.Substring(index, str.IndexOf(']', index) - index) + ".sql");
              if (scriptInfo != null)
              {
                scriptInfo.Text += Environment.NewLine + Environment.NewLine + str + Environment.NewLine + "GO";
              }
              else
              {
                scriptInfos[ScriptType.Constraint].Add(new ScriptInfo(
                  ScriptType.Constraint
                  , "dbo.constraint." + str.Substring(index, str.IndexOf(']', index) - index) + ".sql"
                  , str + Environment.NewLine + "GO"));
              }
            }
            else if (str.StartsWith("\r\nIF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].["))
            {
              int index = str.IndexOf("AND name = N'") + 13;
              string name = str.Substring(index, str.IndexOf('\'', index) - index);
              scriptInfos[ScriptType.DropIndex].Add(new ScriptInfo(
                ScriptType.DropIndex
                , "dbo.drop.index." + name + ".sql"
                , @"IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[" + name + @"]') AND name = N'" + table.Name + @"')
BEGIN
  ALTER TABLE [dbo].[" + table.Name + @"] DROP CONSTRAINT [" + name + @"]
END
GO"));
              scriptInfos[ScriptType.Index].Add(new ScriptInfo(
                ScriptType.Index
                , "dbo.index." + name + ".sql"
                , str.TrimStart("\r\n".ToCharArray()) + Environment.NewLine + "GO"));
            }
            else if (str.StartsWith("IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].["))
            {
              int index = "IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[".Length;
              string name = str.Substring(index, str.IndexOf(']', index) - index);
              scriptInfos[ScriptType.DropCheck].Add(new ScriptInfo(
                ScriptType.DropCheck
                , "dbo.drop.check." + name + ".sql"
                ,
@"
IF EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[" + name + @"]') AND parent_object_id = OBJECT_ID(N'[dbo].[" + table.Name + @"]'))
BEGIN
  ALTER TABLE [dbo].[" + table.Name + @"] DROP CONSTRAINT [" + name + @"]
END
GO"));
              scriptInfos[ScriptType.Check].Add(new ScriptInfo(
                ScriptType.Check
                , "dbo.check." + name + ".sql"
                , str + Environment.NewLine + "GO"));
            }
            else
            {

            }
          }
          #endregion

          #region default constraints
          foreach (Column column in table.Columns)
          {
            if (column.DefaultConstraint != null)
            {
              string dropText =
@"
IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[" + column.DefaultConstraint.Name + @"]') AND parent_object_id = OBJECT_ID(N'[dbo].[" + table.Name + @"]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[" + column.DefaultConstraint.Name + @"]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[" + table.Name + @"] DROP CONSTRAINT [" + column.DefaultConstraint.Name + @"]    
  END
END
GO";

              scriptInfos[ScriptType.DropDefault].Add(new ScriptInfo(
                ScriptType.DropDefault
                , "dbo.drop.default." + column.DefaultConstraint.Name + ".sql"
                , dropText));

              foreach (string str in scripter2.Script(new Urn[] { column.DefaultConstraint.Urn }))
              {
                scriptInfos[ScriptType.Default].Add(new ScriptInfo(
                  ScriptType.Default
                  , "dbo.default." + column.DefaultConstraint.Name + ".sql"
                  , str.TrimStart("\r\n".ToCharArray()) + Environment.NewLine + "GO"));
              }
            }
          }
          #endregion
        }
      }
    }

    private void AppendStoredProcedureScripts(Server server, Database database
      , Dictionary<ScriptType, List<ScriptInfo>> scriptInfos)
    {
      RaiseMessageEvent("Start building stored procedure scripts");

      Scripter scripter = new Scripter(server);

      ScriptingOptions so = new ScriptingOptions();
      so.IncludeIfNotExists = false;
      so.IncludeHeaders = false;
      so.Permissions = false;
      so.ExtendedProperties = false;
      so.ScriptDrops = false;
      so.IncludeDatabaseContext = false;
      so.NoCollation = true;
      so.NoFileGroup = true;
      so.NoIdentities = false;

      scripter.Options = so;

      server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
      database.StoredProcedures.Refresh();
      StringCollection sc;
      foreach (StoredProcedure storedProcedure in database.StoredProcedures)
      {
        if (!storedProcedure.IsSystemObject)
        {
          RaiseMessageEvent("Generate scripts for sp " + storedProcedure.Name);
          scriptInfos[ScriptType.DropStoredProcedure].Add(new ScriptInfo(
            ScriptType.DropStoredProcedure
            , "dbo.drop.sp." + storedProcedure.Name + ".sql"
            , @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + storedProcedure.Name + @"]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[" + storedProcedure.Name + @"];
END
GO"));

          sc = scripter.Script(new Urn[] { storedProcedure.Urn });
          StringBuilder sb = new StringBuilder();

          foreach (string str in sc)
          {
            if (str == "SET ANSI_NULLS ON"
              || str == "SET QUOTED_IDENTIFIER ON")
            {
              continue;
            }

            string text =
@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + storedProcedure.Name + @"]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[" + storedProcedure.Name + @"];
END
GO

";

            scriptInfos[ScriptType.StoredProcedure].Add(new ScriptInfo(
              ScriptType.StoredProcedure
              , "dbo.sp." + storedProcedure.Name + ".sql"
              , text + str + Environment.NewLine + "GO"));

          }

        }
      }
    }

    private string RemoveSqlComments(string input)
    {
      Regex regex = new Regex(@"(--.*)|(((/\*)+?[\w\W]+?(\*/)+))");

      string output = input;
      Match matchResult = regex.Match(input);
      while (matchResult.Success)
      {
        output = output.Replace(matchResult.Value, string.Empty);
        matchResult = matchResult.NextMatch();
      }

      return output.ToLowerInvariant();
    }
    private void AppendFundctionScripts(Server server, Database database
      , Dictionary<ScriptType, List<ScriptInfo>> scriptInfos, StringBuilder errorMessages)
    {
      RaiseMessageEvent("Start building function scripts");

      Scripter scripter = new Scripter(server);

      ScriptingOptions so = new ScriptingOptions();
      so.IncludeIfNotExists = true;
      so.IncludeHeaders = false;
      so.Permissions = false;
      so.ExtendedProperties = false;
      so.ScriptDrops = false;
      so.IncludeDatabaseContext = false;
      so.NoCollation = true;
      so.NoFileGroup = true;
      so.NoIdentities = false;

      scripter.Options = so;

      server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
      database.UserDefinedFunctions.Refresh();
      StringCollection sc;

      Queue<ScriptInfo> bodiesQueue = new Queue<ScriptInfo>();
      Queue<ScriptInfo> namesQueue = new Queue<ScriptInfo>();
      ScriptInfo scriptInfo;
      StringBuilder sb = new StringBuilder();

      foreach (UserDefinedFunction userDefinedFunction in database.UserDefinedFunctions)
      {
        if (!userDefinedFunction.IsSystemObject)
        {
          RaiseMessageEvent("Generate scripts for function " + userDefinedFunction.Name);

          scriptInfos[ScriptType.DropFunction].Add(new ScriptInfo(
            ScriptType.DropFunction
            , "dbo.drop.function." + userDefinedFunction.Name + ".sql"
            , @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + userDefinedFunction.Name + @"]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[" + userDefinedFunction.Name + @"];
END
GO"));

          sc = scripter.Script(new Urn[] { userDefinedFunction.Urn });
          sb.Clear();

          foreach (string str in sc)
          {
            if (str == "SET ANSI_NULLS ON"
              || str == "SET QUOTED_IDENTIFIER ON")
            {
              continue;
            }

            string text =
@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + userDefinedFunction.Name + @"]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[" + userDefinedFunction.Name + @"];
END
GO

";

            scriptInfo = new ScriptInfo(
              ScriptType.Function
              , "dbo.function." + userDefinedFunction.Name + ".sql"
              , text + str + Environment.NewLine + "GO"
              , userDefinedFunction.Name
              , RemoveSqlComments(str).ToLowerInvariant());

            namesQueue.Enqueue(scriptInfo);
            bodiesQueue.Enqueue(scriptInfo);
          }

        }
      }

      int fullRun = namesQueue.Count;
      int counter = 0;
      int lastRunCount = 0;
      Regex regex;
      ScriptInfo current;
      ScriptInfo peek;

      RaiseMessageEvent("Resolving function references");
      bool flag = false;
      while (namesQueue.Count != 0)
      {
        flag = false;
        current = namesQueue.Dequeue();

        peek = bodiesQueue.Peek();
        foreach (ScriptInfo scriptInfoFromNamesQueue in namesQueue)
        {
          regex = new Regex(@"( |\.)" + scriptInfoFromNamesQueue.Name.ToLowerInvariant() + @"( |\()");
          
          if (scriptInfoFromNamesQueue.Name == peek.Name)
            continue;
          
          if (regex.IsMatch(peek.FormattedText)) // TODO (Roman): this should be a formatted text
          {
            flag = true;
            scriptInfoFromNamesQueue.ReferencedFunctions.Add(peek.Name);
            //break;
          }
        }

        if (!flag)
        {
          scriptInfos[ScriptType.Function].Add(bodiesQueue.Dequeue());
        }
        else
        {
          bodiesQueue.Enqueue(bodiesQueue.Dequeue());
          namesQueue.Enqueue(current);
        }

        counter++;
        if (counter == fullRun)
        {
          counter = 0;

          if (namesQueue.Count == lastRunCount
              && lastRunCount != 0)
          {
            RaiseMessageEvent("Circular dependencies detected at functions");
            break;
          }

          lastRunCount = fullRun = namesQueue.Count;
        }
      }

      bool first = true;
      foreach (ScriptInfo scriptInfoFromNamesQueue in namesQueue)
      {
        sb.Clear();

        first = true;
        sb.Append("Function " + scriptInfoFromNamesQueue.Name + " has a (potential) circular reference to other functions. Referenced functions: ");
        foreach (string reference in scriptInfoFromNamesQueue.ReferencedFunctions)
        {
          sb.Append((first ? string.Empty : ", ") + reference);
          first = false;
        }

        scriptInfos[ScriptType.Function].Add(scriptInfoFromNamesQueue);
        RaiseMessageEvent(sb.ToString());
        errorMessages.Append(Environment.NewLine);
        errorMessages.Append(sb);
      }
    }

    private void test()
    {
      DatabaseScriptGenerator dsg = new DatabaseScriptGenerator();
      dsg.NewMessage += new EventHandler<MessageEventArgs>(delegate(object sender, MessageEventArgs e) { System.Console.WriteLine(e.Message); });
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True"
        , @"COMPI\SQLEXPRESS", "Workmate_debug");

      StringBuilder errors = new StringBuilder();
      dsg.Generate(connectionString, @"C:\temp\Workmate_debug\", @"C:\temp\Workmate_debug\Schema", @"C:\temp\Workmate_debug\Drop"
        , "InstallScript.sql", "DropObjects.sql", errors);

      Trace.WriteLine(errors.ToString());

      System.Console.ReadLine();
    }
    public void Generate(string connection, string path, string schemaPath, string dropFilesPath
      , string installScriptFilename, string dropAllFilename, StringBuilder errors)
    {
      Server server = new Server(new ServerConnection(new SqlConnection(connection)));
      SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connection);
      Database database = new Database(server, connectionStringBuilder.InitialCatalog);

      Dictionary<ScriptType, List<ScriptInfo>> scriptInfos = new Dictionary<ScriptType, List<ScriptInfo>>()
      {
        { ScriptType.Constraint, new List<ScriptInfo>()},
        { ScriptType.Function, new List<ScriptInfo>()},
        { ScriptType.Check, new List<ScriptInfo>()},        
        { ScriptType.Index, new List<ScriptInfo>()},
        { ScriptType.StoredProcedure, new List<ScriptInfo>()},
        { ScriptType.Table, new List<ScriptInfo>()},
        { ScriptType.DropConstraint, new List<ScriptInfo>()},
        { ScriptType.DropFunction, new List<ScriptInfo>()},
        { ScriptType.DropCheck, new List<ScriptInfo>()},
        { ScriptType.DropIndex, new List<ScriptInfo>()},
        { ScriptType.DropStoredProcedure, new List<ScriptInfo>()},
        { ScriptType.DropTable, new List<ScriptInfo>()},
        { ScriptType.Default, new List<ScriptInfo>()},
        { ScriptType.DropDefault, new List<ScriptInfo>()},
      };

      AppendTableScripts(server, database, scriptInfos);
      AppendStoredProcedureScripts(server, database, scriptInfos);      
      AppendFundctionScripts(server, database, scriptInfos, errors);
      
      path = path.TrimEnd('\\') + @"\";
      schemaPath = schemaPath.TrimEnd('\\') + @"\";
      dropFilesPath = dropFilesPath.TrimEnd('\\') + @"\";

      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      WriteToFiles(schemaPath + @"Constraints\", scriptInfos[ScriptType.Constraint]);
      WriteToFiles(schemaPath + @"Functions\", scriptInfos[ScriptType.Function]);
      WriteToFiles(schemaPath + @"Constraints\", scriptInfos[ScriptType.Check]);
      WriteToFiles(schemaPath + @"Indexes\", scriptInfos[ScriptType.Index]);
      WriteToFiles(schemaPath + @"StoredProcedures\", scriptInfos[ScriptType.StoredProcedure]);
      WriteToFiles(schemaPath + @"Tables\", scriptInfos[ScriptType.Table]);
      WriteToFiles(schemaPath + @"Defaults\", scriptInfos[ScriptType.Default]);
      WriteToFiles(dropFilesPath + @"Constraints\", scriptInfos[ScriptType.DropConstraint]);
      WriteToFiles(dropFilesPath + @"Constraints\", scriptInfos[ScriptType.DropCheck]);
      WriteToFiles(dropFilesPath + @"Indexes\", scriptInfos[ScriptType.DropIndex]);
      WriteToFiles(dropFilesPath + @"StoredProcedures\", scriptInfos[ScriptType.DropStoredProcedure]);
      WriteToFiles(dropFilesPath + @"Defaults\", scriptInfos[ScriptType.DropDefault]);
      WriteToFiles(dropFilesPath + @"Tables\", scriptInfos[ScriptType.DropTable]);

      StringBuilder sb = new StringBuilder();

      sb.AppendLine("/* DROP CONSTARINTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropConstraint])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP CHECK CONSTARINTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropCheck])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP INDEXES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropIndex])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP DEFAULTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropDefault])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP STORED PROCEDURES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropStoredProcedure])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP FUNCTIONS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropFunction])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DROP TABLES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.DropTable])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      File.WriteAllText(path + dropAllFilename, sb.ToString());
      RaiseMessageEvent("Created drop objects script.");


      sb = new StringBuilder();

      sb.AppendLine("/* TABLES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Table])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* DEFAULTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Default])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* CHECK CONSTRAINTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Check])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* INDEXES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Index])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* CONSTRAINTS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Constraint])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* STORED PROCEDURES */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.StoredProcedure])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      sb.AppendLine("/* FUNCTIONS */" + Environment.NewLine);
      foreach (ScriptInfo scriptInfo in scriptInfos[ScriptType.Function])
        sb.AppendLine(scriptInfo.Text + Environment.NewLine);

      File.WriteAllText(path + installScriptFilename, sb.ToString());
      RaiseMessageEvent("Created create objects script.");

      RaiseMessageEvent("Done");

    }

    private void WriteToFiles(string path, List<ScriptInfo> scriptInfos)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      RaiseMessageEvent("Start writing scripts to " + path);
      string filename;

      char[] invalidFileChars = Path.GetInvalidFileNameChars();
      foreach (ScriptInfo scriptInfo in scriptInfos)
      {
        filename = scriptInfo.FileName;
        foreach (char c in invalidFileChars)
          filename = filename.Replace(c, '_');

        filename = path + filename;
        if (!File.Exists(filename)
         || File.ReadAllText(filename) != scriptInfo.Text)
        {
          File.WriteAllText(filename, scriptInfo.Text);
          RaiseMessageEvent("Created " + filename);
        }
        else
        {
          RaiseMessageEvent("Ignored " + filename + " -> same text");
        }
      }
      RaiseMessageEvent("Finished writing scripts to " + path);
    }
  }
}
