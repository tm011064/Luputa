using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenerationTool.Components;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace CodeGenerationTool.Console
{
  class Program
  {
    const string HELP =
@"
Usage : scriptgen [options]
Options : 

  -s:<text>             Specifies the server name where the database is located
  -d:<text>             Specifies the database name
  -p:<folder>           Specifies the root folder where files will be generated
  -sf:<folder>          Creates schema object files at the specified folder (default is Schema)
  -df:<folder>          Creates drop schema object files at the specified folder (default is Drop)
  -iscript:<filename>   Install script file name (default is InstallDB.sql)
  -dscript:<filename>   Drop all objects script file name (default is DropObjects.sql)
  -h[elp]               Prints this message

  -compare              Specifies that upgrade scripts should be generated
  -s2:<text>            Specifies the server name of the database that should be upgraded
  -d2:<text>            Specifies the database name of the database that should be upgraded

Example : 

  scriptgen -s:localhost -d:MyDatabaseName -p:""C:\MyFolder\InstallHere"" -sf:Schema

  scriptgen -compare -s:localhost -d:NewDatabase -s2:localhost -d2:OldDatabase -p:""C:\MyFolder\UpgradeScripts""
    -> will generate upgrade scripts for the old database (d2)

";

    const string ERRORS =
@"
ERROR(S) OCCURRED:

{0}

Try 'scriptgen -help' for more information
";


    private static void RunScriptGenerator(string server, string databaseName, string rootPath, string schemaFolder, string dropFilesFolder
      , string installScriptFileName, string dropObjectsFileName)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True"
        , server
        , databaseName);

      using (SqlConnection c = new System.Data.SqlClient.SqlConnection(connectionString))
      {
        try { c.Open(); }
        catch (Exception e)
        {
          System.Console.WriteLine(string.Format(ERRORS, "Unable to run scriptgen. Reason: " + e.Message));
          return;
        }
        finally
        {
          if (c.State == System.Data.ConnectionState.Open)
            c.Close();
        }
      }

      StringBuilder errors = new StringBuilder();
      DatabaseScriptGenerator databaseScriptGenerator = new DatabaseScriptGenerator();
      databaseScriptGenerator.NewMessage += new EventHandler<MessageEventArgs>(databaseScriptGenerator_NewMessage);
      databaseScriptGenerator.Generate(connectionString
        , rootPath.TrimEnd('\\')
        , rootPath.TrimEnd('\\') + @"\" + schemaFolder.Trim('\\')
        , rootPath.TrimEnd('\\') + @"\" + dropFilesFolder.Trim('\\')
        , installScriptFileName
        , dropObjectsFileName
        , errors);

      if (errors.Length == 0)
      {
        System.Console.WriteLine(
  @"
SUCCESS

Yout have successfully created all database scripts. Press ENTER");
      }
      else
      {
        System.Console.WriteLine(
  @"
ERRORS OCCURED

Scripts were generated but the following errors occurred during generation:

" + errors.ToString() + @"

Press ENTER");
      }

      System.Console.ReadLine();
    }

    static void Main(string[] args)
    {
      System.Console.WriteLine(@"scriptgen 0.01");

      string server = null;
      string databaseName = null;
      string server2 = null;
      string databaseName2 = null;
      string rootPath = null;
      string schemaFolder = "Schema";
      string dropFilesFolder = "Drop";
      string installScriptFileName = "InstallDB.sql";
      string dropObjectsFileName = "DropObjects.sql";
      bool doCompare = false;


      string key, value;
      int index;
      foreach (string arg in args)
      {
        index = arg.IndexOf(':');
        if (index < 0)
        {
          key = arg;
          value = null;
        }
        else
        {
          key = arg.Substring(0, index);
          value = arg.Substring(index + 1).Trim('"');
        }

        switch (key)
        {
          case "-s": server = value; break;
          case "-d": databaseName = value; break;
          case "-p": rootPath = value; break;
          case "-sf": schemaFolder = value; break;
          case "-df": dropFilesFolder = value; break;
          case "-iscript": installScriptFileName = value; break;
          case "-dscript": dropObjectsFileName = value; break;

          case "-compare": doCompare = true; break;
          case "-s2": server2 = value; break;
          case "-d2": databaseName2 = value; break;

          case "-h":
          case "-help":
            System.Console.WriteLine(HELP);
            return;
        }
      }

      if (string.IsNullOrEmpty(server))
      {
        System.Console.WriteLine(string.Format(ERRORS, "Unable to run scriptgen. You have to provide a server name."));
        return;
      }
      if (string.IsNullOrEmpty(databaseName))
      {
        System.Console.WriteLine(string.Format(ERRORS, "Unable to run scriptgen. You have to provide a database name."));
        return;
      }
      if (string.IsNullOrEmpty(rootPath))
      {
        System.Console.WriteLine(string.Format(ERRORS, "Unable to run scriptgen. You have to provide a schema objects path."));
        return;
      }

      if (doCompare)
      {
      }
      else
      {
        RunScriptGenerator(server, databaseName, rootPath, schemaFolder, dropFilesFolder, installScriptFileName, dropObjectsFileName);
      }

    }

    static void databaseScriptGenerator_NewMessage(object sender, MessageEventArgs e)
    {
      System.Console.WriteLine(e.Message);
    }
  }
}
