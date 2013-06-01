using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApplicationDataStorageManager<T>
    {
        /// <summary>
        /// Determines whether the underlying store exists
        /// </summary>
        /// <returns></returns>
        bool DoesStoreExist();

        /// <summary>
        /// Loads a data object from this instance's store
        /// </summary>
        /// <returns></returns>
        T Load();
        /// <summary>
        /// Saves a data object to this instance's store
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        bool Save(T obj);

        /// <summary>
        /// Tries the load a data object from this instance's store
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>true if successful, otherwise false</returns>
        bool TryLoad(out T obj, out string errorMessage);
        /// <summary>
        /// Tries the save a data object to this instance's store
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>true if successful, otherwise false</returns>
        bool TrySave(T obj, out string errorMessage);
    }
}
