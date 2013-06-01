using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CodeGenerationTool.Components;

namespace CodeGenerationTool
{
  public partial class MainForm : Form
  {
    #region members
    string _LastConnectionString;
    string _LastSpConnectionString;
    string _LastTablesConnectionString;
    string _LastTablesClassesConnectionString;
    delegate void LoadDBNamesHandler(string connection);
    delegate void LoadTablesHandler(string connection, ComboBox comboBox);
    #endregion

    private string GetTemplatePath()
    {
      return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Templates\");
    }

    private void btnRefreshDropdowns_Click(object sender, EventArgs e)
    {
      _LastConnectionString = _LastSpConnectionString = _LastTablesConnectionString = _LastTablesClassesConnectionString = null;
    }

    private void btnGenerateCRUD_Click(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      Cursor.Current = Cursors.WaitCursor;

      string templatePath = GetTemplatePath();

      string selectAll = CodeGenerator.GetSelectAllStoredProcedure(connectionString, cbTables.Text, "dbo", (int)txtTabIndent.Value, File.ReadAllText(templatePath + "CRUD_SelectAll.txt"));
      string select = CodeGenerator.GetSelectStoredProcedure(connectionString, cbTables.Text, "dbo", (int)txtTabIndent.Value, File.ReadAllText(templatePath + "CRUD_Select.txt"));
      string update = CodeGenerator.GetUpdateStoredProcedure(connectionString, cbTables.Text, "dbo", (int)txtTabIndent.Value, File.ReadAllText(templatePath + "CRUD_Update.txt"));
      string insert = CodeGenerator.GetInsertStoredProcedure(connectionString, cbTables.Text, "dbo", (int)txtTabIndent.Value, File.ReadAllText(templatePath + "CRUD_Insert.txt"));
      string delete = CodeGenerator.GetDeleteStoredProcedure(connectionString, cbTables.Text, "dbo", (int)txtTabIndent.Value, File.ReadAllText(templatePath + "CRUD_Delete.txt"));

      txtOutput.Text =
        selectAll
        + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine
        + select
        + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine
        + update
        + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine
        + insert
        + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine
        + delete
        + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine;
      Cursor.Current = Cursors.Default;

      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      switch (folderBrowserDialog.ShowDialog(this))
      {
        case DialogResult.OK:
          DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
          if (directoryInfo.Exists)
          {
            File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, cbTables.Text + "_Get.proc.sql"), select);
            File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, cbTables.Text + "_GetAll.proc.sql"), selectAll);
            File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, cbTables.Text + "_Update.proc.sql"), update);
            File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, cbTables.Text + "_Insert.proc.sql"), insert);
            File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, cbTables.Text + "_Delete.proc.sql"), delete);

            Process.Start(folderBrowserDialog.SelectedPath);
          }
          break;
      }
    }

    private void btnGenerateClassCode_Click(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      Cursor.Current = Cursors.WaitCursor;

      string templatePath = GetTemplatePath();
      txtOutput.Text = CodeGenerator.GetClassCode(connectionString, cbTablesClasses.Text, "dbo", File.ReadAllText(templatePath + "Class_Simple.txt"));
      Cursor.Current = Cursors.Default;
    }

    private void btnGenerate_Click(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      Cursor.Current = Cursors.WaitCursor;
      string templatePath = GetTemplatePath();
      switch (cbSpType.Text)
      {
        case "Data Reader": txtOutput.Text = CodeGenerator.GetStoredProcedureWrapperCode(connectionString, cbStoredProcedure.Text, "dbo", File.ReadAllText(templatePath + "SP_DataReader.txt")); break;
        case "Scalar": txtOutput.Text = CodeGenerator.GetStoredProcedureWrapperCode(connectionString, cbStoredProcedure.Text, "dbo", File.ReadAllText(templatePath + "SP_ExecuteScalar.txt")); break;
        case "Non Query": txtOutput.Text = CodeGenerator.GetStoredProcedureWrapperCode(connectionString, cbStoredProcedure.Text, "dbo", File.ReadAllText(templatePath + "SP_ExecuteNonQuery.txt")); break;
      }
      Cursor.Current = Cursors.Default;
    }

    void RefreshItems(ComboBox cb, IEnumerable<string> items)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((MethodInvoker)delegate { RefreshItems(cb, items); });
        return;
      }

      cb.BeginUpdate();
      try
      {
        cb.Items.Clear();
        foreach (string item in items)
          cb.Items.Add(item);
      }
      finally
      {
        cb.EndUpdate();
      }
    }

    public MainForm()
    {
      InitializeComponent();

      cbSpType.Items.Add("Data Reader");
      cbSpType.Items.Add("Scalar");
      cbSpType.Items.Add("Non Query");

      cbSpType.SelectedIndex = 0;

      //cbServerName.Text = "localhost";
      cbServerName.Text = @"COMPI\SQLEXPRESS";

      this.Load += new EventHandler(MainForm_Load);

      cbDatabaseName.Enter += new EventHandler(cbDatabaseName_Enter);
      cbStoredProcedure.Enter += new EventHandler(cbStoredProcedure_Enter);
      cbTables.Enter += new EventHandler(cbTables_Enter);
      cbTablesClasses.Enter += new EventHandler(cbTablesClasses_Enter);

      txtOutput.KeyUp += new KeyEventHandler(txtOutput_KeyUp);
    }

    void txtOutput_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
      {
        ((TextBox)sender).SelectAll();
      }
    }

    void cbTablesClasses_Enter(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      // skip if no change from the last time
      if (_LastTablesClassesConnectionString == connectionString)
        return;

      _LastTablesClassesConnectionString = connectionString;
      LoadTablesHandler call = new LoadTablesHandler(LoadTableNames);
      call.BeginInvoke(connectionString, cbTablesClasses, null, null);
    }

    void cbStoredProcedure_Enter(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      // skip if no change from the last time
      if (_LastSpConnectionString == connectionString)
        return;

      _LastSpConnectionString = connectionString;
      LoadDBNamesHandler call = new LoadDBNamesHandler(LoadStoredProceduresNames);
      call.BeginInvoke(connectionString, null, null);
    }
    void cbDatabaseName_Enter(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Trusted_Connection=True", this.cbServerName.Text);

      // skip if no change from the last time
      if (_LastConnectionString == connectionString)
        return;

      _LastConnectionString = connectionString;
      LoadDBNamesHandler call = new LoadDBNamesHandler(LoadDBNames);
      call.BeginInvoke(connectionString, null, null);
    }
    void cbTables_Enter(object sender, EventArgs e)
    {
      string connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True", this.cbServerName.Text, this.cbDatabaseName.Text);

      // skip if no change from the last time
      if (_LastTablesConnectionString == connectionString)
        return;

      _LastTablesConnectionString = connectionString;
      LoadTablesHandler call = new LoadTablesHandler(LoadTableNames);
      call.BeginInvoke(connectionString, cbTables, null, null);
    }

    private void MainForm_Load(object sender, System.EventArgs e)
    {
      Action call = new Action(LoadServerNames);
      call.BeginInvoke(null, null);
    }

    #region private methods
    private void LoadServerNames()
    {
      DataTable t = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
      List<string> names = new List<string>();

      foreach (DataRow r in t.Rows)
      {
        string n = r["ServerName"] as string;
        string i = r["InstanceName"] as string;

        if (!string.IsNullOrEmpty(i))
          n = n + '\\' + i;

        names.Add(n);
      }
      RefreshItems(cbServerName, names.OrderBy(p => p).ToList());
    }

    private void LoadDBNames(string connection)
    {
      using (SqlConnection c = new System.Data.SqlClient.SqlConnection(connection))
      {
        SqlDataAdapter da = new SqlDataAdapter("sp_databases", c);
        DataTable t = new DataTable();
        Exception err = null;

        try
        {
          if (c.State != ConnectionState.Open)
            c.Open();

          Version sqlServerVersion = new Version(c.ServerVersion);

          if (sqlServerVersion.Major > 8)
            da.SelectCommand.CommandText = "select [name] as DATABASE_NAME from sys.databases where name not in ('master', 'tempdb', 'msdb', 'model')";

          da.Fill(t);
        }
        catch (Exception e)
        {
          err = e;
        }

        List<string> names = new List<string>();

        foreach (DataRow r in t.Rows)
        {
          string n = r["DATABASE_NAME"] as string;

          string nl = n != null ? n.ToLower() : null;

          if (n == null || nl == "master" || nl == "tempdb"
            || nl == "model" || nl == "msdb" || nl == "distribution")
            continue;

          names.Add(n);
        }
        RefreshItems(cbDatabaseName, names.OrderBy(p => p).ToList());
      }
    }

    private void LoadStoredProceduresNames(string connection)
    {
      using (SqlConnection c = new System.Data.SqlClient.SqlConnection(connection))
      {
        SqlDataAdapter da = new SqlDataAdapter("sp_databases", c);
        DataTable t = new DataTable();
        Exception err = null;

        try
        {
          if (c.State != ConnectionState.Open)
            c.Open();

          Version sqlServerVersion = new Version(c.ServerVersion);

          if (sqlServerVersion.Major > 8)
            da.SelectCommand.CommandText = @"select SPECIFIC_NAME AS name
from
    INFORMATION_SCHEMA.ROUTINES as ISR
where
    ISR.ROUTINE_TYPE = 'PROCEDURE' and
    ObjectProperty (Object_Id (ISR.ROUTINE_NAME), 'IsMSShipped') = 0 and
    (
        select 
            major_id 
        from 
            sys.extended_properties 
        where 
            major_id = object_id(ISR.ROUTINE_NAME) and 
            minor_id = 0 and 
            class = 1 and 
            name = N'microsoft_database_tools_support'
    ) is null
order by
    ISR.ROUTINE_CATALOG,
    ISR.ROUTINE_SCHEMA,
    ISR.ROUTINE_NAME";

          da.Fill(t);
        }
        catch (Exception e)
        {
          err = e;
        }

        List<string> names = new List<string>();

        foreach (DataRow r in t.Rows)
        {
          names.Add(r["name"] as string);
        }
        RefreshItems(cbStoredProcedure, names.OrderBy(p => p).ToList());
      }
    }

    private void LoadTableNames(string connection, ComboBox comboBox)
    {
      using (SqlConnection c = new System.Data.SqlClient.SqlConnection(connection))
      {
        SqlDataAdapter da = new SqlDataAdapter("sp_databases", c);
        DataTable t = new DataTable();
        Exception err = null;

        try
        {
          if (c.State != ConnectionState.Open)
            c.Open();

          Version sqlServerVersion = new Version(c.ServerVersion);

          if (sqlServerVersion.Major > 8)
            da.SelectCommand.CommandText = "SELECT * FROM information_schema.tables";

          da.Fill(t);
        }
        catch (Exception e)
        {
          err = e;
        }

        List<string> names = new List<string>();

        foreach (DataRow r in t.Rows)
        {
          names.Add(r["TABLE_NAME"] as string);
        }
        RefreshItems(comboBox, names.OrderBy(p => p).ToList());
      }
    }
    #endregion

  }
}
