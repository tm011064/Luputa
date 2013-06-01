using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Linq;
using CommonTools.Extensions;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SqlTypes;
using Workmate.Data;
using System.Threading;
using NUnit.Framework;

namespace Workmate.Tests
{
  class QuickTest
  {
    void tttt()
    {
      XElement xxx = new XElement("g");
      //xxx.Add(new XElement("h", "hello"));
      Trace.WriteLine(xxx.HasElements);
      Trace.WriteLine(xxx.ToString());
    }


    class SomeDisposable : IDisposable
    {

      #region IDisposable Members

      public void Dispose()
      {
        Trace.WriteLine("Dispose me");
      }

      #endregion
    }
    string returnmethod()
    {
      using (SomeDisposable sd = new SomeDisposable())
        return "HELLO WORLD";
    }
    void tt()
    {
      Trace.WriteLine("Step1");
      string s = returnmethod();
      Trace.WriteLine(s); ;
      Trace.WriteLine("Step2"); ;
    }

    public void test()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;


      string applicationPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
      string path = Path.Combine(applicationPath, @"DataAccess\SqlProvider\SetupScript.sql");
      byte[] bytes = File.ReadAllBytes(path);

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open(); // this will throw an exception if we can't connect

        XElement xml = null;
        using (SqlCommand cmd = connection.CreateCommand())
        {
          cmd.CommandText = "SELECT TOP 1 ExtraInfo FROM cms_Contents";
          cmd.CommandType = System.Data.CommandType.Text;

          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              xml = XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None);
            }
          }
        }

        var job = xml.ParseXElementNode<string>("job", "HELLO");
        var age = xml.ParseXElementNode<int>("age", 200);

        int? fileId = null;
        using (SqlCommand cmd = connection.CreateCommand())
        {
          cmd.CommandText = "cms_Files_Insert";
          cmd.CommandType = System.Data.CommandType.StoredProcedure;

          cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, 5));
          cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, 99));
          cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, "TestName"));
          cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.VarBinary, bytes.Length, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, bytes));
          cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar, 64, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, "UNKNOWN"));
          cmd.Parameters.Add(new SqlParameter("@ContentSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, bytes.Length));
          cmd.Parameters.Add(new SqlParameter("@FriendlyFileName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, "TestName"));
          cmd.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, 0));
          cmd.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, 0));
          cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, 1));

          SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
          cmd.Parameters.Add(rv);

          cmd.ExecuteNonQuery();
          if (rv != null && rv.Value != null)
            fileId = (int)(rv.Value);
        }

        using (SqlCommand cmd = connection.CreateCommand())
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandText = "cms_Files_Get";

          cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));

          using (System.Data.IDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              byte[] b = dr["Content"] as byte[];
            }
          }
        }

      }
    }
  }
}
