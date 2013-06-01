using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Data.Common;

namespace CommonTools.Data
{
    /// <summary>
    /// Used by the DataAccessFactory to return status results back to the client.
    /// </summary>
    public enum DbStatus : byte
    {
        /// <summary>
        /// Successful return.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Unable to parse the return type.
        /// </summary>
        UnableToParseObject = 1,

        /// <summary>
        /// 
        /// </summary>
        NoObjectReturned = 2
    }

    /// <summary>
    /// Provides a facade for creating and running Database queries on the server.
    /// </summary>
    /// <remarks>One of the disadvantages of generic database factories
    /// is that they provide only limited support for parametrised SQL queries.</remarks>
    public class DataAccessFactory
    {
        #region Global Variables
        private DbCommand _Cmd;
        private DbProviderFactory _Provider;
        private DbConnection _Con;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the connection used by the factory.
        /// </summary>
        public DbConnection Connection
        {
            get { return _Con; }
        }
        #endregion

        #region Public Methods

        #region SQL Queries
        /// <summary>
        /// Populates the referenced DataSet with data returned by specified select query. 
        /// Returns the number of rows affected by the query. It should also be noted that
        /// their stored procedure support is also somewhat limited.
        /// </summary>
        /// <param name="query">SQL query to run against the database</param>
        /// <param name="queryResult">DataSet with data returned by specified select query</param>
        /// <returns>The number of rows affected by the query</returns>
        public int Select(string query, ref DataSet queryResult)
        {
            _Cmd = _Provider.CreateCommand();
            _Cmd.Connection = _Con;
            _Cmd.CommandText = query;

            using (DbDataAdapter adapter = _Provider.CreateDataAdapter())
            {
                adapter.SelectCommand = _Cmd;
                _Con.Open();
                return adapter.Fill(queryResult);
            }
        }
        /// <summary>
        /// Populates the referenced DataTable with data returned by specified select query. 
        /// Returns the number of rows affected by the query.
        /// </summary>
        /// <param name="query">SQL query to run against the database</param>
        /// <param name="queryResult">DataTable with data returned by specified select query</param>
        /// <returns>The number of rows affected by the query</returns>
        public int Select(string query, ref DataTable queryResult)
        {
            _Cmd = _Provider.CreateCommand();
            _Cmd.Connection = _Con;
            _Cmd.CommandText = query;

            DbDataAdapter adapter = _Provider.CreateDataAdapter();
            adapter.SelectCommand = _Cmd;

            try
            {
                _Con.Open();

                try
                {
                    return adapter.Fill(queryResult);
                }
                catch (DbException err)
                {
                    throw err;
                }
            }
            catch (DbException err)
            {
                throw err;
            }
            finally
            {
                _Con.Close();
            }
        }

        /// <summary>
        /// Returns a DbDataReader associated to the specified query. Make sure to open the connection
        /// before executing this method.
        /// </summary>
        /// <param name="query">The SQL statement.</param>
        /// <returns>A DbDataReader associated to the specified query.</returns>
        public DbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandBehavior.Default);
        }

        /// <summary>
        /// Returns a DbDataReader associated to the specified query. Make sure to open the connection
        /// before executing this method.
        /// </summary>
        /// <param name="query">The SQL statement.</param>
        /// <param name="behavior">One of the System.Data.CommandBehavior values.</param>
        /// <returns>A DbDataReader associated to the specified query, null if the ConnectionState of
        /// the DB-Connection is not ConnectionState.Open.
        /// </returns>
        public DbDataReader ExecuteReader(string query, CommandBehavior behavior)
        {
            if (_Con.State != ConnectionState.Open)
                return null;

            _Cmd = _Provider.CreateCommand();
            _Cmd.Connection = _Con;
            _Cmd.CommandText = query;

            return _Cmd.ExecuteReader(behavior);
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <param name="query">The SQL statement.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string query)
        {
            _Cmd = _Provider.CreateCommand();
            _Cmd.Connection = _Con;
            _Cmd.CommandText = query;

            try
            {
                _Con.Open();
                return _Cmd.ExecuteNonQuery();
            }
            catch (DbException err)
            {
                throw err;
            }
            finally
            {
                _Con.Close();
            }
        }

        /// <summary>
        /// Executes the query and references the first column of the first row in the result
        /// set returned by the query to the scalar parameter.
        /// </summary>
        /// <param name="query">The SQL statement.</param>
        /// <param name="scalar">The returned scalar object.</param>
        /// <returns>The DbStatus of the executed SQL statement</returns>
        public DbStatus ExecuteScalar<T>(string query, ref T scalar)
        {
            _Cmd = _Provider.CreateCommand();
            _Cmd.Connection = _Con;
            _Cmd.CommandText = query;

            object obj = new object();
            try
            {
                _Con.Open();

                try
                {
                    obj = _Cmd.ExecuteScalar();
                }
                catch (DbException err)
                {
                    throw err;
                }
            }
            catch (DbException err)
            {
                throw err;
            }
            finally
            {
                _Con.Close();
            }

            if (obj == null)
                return DbStatus.NoObjectReturned;

            if (obj is T)
            {
                scalar = (T)obj;
                return DbStatus.Success;
            }
            else
            {
                return DbStatus.UnableToParseObject;
            }

        }
        #endregion

        #region General Methods
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection.
        /// </summary>
        /// <returns>A System.Data.DataTable that contains schema information.</returns>
        public DataTable GetSchemaTable()
        {
            return GetSchemaTable(null, null);
        }

        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name.
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return.</param>
        /// <returns>A System.Data.DataTable that contains schema information.</returns>
        public DataTable GetSchemaTable(string collectionName)
        {
            return GetSchemaTable(collectionName, null);
        }

        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name and the specified string array
        /// for the restriction values.
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return.</param>
        /// <param name="restrictionValues">TSpecifies a set of restriction values for the requested schema.</param>
        /// <returns>A System.Data.DataTable that contains schema information.</returns>
        public DataTable GetSchemaTable(string collectionName, string[] restrictionValues)
        {
            DataTable dt = new DataTable();

            try
            {
                _Con.Open();

                try
                {
                    dt = _Con.GetSchema(collectionName, restrictionValues);
                }
                catch (DbException err)
                {
                    throw err;
                }
            }
            catch (DbException err)
            {
                throw err;
            }
            finally
            {
                _Con.Close();
            }

            return dt;
        }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">The DB-ConnectionString to use.</param>
        /// <param name="providerInvariantName">The type of the provider. CodeReview: use Type instead?</param>
        public DataAccessFactory(string providerInvariantName, string connectionString)
        {
            _Provider = DbProviderFactories.GetFactory(providerInvariantName);
            _Con = _Provider.CreateConnection();
            _Con.ConnectionString = connectionString;
        }
        #endregion
    }
}
