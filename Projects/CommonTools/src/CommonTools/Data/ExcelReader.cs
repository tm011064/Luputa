using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace CommonTools.Data
{
    /// <summary>
    /// This method can be used to access excel files via an OleDbConnection. The machine the program runs on does not need to have excel installed 
    /// in order to access and read excel files. In general, an ExcelReader instance is designed to last for one "unit of work" however your application 
    /// defines that term. A DataContext is lightweight and is not expensive to create. A typical application creates ExcelReader instances at method 
    /// scope or as a member of short-lived classes that represent a logical set of related database operations.
    /// </summary>
    public class ExcelReader : IDisposable
    {
        #region members
        private string _ConnectionString;
        private HashSet<string> _WorkSheetNames = new HashSet<string>();
        private Dictionary<string, DataTable> _DataTables = new Dictionary<string, DataTable>();
        private Regex _CellRegex = new Regex(@"^([A-Za-z]+)([0-9]+)$");
        private string _LocalCopyFilePath;
        #endregion

        #region private methods
        private string GetColumnName(int index)
        {
            string sColName = "";
            if (index < 26)
                sColName = Convert.ToString(Convert.ToChar((Convert.ToByte((char)'A') + index)));
            else
            {
                int intFirst = ((int)index / 26);
                int intSecond = ((int)index % 26);
                sColName = Convert.ToString(Convert.ToByte((char)'A') + intFirst);
                sColName += Convert.ToString(Convert.ToByte((char)'A') + intSecond);
            }
            return sColName;
        }

        private int GetColumnIndex(string name)
        {
            name = name.ToUpperInvariant();
            int intColNumber = 0;
            if (name.Length > 1)
            {
                intColNumber = Convert.ToInt16(Convert.ToByte(name[1]) - 65);
                intColNumber += Convert.ToInt16(Convert.ToByte(name[1]) - 64) * 26;
            }
            else
                intColNumber = Convert.ToInt16(Convert.ToByte(name[0]) - 65);
            return intColNumber;
        }

        private DataTable LoadWorksheet(string worksheetName)
        {
            OleDbConnection connection = new System.Data.OleDb.OleDbConnection(_ConnectionString);
            OleDbDataAdapter cmd = null;
            
            try
            {
                cmd = new System.Data.OleDb.OleDbDataAdapter(string.Format("SELECT * FROM [{0}$]", worksheetName), connection);
                connection.Open();

                DataTable dt = new DataTable();
                cmd.Fill(dt);

                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    try { connection.Dispose(); }
                    catch { }
                }
                if (cmd != null)
                {
                    try { cmd.Dispose(); }
                    catch { }
                }
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Iterates over worksheet rows.
        /// </summary>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public IEnumerable<object[]> IterateOverWorksheetRows(string worksheetName, int startIndex)
        {
            return IterateOverWorksheetRows(worksheetName, startIndex, -1);
        }
        /// <summary>
        /// Iterates the over worksheet rows.
        /// </summary>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <param name="startRowNumber">The start row number.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public IEnumerable<object[]> IterateOverWorksheetRows(string worksheetName, int startRowNumber, int count)
        {
            DataTable dt = GetWorksheet(worksheetName);
            if (dt != null)
            {
                int rowCount = dt.Rows.Count;
                if (startRowNumber < rowCount)
                {
                    int endIndex = startRowNumber - 1 + count;
                    if (count < 0
                        || endIndex > rowCount)
                    {
                        endIndex = rowCount;
                    }
                    for (int i = startRowNumber - 1; i < endIndex; i++)
                    {
                        yield return dt.Rows[i].ItemArray;
                    }
                }
            }
        }

        /// <summary>
        /// Tries the read and parse a specified cell value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue<T>(string worksheetName, string cell, out T value)
        {
            DataTable dt = GetWorksheet(worksheetName);
            if (dt == null)
            {
                value = default(T);
                return false;
            }

            Match match = _CellRegex.Match(cell);
            if (!match.Success)
            {
                value = default(T);
                return false;
            }

            int columnIndex = GetColumnIndex(match.Groups[1].Value);

            int rowCount = dt.Rows.Count;
            int columnCount = dt.Columns.Count;
            int rowIndex;
            if (!int.TryParse(match.Groups[2].Value, out rowIndex)
                || rowIndex > rowCount
                || columnIndex >= columnCount
                )
            {
                value = default(T);
                return false;
            }
            else
            {
                rowIndex--;
            }

            return ConversionHelper.TryParse<T>(dt.Rows[rowIndex][columnIndex].ToString(), out value);
        }

        /// <summary>
        /// Gets the worksheet names.
        /// </summary>
        /// <returns></returns>
        public string[] GetWorksheetNames()
        {
            return _WorkSheetNames.ToArray();
        }

        /// <summary>
        /// Gets a specified worksheet.
        /// </summary>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <returns></returns>
        public DataTable GetWorksheet(string worksheetName)
        {
            worksheetName = worksheetName.ToUpperInvariant();

            if (!_WorkSheetNames.Contains(worksheetName))
                return null;

            if (!_DataTables.ContainsKey(worksheetName))
            {
                DataTable dt = null;

                try { dt = LoadWorksheet(worksheetName); }
                catch { }

                _DataTables.Add(worksheetName, dt);
            }

            return _DataTables[worksheetName];
        }
        #endregion

        #region constructors
        private void Initialize(string fullFilePath, bool isTempFileCopy)
        {
            FileInfo fileInfo = new FileInfo(fullFilePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("The file " + fullFilePath + " was not found or is inaccessible");

            switch (fileInfo.Extension)
            {
                case ".xlsx":
                    _ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO;\"", fullFilePath);
                    break;

                case ".xls":
                default:
                    _ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO;\"", fullFilePath);
                    break;
            }

            OleDbConnection connection = new OleDbConnection(_ConnectionString);
            DataTable dataTable = null;
            try
            {
                connection.Open();

                //get all the available sheets
                dataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //get the number of sheets in the file
                _WorkSheetNames = new HashSet<string>();
                string name;
                foreach (DataRow row in dataTable.Rows)
                {
                    name = row["TABLE_NAME"].ToString().Trim('$');
                    if (name.StartsWith("'")
                        && name.EndsWith("$'"))
                    {
                        name = name.Substring(1,  name.Length - 3);
                    }

                    _WorkSheetNames.Add(name.ToUpperInvariant());
                }
            }
            catch (OleDbException dbException)
            {
                if (!isTempFileCopy
                    && dbException.Errors.Count > 0)
                {
                    if (dbException.Errors[0].NativeError == -67568648)
                    {// the file is opened by someone else with a lock, in that case we will copy the content to a local file which we will delete later on...

                        _LocalCopyFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid().ToString() + fileInfo.Extension);

                        File.Copy(fullFilePath, _LocalCopyFilePath);
                        Initialize(_LocalCopyFilePath, true);

                        return;
                    }
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (connection != null)
                {
                    try { connection.Dispose(); }
                    catch { }
                }
                if (dataTable != null)
                {
                    try { dataTable.Dispose(); }
                    catch { }
                }
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelReader"/> class.
        /// </summary>
        /// <param name="fullFilePath">The full file path.</param>
        public ExcelReader(string fullFilePath) : this(fullFilePath, true) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelReader"/> class.
        /// </summary>
        /// <param name="fullFilePath">The full file path of the excel file.</param>
        /// <param name="tryLocalCopyIfFileIsLocked">if set to true, the excel file is copied to a local file in order to circumvent files that are locked. 
        /// If this option is used, you have to call the Dispose() method when the reader is not used any more, otherwise the local file copy will 
        /// not be deleted.</param>
        public ExcelReader(string fullFilePath, bool tryLocalCopyIfFileIsLocked)
        {
            Initialize(fullFilePath, !tryLocalCopyIfFileIsLocked);
        }
        #endregion

        #region IDisposable Members
        private bool _IsDisposed = false;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;

                if (!string.IsNullOrEmpty(_LocalCopyFilePath))
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        try { File.Delete(_LocalCopyFilePath); }
                        catch { }
                    });

                }
            }
        }

        #endregion
    }
}
