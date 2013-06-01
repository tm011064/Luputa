using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;

namespace CodeGenerationTool.Components
{
  public class SchemaComparer
  {

    private string CompareUserDefinedFunctions(Server serverA, Database databaseA
      , Server serverB, Database databaseB
      , bool dropExisting)
    {
      databaseA.UserDefinedFunctions.Refresh();
      databaseB.UserDefinedFunctions.Refresh();

      HashSet<string> aNames = new HashSet<string>();
      Dictionary<string, UserDefinedFunction> bUserDefinedFunctions = new Dictionary<string, UserDefinedFunction>();
      foreach (UserDefinedFunction sp in databaseB.UserDefinedFunctions)
      {
        if (!sp.IsSystemObject)
        {
          bUserDefinedFunctions[sp.Name] = sp;
        }
      }

      StringBuilder alters = new StringBuilder();
      StringBuilder drops = new StringBuilder();
      UserDefinedFunction udfB;

      bool hasChanged;
      foreach (UserDefinedFunction udfA in databaseA.UserDefinedFunctions)
      {
        if (!udfA.IsSystemObject)
        {
          hasChanged = false;

          if (bUserDefinedFunctions.ContainsKey(udfA.Name))
          {
            udfB = bUserDefinedFunctions[udfA.Name];
            if (udfA.TextBody != udfB.TextBody
                || udfA.TextHeader != udfB.TextHeader)
            {
              hasChanged = true;
            }
          }
          else
          {
            hasChanged = true;
          }

          if (hasChanged)
          {
            alters.AppendLine(
  @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + udfA.Name + @"]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[" + udfA.Name + @"];
END
GO
" + udfA.TextHeader + udfA.TextBody + @"
GO");
          }

          aNames.Add(udfA.Name);
        }
      }

      if (dropExisting)
      {
        foreach (string name in bUserDefinedFunctions.Keys)
        {
          if (!aNames.Contains(name))
          {
            drops.AppendLine(
  @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + name + @"]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[" + name + @"];
END
GO");
          }
        }
      }

      return alters.ToString()
        + Environment.NewLine + drops.ToString();
    }

    private string CompareStoredProcedures(Server serverA, Database databaseA
      , Server serverB, Database databaseB
      , bool dropExisting)
    {
      databaseA.StoredProcedures.Refresh();
      databaseB.StoredProcedures.Refresh();

      HashSet<string> aNames = new HashSet<string>();
      Dictionary<string, StoredProcedure> bStoredProcedures = new Dictionary<string, StoredProcedure>();
      foreach (StoredProcedure sp in databaseB.StoredProcedures)
      {
        if (!sp.IsSystemObject)
        {
          bStoredProcedures[sp.Name] = sp;
        }
      }

      StringBuilder alters = new StringBuilder();
      StringBuilder drops = new StringBuilder();
      StoredProcedure spB;

      bool hasChanged;
      foreach (StoredProcedure spA in databaseA.StoredProcedures)
      {
        if (!spA.IsSystemObject)
        {
          hasChanged = false;

          if (bStoredProcedures.ContainsKey(spA.Name))
          {
            spB = bStoredProcedures[spA.Name];
            if (spA.TextBody != spB.TextBody
                || spA.TextHeader != spB.TextHeader)
            {
              hasChanged = true;
            }
          }
          else
          {
            hasChanged = true;
          }

          if (hasChanged)
          {
            alters.AppendLine(
  @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + spA.Name + @"]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[" + spA.Name + @"];
END
GO
" + spA.TextHeader + spA.TextBody + @"
GO");
          }

          aNames.Add(spA.Name);
        }
      }

      if (dropExisting)
      {
        foreach (string name in bStoredProcedures.Keys)
        {
          if (!aNames.Contains(name))
          {
            drops.AppendLine(
  @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + name + @"]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[" + name + @"];
END
GO");
          }
        }
      }

      return alters.ToString()
        + Environment.NewLine + drops.ToString();
    }



    private void test()
    {
      SchemaComparer sc = new SchemaComparer();
      sc.Compare(
          string.Format("Server={0};Database={1};Trusted_Connection=True", @"COMPI\SQLEXPRESS", "Workmate_new")
        , string.Format("Server={0};Database={1};Trusted_Connection=True", @"COMPI\SQLEXPRESS", "Workmate")
        , @"C:\temp\Workmate_debug\Upgrades\"
        , true);
      
      System.Console.ReadLine();
    }
    public void Compare(string connectionA, string connectionB, string path, bool dropExisting)
    {
      Server serverA = new Server(new ServerConnection(new SqlConnection(connectionA)));
      SqlConnectionStringBuilder connectionStringBuilderA = new SqlConnectionStringBuilder(connectionA);
      Database databaseA = new Database(serverA, connectionStringBuilderA.InitialCatalog);

      Server serverB = new Server(new ServerConnection(new SqlConnection(connectionB)));
      SqlConnectionStringBuilder connectionStringBuilderB = new SqlConnectionStringBuilder(connectionB);
      Database databaseB = new Database(serverB, connectionStringBuilderB.InitialCatalog);

      StringBuilder sb = new StringBuilder();

      sb.AppendLine("/* STORED PRCEDURES */");
      sb.AppendLine(CompareStoredProcedures(serverA, databaseA, serverB, databaseB, dropExisting));
      sb.AppendLine("/* FUNCTIONS */");
      sb.AppendLine(CompareUserDefinedFunctions(serverA, databaseA, serverB, databaseB, dropExisting));

      Trace.WriteLine(sb.ToString());
    }
  }
}
