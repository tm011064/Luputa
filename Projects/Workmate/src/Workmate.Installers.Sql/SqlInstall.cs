using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Workmate.Installers.Sql
{
  [RunInstaller(true)]
  public partial class SqlInstall : Installer
  {
    //default value, it will be overwrite by the installer
		string _ConnectionString=string.Empty;

		private Container _Components=null;

    #region constructors
    public SqlInstall()
		{
			// This call is required by the Designer.
			InitializeComponent();
		}
    #endregion

    /// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
        if (_Components != null)
				{
          _Components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		private static string GetScript(string path)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
      DirectoryInfo directoryInfo = new FileInfo(asm.Location).Directory;

      return File.ReadAllText(directoryInfo.FullName + path);
		}

		private static void ExecuteSql(SqlConnection sqlCon)
		{
			string[] SqlLine;
			Regex regex = new Regex("^GO",RegexOptions.IgnoreCase | RegexOptions.Multiline);

      string txtSQL = GetScript(@"Scripts\Schema\CreateObjects.sql");
			SqlLine = regex.Split(txtSQL);

			SqlCommand cmd = sqlCon.CreateCommand();
			cmd.Connection = sqlCon;

			foreach(string line in SqlLine)
			{
				if(line.Length>0)
				{
					cmd.CommandText = line;
					cmd.CommandType = CommandType.Text;
					try
					{
						cmd.ExecuteNonQuery();
					}
					catch(SqlException)
					{
						//rollback
						ExecuteDrop(sqlCon);
						break;
					}
				}
			}
		}
		private static void ExecuteDrop(SqlConnection sqlCon)
		{	
			if(sqlCon.State!=ConnectionState.Closed)sqlCon.Close();
			sqlCon.Open();
			SqlCommand cmd = sqlCon.CreateCommand();
			cmd.Connection = sqlCon;
      cmd.CommandText = 
@"
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'ASPState')
	DROP DATABASE [ASPState]
GO
";
			cmd.CommandType = CommandType.Text;
			cmd.ExecuteNonQuery();
			sqlCon.Close();
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install (stateSaver);

			if(Context.Parameters["server"].Length > 0
          && Context.Parameters["database"].Length > 0)
			{
        _ConnectionString = "Server=" + Context.Parameters["server"] + ";Database=" + Context.Parameters["database"] + ";Trusted_Connection=True";

        stateSaver.Add("connectionString", _ConnectionString);
			}

      SqlConnection sqlCon = new SqlConnection(_ConnectionString);

			sqlCon.Open();
			ExecuteSql(sqlCon);
			if(sqlCon.State!=ConnectionState.Closed)sqlCon.Close();
		}

		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall (savedState);

			if(savedState.Contains("connectionString"))
			{
        _ConnectionString = savedState["connectionString"].ToString();			
			}

      SqlConnection sqlCon = new SqlConnection(_ConnectionString);
	
			ExecuteDrop(sqlCon);
		}
  }
}
