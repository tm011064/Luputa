using System;
using System.Xml;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using CommonTools.Components.Logging;
using System.Xml.Linq;
using System.Reflection;

namespace CommonTools.Xml
{
    /// <summary>
    /// This class exposes general help methods dealing with xml serialization.
    /// </summary>
    /// <typeparam name="T">The returntype used for some serialization methods.</typeparam>
    public static class XmlSerializationHelper<T>
    {
        #region static methods
        /// <summary>
        /// Saves an object as it's serialized XML represenetation at a given file.
        /// </summary>
        /// <param name="settings">The T object to serialize.</param>
        /// <param name="file">The file to save the serialized object to.</param>
        /// <returns>true is successfull, otherwise false.</returns>
        public static bool SaveAsXmlFile(T settings, string file)
        {
            string s = string.Empty;
            return SaveAsXmlFile(settings, file, out s);
        }
        /// <summary>
        /// Saves an object as it's serialized XML represenetation at a given file.
        /// </summary>
        /// <param name="settings">The T object to serialize.</param>
        /// <param name="file">The file to save the serialized object to.</param>
        /// <param name="errormessage">The errormessage if an error occurred. Returns string.Empty if successfull.</param>
        /// <returns>true is successfull, otherwise false.</returns>
        public static bool SaveAsXmlFile(T settings, string file, out string errormessage)
        {
            errormessage = string.Empty;

            if (!string.IsNullOrEmpty(file))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        serializer.Serialize((TextWriter)writer, settings);
                        writer.Close();
                    }

                    return true;
                }
                catch (Exception err)
                {
                    errormessage = err.Message;
                }
            }
            else
                errormessage = "Specified filename string was null or empty.";

            return false;
        }
        /// <summary>
        /// This method converts a T object to an XElement
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns></returns>
        public static XElement ConvertToXElement(T obj)
        {
            if (obj != null)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (Stream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, obj);
                        stream.Position = 0;
                        return XElement.Load(XmlReader.Create(stream));
                    }
                }
                catch { return null; }
            }

            return null;
        }
        /// <summary>
        /// This method serializes a T object to a xml string representation.
        /// </summary>
        /// <param name="settings">The T object to serialize</param>
        /// <returns>The xml representation of the specified object, string.Empty if an error occurred.</returns>
        public static string ConvertToXml(T settings)
        {
            string xml = string.Empty;
            if (settings != null)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                    {
                        serializer.Serialize((TextWriter)writer, settings);
                        xml = writer.ToString();
                        writer.Close();
                    }
                }
                catch { xml = string.Empty; }
            }

            return xml;
        }
        /// <summary>
        /// This method serializes a T object to a xml string representation.
        /// </summary>
        /// <param name="settings">The T object to serialize</param>
        /// <param name="omitNamespace">if set to <c>true</c> the namespace declaration withing the root node will be omitted.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> the xml declaration at the beginning of the string will be omitted.</param>
        /// <returns>
        /// The xml representation of the specified object, string.Empty if an error occurred.
        /// </returns>
        public static string ConvertToXml(T settings, bool omitNamespace, bool omitXmlDeclaration)
        {
            string xml = string.Empty;
            if (settings != null)
            {
                try
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                    {

                        if (omitNamespace)
                        {
                            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                            ns.Add("", "");
                            serializer.Serialize((TextWriter)writer, settings, ns);
                        }
                        else
                        {
                            serializer.Serialize((TextWriter)writer, settings);
                        }

                        xml = writer.ToString();
                        writer.Close();
                    }

                    if (omitXmlDeclaration)
                        xml = xml.Remove(0, xml.IndexOf('\n')).Trim();
                }
                catch { xml = string.Empty; }
            }

            return xml;
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation.
        /// </summary>
        /// <param name="xml">The serialized xml representation of this T object</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromXml(string xml)
        {
            string s = string.Empty;
            return ConvertFromXml(xml, out s);
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation.
        /// </summary>
        /// <param name="xml">The serialized xml representation of this T object</param>
        /// <param name="errormessage">The errormessage when an error occurred. Returns string.Empty if the conversion succeeded.</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromXml(string xml, out string errormessage)
        {
            errormessage = string.Empty;

            if (!string.IsNullOrEmpty(xml))
            {
                T returnObject;
                try
                {
                    using (StringReader reader = new StringReader(xml))
                    {
                        returnObject = (T)(new XmlSerializer(typeof(T)).Deserialize(reader));
                        reader.Close();
                    } return returnObject;
                }
                catch (Exception err)
                {
                    errormessage = err.Message;
                }
            }
            else
                errormessage = "Specified xml representation string was null or empty.";

            return default(T);
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation.
        /// </summary>
        /// <param name="element">The XElement representation of this T object</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromXml(XElement element)
        {
            string s = string.Empty;
            return ConvertFromXml(element, out s);
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation.
        /// </summary>
        /// <param name="element">The XElement representation of this T object</param>
        /// <param name="errormessage">The errormessage when an error occurred. Returns string.Empty if the conversion succeeded.</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromXml(XElement element, out string errormessage)
        {
            errormessage = string.Empty;

            if (element != null)
            {
                T returnObject;
                try
                {
                    returnObject = (T)(new XmlSerializer(typeof(T)).Deserialize(element.CreateReader()));
                    return returnObject;
                }
                catch (Exception err)
                {
                    errormessage = err.Message;
                }
            }
            else
                errormessage = "Specified xml representation string was null or empty.";

            return default(T);
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation stored at a specified file.
        /// </summary>
        /// <param name="filename">The file (absolute path) that holds the serialized xml representation of this T object.</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromFile(string filename)
        {
            string s = string.Empty;
            return ConvertFromFile(filename, out s);
        }
        /// <summary>
        /// This method returns a T object from it's serialized xml representation stored at a specified file.
        /// </summary>
        /// <param name="filename">The file (absolute path) that holds the serialized xml representation of this T object.</param>
        /// <param name="errormessage">The errormessage when an error occurred. Returns string.Empty if the conversion succeeded.</param>
        /// <returns>The T object, null if an error occurred.</returns>
        public static T ConvertFromFile(string filename, out string errormessage)
        {
            errormessage = string.Empty;

            if (!string.IsNullOrEmpty(filename))
            {
                T returnObject;
                try
                {
                    using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        returnObject = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
                        stream.Close();
                    }
                    return returnObject;
                }
                catch (Exception err)
                {
                    errormessage = ConversionHelper.GetFormattedException("", err);
                }
            }
            else
                errormessage = "Specified filename string was null or empty.";

            return default(T);
        }
        #endregion
    }
}

