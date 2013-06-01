using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace CommonTools.Components.Reflection
{
    /// <summary>
    /// This class has helper methods for reflection
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Creates an instance of a strongly typed DataSet's underlying data table. This method assumes that the given DataSet has 
        /// only one data table implemented -> otherwise it will return an instance of the first datatable of the DataSet.
        /// </summary>
        /// <param name="stronglyTypedDataSet">The type of the strongly typed data set.</param>
        /// <returns>The underlying data table.</returns>
        public static DataTable CreateDataTableInstance(Type stronglyTypedDataSet)
        {
            Type tempType = null;
            foreach (Type type in stronglyTypedDataSet.GetNestedTypes())
            {
                if (type.BaseType != typeof(System.Data.DataRow))
                {
                    tempType = type;
                    while (true)
                    {
                        tempType = tempType.BaseType;
                        if (tempType == null)
                            break;
                        if (tempType == typeof(System.Data.DataTable))
                        {
                            object dataTable = null;
                            dataTable = type.InvokeMember("", BindingFlags.CreateInstance, null, dataTable, null);

                            if (dataTable != null)
                                return (DataTable)dataTable;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Gets an instance of a given table adapter from it's type.
        /// </summary>
        /// <param name="tableAdapterType">Type of the table adapter.</param>
        /// <returns>An instance of a given table adapter from it's type</returns>
        public static Component CreateTableAdapterInstance(Type tableAdapterType)
        {
            object adapter = null;
            adapter = tableAdapterType.InvokeMember("", BindingFlags.CreateInstance, null, adapter, null);
            if (adapter != null)
                return (Component)adapter;

            return null;
        }
        /// <summary>
        /// Gets a type from a given assembly from it's string representation.
        /// </summary>
        /// <param name="typeName">The type.</param>
        /// <param name="assembly">The assembly to get the type from.</param>
        /// <returns>The type from a given assembly, null if the type could not be found.</returns>
        public static Type GetTypeFromAssembly(string typeName, Assembly assembly)
        {
            return GetTypeFromAssembly(typeName, assembly.GetExportedTypes());
        }
        /// <summary>
        /// Gets a type from a given assembly's exported types from it's string representation.
        /// </summary>
        /// <param name="typeName">The type.</param>
        /// <param name="assemblyTypes">The exported types of an assembly.</param>
        /// <returns>The type from a given assembly, null if the type could not be found.</returns>
        public static Type GetTypeFromAssembly(string typeName, Type[] assemblyTypes)
        {
            foreach (Type type in assemblyTypes)
            {
                if (type.Name == typeName || type.FullName == typeName)
                    return type;
            }

            return null;
        }
        /// <summary>
        /// This method returns an assembly without locking it.
        /// </summary>
        /// <param name="filename">The filename (full path).</param>
        /// <returns>An assembly extracted from the given filename.</returns>
        public static Assembly LoadAssembly(string filename)
        {
            byte[] bytes = null;
            FileStream fs = null;
            try
            {
                FileInfo fi = new FileInfo(filename);
                fs = fi.OpenRead();

                int length = (int)fi.Length;
                bytes = new byte[length];
                fs.Read(bytes, 0, length);

                if (bytes != null)
                {
                    return Assembly.Load(bytes);
                }
            }
            finally
            {
                fs.Close();
            }

            return null;
        }
    }
}
