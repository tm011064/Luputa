using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using CommonTools.Extensions;

namespace CommonTools.Data
{
    /// <summary>
    /// DataAccessManager provides methods for connecting to SQL Server
    /// database and running sprocs against that server. Includes support for Parameters.
    /// You will note that Sproc support isn't really supported by the database-agnostic
    /// DataAccessFactory, hence the reason for this class.
    /// </summary>
    public class DataAccessManager
    {
        #region Global Variables
        private string _ConnectionString;
        private SqlCommand _Cmd = new SqlCommand();
        private List<SqlParameter> _Parameters = new List<SqlParameter>();  // Lists all parameters for the sql-storedprocedure
        private List<string> _OutputParameters = new List<string>();
        #endregion

        #region Private methods
        /// <summary>
        /// What's this for?
        /// </summary>
        /// <param name="pEnumToTest"></param>
        /// <returns></returns>
        private bool IsFlaggedEnum(Enum pEnumToTest)
        {
            Type t = pEnumToTest.GetType();

            object[] attr = t.GetCustomAttributes(false);

            foreach (object enumAttribute in attr)
                if (enumAttribute is FlagsAttribute)
                    return true;

            return false;
        }
        #endregion

        #region public static methods
        /// <summary>|
        /// Throws the data access manager exception.
        /// </summary>
        /// <param name="err">The err.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        public static void ThrowDataAccessManagerException(SqlException err, string storedProcedureName)
        {
            switch (err.Number)
            {
                case 53:
                    throw new DataAccessManagerConnectionStringException("Could not connect to database.", err);
                case 2812:
                    throw new DataAccessManagerStoredProcedureException("Could not find stored procedure " + storedProcedureName, err);
                default:
                    throw new DataAccessManagerException("An SqlException exception occurred during execution of this database call. See inner exception for further details.", err);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method adds an sql-OUTPUT parameter
        /// </summary>
        /// <param name="parameterName">Name of the SQL parameter</param>
        /// <param name="dbType">SQLDbType of the SQL parameter</param>
        public void AddOutPutParameter(string parameterName, SqlDbType dbType)
        {
            SqlParameter sp = new SqlParameter();
            sp.SqlDbType = dbType;
            sp.Direction = ParameterDirection.Output;
            sp.ParameterName = parameterName;

            _Parameters.Add(sp);
            _OutputParameters.Add(parameterName);
        }
        /// <summary>
        /// This method adds an sql-OUTPUT parameter
        /// </summary>
        /// <param name="parameterName">Name of the SQL parameter</param>
        /// <param name="dbType">SQLDbType of the SQL parameter</param>
        /// <param name="Size">Size of the SQL parameter</param>
        public void AddOutPutParameter(string parameterName, SqlDbType dbType, int Size)
        {
            SqlParameter sp = new SqlParameter();
            sp.SqlDbType = dbType;
            sp.Direction = ParameterDirection.Output;
            sp.ParameterName = parameterName;
            sp.Size = Size;

            _Parameters.Add(sp);
            _OutputParameters.Add(parameterName);
        }
        /// <summary>
        /// This Method adds an sql-INPUT parameter
        /// </summary>
        /// <param name="parameterName">Name of the SQL parameter</param>
        /// <param name="value">Value of the SQL parameter</param>
        public void AddInputParameter(string parameterName, object value)
        {
            SqlParameter sp = new SqlParameter();
            sp.Direction = ParameterDirection.Input;
            sp.ParameterName = parameterName;

            if (value != null || value == DBNull.Value)
            {
                if (value is Enum)
                {
                    if (IsFlaggedEnum(value as Enum))
                    {
                        sp.SqlDbType = SqlDbType.Int;
                        sp.Value = value;
                    }
                }
                else
                {
                    switch (value.GetType().ToString())
                    {
                        case "System.Guid":
                            sp.SqlDbType = SqlDbType.UniqueIdentifier;
                            sp.Value = value;
                            break;
                        case "System.Decimal":
                            sp.SqlDbType = SqlDbType.Decimal;
                            sp.Value = value;
                            break;
                        case "System.Float":
                        case "System.Double":
                            sp.SqlDbType = SqlDbType.Float;
                            sp.Value = value;
                            break;
                        case "System.Boolean":
                            sp.SqlDbType = SqlDbType.Bit;
                            sp.Value = value;
                            break;
                        case "System.String":
                            sp.SqlDbType = SqlDbType.NVarChar;
                            sp.Value = value;
                            break;
                        case "System.Int32":
                            sp.SqlDbType = SqlDbType.Int;
                            sp.Value = value;
                            break;
                        case "System.Int16":
                            sp.SqlDbType = SqlDbType.SmallInt;
                            sp.Value = value;
                            break;
                        case "System.Int64":
                            sp.SqlDbType = SqlDbType.BigInt;
                            sp.Value = value;
                            break;
                        case "System.Byte":
                            sp.SqlDbType = SqlDbType.TinyInt;
                            sp.Value = value;
                            break;
                        case "System.DateTime":
                            sp.SqlDbType = SqlDbType.DateTime;
                            sp.Value = ((DateTime)value).ToCanonical();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                sp.SqlDbType = SqlDbType.NVarChar;
                sp.Value = DBNull.Value;
            }

            _Parameters.Add(sp);
        }

        #region stored procedure exectuion methods
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a datatable.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <returns>The datatable filled with the records selected by the stored procedure.</returns>
        public T ExecuteTableQuery<T>(string procedureName)
            where T : System.Data.DataTable, new()
        {
            int? returnValue = null;
            return ExecuteTableQuery<T>(procedureName, ref returnValue);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a datatable.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <returns>The datatable filled with the records selected by the stored procedure.</returns>
        public T ExecuteTableQuery<T>(string procedureName, ref int? returnValue)
            where T : System.Data.DataTable, new()
        {
            Dictionary<string, object> outputValues = new Dictionary<string, object>();
            return ExecuteTableQuery<T>(procedureName, ref returnValue, out outputValues);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a datatable.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="outputValues">A dictionary containing the values of all sql outputparameters.</param>
        /// <returns>The datatable filled with the records selected by the stored procedure.</returns>
        public T ExecuteTableQuery<T>(string procedureName, out Dictionary<string, object> outputValues)
            where T : System.Data.DataTable, new()
        {
            int? returnValue = null;
            return ExecuteTableQuery<T>(procedureName, ref returnValue, out outputValues);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a datatable.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <param name="outputValues">A dictionary containing the values of all sql outputparameters.</param>
        /// <returns>The datatable filled with the records selected by the stored procedure.</returns>
        public T ExecuteTableQuery<T>(string procedureName, ref int? returnValue, out Dictionary<string, object> outputValues)
            where T : System.Data.DataTable, new()
        {
            T returnSet = new T();
            outputValues = new Dictionary<string, object>();

            SqlConnection con = new SqlConnection(_ConnectionString);
            _Cmd.CommandText = procedureName;
            _Cmd.Connection = con;

            for (int i = 0; i < _Parameters.Count; i++)
            {
                _Cmd.Parameters.Add(_Parameters[i]);
            }

            SqlParameter retValue = null;
            if (returnValue.HasValue)
            {
                retValue = _Cmd.Parameters.Add("@RETURN_VALUE", System.Data.SqlDbType.Int);
                retValue.Direction = System.Data.ParameterDirection.ReturnValue;
            }

            _Cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(_Cmd);

            try
            {
                con.Open();
                adapter.Fill(returnSet);
            }
            catch (SqlException err)
            {
                ThrowDataAccessManagerException(err, procedureName);
            }
            finally
            {
                con.Close();
            }

            if (retValue != null && retValue.Value != null)
                returnValue = (int)(retValue.Value);

            foreach (string key in _OutputParameters)
                outputValues.Add(key, _Cmd.Parameters[key].Value);

            return returnSet;
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a dataset.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <returns>The dataset filled with the records selected by the stored procedure.</returns>
        public T ExecuteDatasetQuery<T>(string procedureName)
            where T : System.Data.DataSet, new()
        {
            int? returnValue = null;
            return ExecuteDatasetQuery<T>(procedureName, ref returnValue);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a dataset.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <returns>The dataset filled with the records selected by the stored procedure.</returns>
        public T ExecuteDatasetQuery<T>(string procedureName, ref int? returnValue)
            where T : System.Data.DataSet, new()
        {
            Dictionary<string, object> outputValues = new Dictionary<string, object>();
            return ExecuteDatasetQuery<T>(procedureName, ref returnValue, out outputValues);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a dataset.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="outputValues">A dictionary containing the values of all sql outputparameters.</param>
        /// <returns>The dataset filled with the records selected by the stored procedure.</returns>
        public T ExecuteDatasetQuery<T>(string procedureName, out Dictionary<string, object> outputValues)
            where T : System.Data.DataSet, new()
        {
            int? returnValue = null;
            return ExecuteDatasetQuery<T>(procedureName, ref returnValue, out outputValues);
        }
        /// <summary>
        /// This method executes a specified stored procedure and returns the selected record as a dataset.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <param name="outputValues">A dictionary containing the values of all sql outputparameters.</param>
        /// <returns>The dataset filled with the records selected by the stored procedure.</returns>
        public T ExecuteDatasetQuery<T>(string procedureName, ref int? returnValue, out Dictionary<string, object> outputValues)
            where T : System.Data.DataSet, new()
        {
            T returnSet = new T();
            outputValues = new Dictionary<string, object>();

            SqlConnection con = new SqlConnection(_ConnectionString);
            _Cmd.CommandText = procedureName;
            _Cmd.Connection = con;

            for (int i = 0; i < _Parameters.Count; i++)
            {
                _Cmd.Parameters.Add(_Parameters[i]);
            }

            SqlParameter retValue = null;
            if (returnValue.HasValue)
            {
                retValue = _Cmd.Parameters.Add("@RETURN_VALUE", System.Data.SqlDbType.Int);
                retValue.Direction = System.Data.ParameterDirection.ReturnValue;
            }

            _Cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(_Cmd);

            try
            {
                con.Open();
                adapter.Fill(returnSet);
            }
            catch (SqlException err)
            {
                ThrowDataAccessManagerException(err, procedureName);
            }
            finally
            {
                con.Close();
            }

            if (retValue != null && retValue.Value != null)
                returnValue = (int)(retValue.Value);

            foreach (string key in _OutputParameters)
                outputValues.Add(key, _Cmd.Parameters[key].Value);

            return returnSet;
        }
        /// <summary>
        /// This method executes a specified stored procedure that does not return any database record.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteNonQuery(string procedureName)
        {
            int? returnValue = null;
            return ExecuteNonQuery(procedureName, ref returnValue);
        }
        /// <summary>
        /// This method executes a specified stored procedure that does not return any database record.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteNonQuery(string procedureName, ref int? returnValue)
        {
            Dictionary<string, object> outDict = null;
            return ExecuteNonQuery(procedureName, ref returnValue, out outDict);
        }

        /// <summary>
        /// This method executes a specified stored procedure that does not return any database record.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure</param>
        /// <returns>The number of rows affected</returns>
        /// <param name="outputValues">A dictionary containing the values of all sql outputparameters.</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteNonQuery(string procedureName, ref int? returnValue, out Dictionary<string, object> outputValues)
        {
            SqlConnection con = new SqlConnection(_ConnectionString);
            outputValues = new Dictionary<string, object>();
            int affectedRows = 0;

            _Cmd.CommandText = procedureName;
            _Cmd.Connection = con;

            for (int i = 0; i < _Parameters.Count; i++)
            {
                _Cmd.Parameters.Add(_Parameters[i]);
            }

            SqlParameter retValue = null;
            if (returnValue.HasValue)
            {
                retValue = _Cmd.Parameters.Add("@RETURN_VALUE", System.Data.SqlDbType.Int);
                retValue.Direction = System.Data.ParameterDirection.ReturnValue;
            }

            _Cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                affectedRows = _Cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                ThrowDataAccessManagerException(err, procedureName);
            }
            finally
            {
                con.Close();
            }

            if (retValue != null && retValue.Value != null)
                returnValue = (int)(retValue.Value);

            foreach (string key in _OutputParameters)
                outputValues.Add(key, _Cmd.Parameters[key].Value);

            return affectedRows;
        }

        /// <summary>
        /// This method executes a stored procedure that expects only one scalar value to be returned. 
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <returns>Returns the T object selected by this stored procedure.</returns>
        public T ExecuteScalar<T>(string procedureName)
        {
            int? returnValue = null;
            return ExecuteScalar<T>(procedureName, ref returnValue);
        }
        /// <summary>
        /// This method executes a stored procedure that expects only one scalar value to be returned. 
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure.</param>
        /// <param name="returnValue">The return value returned by the stored procedure.</param>
        /// <returns>Returns the T object selected by this stored procedure.</returns>
        public T ExecuteScalar<T>(string procedureName, ref int? returnValue)
        {
            SqlConnection con = new SqlConnection(_ConnectionString);
            _Cmd.CommandText = procedureName;
            _Cmd.Connection = con;

            for (int i = 0; i < _Parameters.Count; i++)
            {
                _Cmd.Parameters.Add(_Parameters[i]);
            }

            SqlParameter retValue = null;
            if (returnValue.HasValue)
            {
                retValue = _Cmd.Parameters.Add("@RETURN_VALUE", System.Data.SqlDbType.Int);
                retValue.Direction = System.Data.ParameterDirection.ReturnValue;
            }

            _Cmd.CommandType = CommandType.StoredProcedure;
            object obj = new object();

            try
            {
                con.Open();
                obj = _Cmd.ExecuteScalar();
            }
            catch (SqlException err)
            {
                ThrowDataAccessManagerException(err, procedureName);
            }
            finally
            {
                con.Close();
            }
            if (retValue != null && retValue.Value != null)
                returnValue = (int)(retValue.Value);

            if (obj == null)
                return default(T);

            return (T)obj;
        }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">The DB-ConnectionString to use.</param>
        public DataAccessManager(string connectionString)
        {
            this._ConnectionString = connectionString;
        }
        #endregion
    }
}
