using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CommonTools.IO
{
    /// <summary>
    /// This class handles application data storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApplicationDataStorageManager<T> : IApplicationDataStorageManager<T>
    {
        #region members
        private ApplicationDataStorageMode _ApplicationDataStorageMode;
        private string _FilePath;
        #endregion

        #region IApplicationDataStorageManager<T> Members

        /// <summary>
        /// Determines whether the underlying store exists
        /// </summary>
        /// <returns></returns>
        public bool DoesStoreExist()
        {
            return File.Exists(this._FilePath);
        }

        /// <summary>
        /// Loads a data object from this instance's store
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            T obj;
            string message;

            TryLoad(out obj, out message);

            return obj;
        }

        /// <summary>
        /// Tries the load a data object from this instance's store
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool TryLoad(out T obj, out string errorMessage)
        {
            obj = default(T);
            errorMessage = null;

            if (!DoesStoreExist())
                return false;

            try
            {
                using (FileStream stream = new FileStream(_FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    obj = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
                    stream.Close();
                }

                return true;
            }
            catch (Exception err)
            {
                errorMessage = err.Message;
            }

            return false;
        }

        /// <summary>
        /// Saves a data object to this instance's store
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public bool Save(T obj)
        {
            string message;
            return TrySave(obj, out message);
        }

        /// <summary>
        /// Tries the save a data object to this instance's store
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool TrySave(T obj, out string errorMessage)
        {
            if (obj == null)
                throw new ArgumentNullException();

            errorMessage = null;

            FileInfo fileInfo = new FileInfo(this._FilePath);
            if (!fileInfo.Directory.Exists)
            {
                DirectoryInfo directoryInfo = System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);
                if (!directoryInfo.Exists)
                    return false;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StreamWriter writer = new StreamWriter(_FilePath, false))
                {
                    serializer.Serialize((TextWriter)writer, obj);
                    writer.Close();
                }

                return true;
            }
            catch (Exception err)
            {
                errorMessage = err.Message;
            }

            return false;
        }

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDataStorageManager&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="applicationPath">The application path (eg.: "CompanyName\ApplicationName\Version")</param>
        /// <param name="fileName">Name of the data storage file.</param>
        /// <param name="applicationDataStorageMode">The application data storage mode.</param>
        public ApplicationDataStorageManager(string applicationPath, string fileName, ApplicationDataStorageMode applicationDataStorageMode)
        {
            this._ApplicationDataStorageMode = applicationDataStorageMode;

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(appDataFolder, applicationPath);

            switch (applicationDataStorageMode)
            {
                case ApplicationDataStorageMode.User: path = Path.Combine(path, Environment.UserName); break;
            }

            this._FilePath = Path.Combine(path, fileName);
        }
        #endregion
    }
}
