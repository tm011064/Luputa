using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Runtime.Serialization.Json;

namespace CommonTools.Components.Testing
{
  /// <summary>
  /// This class provides help methods to facilitate debugging.
  /// </summary>
  public static class DebugUtility
  {
    #region validation
    /// <summary>
    /// Compares the property values of two objects.
    /// </summary>
    /// <param name="a">The object to compare with the other object</param>
    /// <param name="b">The object to compare with the other object</param>
    /// <param name="unequalProperties">A list of all unequal property values.</param>
    /// <returns>
    /// true if the property values of the specified objects are equal, otherwise false.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Throws System.ArgumentNullException if one or both of the specified objects is null.</exception>
    ///   
    /// <exception cref="System.ArgumentException">Throws System.ArgumentException if the objects are not from the same type</exception>
    public static bool ArePropertyValuesEqual(object a, object b, out string unequalProperties)
    {
      return ArePropertyValuesEqual(a, b, TimeSpan.Zero, out unequalProperties);
    }
    /// <summary>
    /// Compares the property values of two objects.
    /// </summary>
    /// <param name="a">The object to compare with the other object</param>
    /// <param name="b">The object to compare with the other object</param>
    /// <param name="dateTimeWindow">The date time window.</param>
    /// <param name="unequalProperties">A list of all unequal property values.</param>
    /// <returns>
    /// true if the property values of the specified objects are equal, otherwise false.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Throws System.ArgumentNullException if one or both of the specified objects is null.</exception>
    ///   
    /// <exception cref="System.ArgumentException">Throws System.ArgumentException if the objects are not from the same type</exception>
    public static bool ArePropertyValuesEqual(object a, object b, TimeSpan dateTimeWindow, out string unequalProperties)
    {
      if (a == null)
        throw new ArgumentNullException("Object a must not be null when comparing properties to another object");
      if (b == null)
        throw new ArgumentNullException("Object b must not be null when comparing properties to another object");

      if (a.GetType().FullName != b.GetType().FullName)
        throw new ArgumentException("Objects have to be of the same type.");

      unequalProperties = string.Empty;

      PropertyInfo[] propertyInfosA = a.GetType().GetProperties();
      PropertyInfo[] propertyInfosB = b.GetType().GetProperties();

      bool areEqual = true;

      object valueA, valueB;

      PropertyInfo infoB;
      foreach (PropertyInfo infoA in propertyInfosA)
      {
        infoB = (from c in propertyInfosB
                 where c.Name == infoA.Name
                 select c).SingleOrDefault();

        try { valueA = infoA.GetValue(a, null); }
        catch (TargetInvocationException) { valueA = null; }
        try { valueB = infoB.GetValue(b, null); }
        catch (TargetInvocationException) { valueB = null; }

        if (valueA != null && valueB != null)
        {
          if ((valueA == null && valueB != null)
              || (valueA != null && valueB == null))
          {
            areEqual = false;
            unequalProperties += infoA.Name + ": " + (valueA == null ? "NULL" : valueA.ToString()) + " != " + (valueB == null ? "NULL" : valueB.ToString()) + "\n";
          }
          else
          {
            if (dateTimeWindow != TimeSpan.Zero
                && infoA.PropertyType == typeof(DateTime)
                && infoB.PropertyType == typeof(DateTime))
            {
              DateTime da = (DateTime)valueA;
              DateTime db = (DateTime)valueB;

              double tsPos = Math.Abs(dateTimeWindow.TotalMilliseconds) * -1;
              double tsNeg = Math.Abs(dateTimeWindow.TotalMilliseconds);

              if (da.AddMilliseconds(tsNeg) > db
               || da.AddMilliseconds(tsPos) < db)
              {
                areEqual = false;
                unequalProperties += infoA.Name + ": " + (valueA == null ? "NULL" : valueA.ToString()) + " != " + (valueB == null ? "NULL" : valueB.ToString()) + "\n";
              }
            }
            else if (valueA.ToString() != valueB.ToString())
            {
              areEqual = false;
              unequalProperties += infoA.Name + ": " + (valueA == null ? "NULL" : valueA.ToString()) + " != " + (valueB == null ? "NULL" : valueB.ToString()) + "\n";
            }
          }
        }
      }

      return areEqual;
    }
    #endregion

    #region random value generator

    /// <summary>
    /// Gets a random enum value from the specified generic Enum.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="random">The random used to generate the random Enum from.</param>
    /// <returns>A random enum value</returns>
    /// <exception cref="System.ArgumentException">Throws a System.ArgumentException if the specified generic type is not an enum.</exception>
    public static U GetRandomEnum<U>(Random random)
    {
      if (typeof(U).BaseType != typeof(Enum))
        throw new ArgumentException("Specified generic type was not of type enum.");

      string[] names = Enum.GetNames(typeof(U));
      return (U)Enum.Parse(typeof(U), names[random.Next(0, names.Length)]);
    }

    /// <summary>
    /// Gets a random enum value from the specified generic Enum.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="random">The random used to generate the random Enum from.</param>
    /// <param name="exclude">The Enum value to from all possible enum values.</param>
    /// <returns>A random enum value</returns>
    /// <exception cref="System.ArgumentException">Throws a System.ArgumentException if the specified generic type is not an enum.</exception>
    public static U GetRandomEnum<U>(Random random, U exclude)
    {
      if (typeof(U).BaseType != typeof(Enum))
        throw new ArgumentException("Specified generic type was not of type enum.");

      string[] names = Enum.GetNames(typeof(U));

      if (names.Length > 1)
      {
        while (true)
        {
          U newType = (U)Enum.Parse(typeof(U), names[random.Next(0, names.Length)]);
          if (newType.ToString() != exclude.ToString())
            return newType;
        }
      }

      return (U)Enum.Parse(typeof(U), names[random.Next(0, names.Length)]);
    }

    /// <summary>
    /// Generates a random true or false boolean
    /// </summary>
    /// <param name="random">The random used to generate the random boolean from.</param>
    /// <returns>Either true or false</returns>
    public static bool FlipCoin(Random random)
    {
      return (random.Next(0, 2) == 0);
    }
    /// <summary>
    /// Generates a random true or false boolean, going in favor of the specified probability.
    /// </summary>
    /// <param name="random">The random used to generate the random boolean from.</param>
    /// <param name="probability">This value defines the probability that the returned value will be true. Set it to a value between 0 and 1.</param>
    /// <returns>Either true or false</returns>
    public static bool FlipRiggedCoin(Random random, double probability)
    {
      return (random.NextDouble() <= probability);
    }

    /// <summary>
    /// Gets a list of enums of a specified enum type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">Throws a System.ArgumentException if the specified generic type is not an enum.</exception>
    public static List<T> GetEnums<T>()
    {
      if (typeof(T).BaseType != typeof(Enum))
        throw new ArgumentException("Specified generic type was not of type enum.");

      List<T> returnList = new List<T>();
      foreach (string name in Enum.GetNames(typeof(T)))
        returnList.Add((T)Enum.Parse(typeof(T), name));
      return returnList;
    }

    /// <summary>
    /// Gets a list of enum types of a specified enum the filtered enums. The probability can be set in order to
    /// set the probablity of which an enum will occur in the return list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="random">The random.</param>
    /// <param name="probability">The probability.</param>
    /// <returns></returns>
    public static List<T> GetFilteredEnums<T>(Random random, double probability)
    {
      List<T> returnList = new List<T>();
      foreach (T obj in GetEnums<T>())
      {
        if (FlipRiggedCoin(random, probability))
          returnList.Add(obj);
      }

      return returnList;
    }
    #endregion

    #region objects
    /// <summary>
    /// Gets all the enum names of a specified enum type as a human readable string.
    /// </summary>
    /// <param name="enumType">Type of the enum.</param>
    /// <returns></returns>
    public static string GetDebugString(Type enumType)
    {
      StringBuilder sb = new StringBuilder();
      foreach (string name in Enum.GetNames(enumType))
        sb.Append(name + ", ");
      return sb.ToString().Trim().TrimEnd(',');
    }

    /// <summary>
    /// Gets all the enumerable object's values as a human readable string.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public static string GetDebugString(IEnumerable obj)
    {
      StringBuilder sb = new StringBuilder();
      IEnumerator enumerator = obj.GetEnumerator();
      while (enumerator.MoveNext())
      {
        sb.Append(DebugUtility.GetObjectString(enumerator.Current) + ", ");
      }
      return sb.ToString().Trim().TrimEnd(',');
    }
    /// <summary>
    /// Gets all the collection's values as a human readable string.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public static string GetDebugString(ICollection obj)
    {
      StringBuilder sb = new StringBuilder();
      IEnumerator enumerator = obj.GetEnumerator();
      while (enumerator.MoveNext())
      {
        sb.Append(enumerator.Current.ToString() + ", ");
      }
      return sb.ToString().Trim().TrimEnd(',');
    }


    /// <summary>
    /// Gets the object's property values formatted in a human readable string.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public static string GetObjectString(object obj)
    {
      return GetObjectString(obj, TextFormat.ASCII);
    }
    /// <summary>
    /// Gets the object string.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <param name="textFormat">The text format.</param>
    /// <returns></returns>
    public static string GetObjectString(object obj, TextFormat textFormat)
    {
      StringBuilder sb = new StringBuilder();

      PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
      object value = null;
      string formattedValue = string.Empty;
      foreach (PropertyInfo info in propertyInfos)
      {
        value = null;
        formattedValue = string.Empty;
        try { value = info.GetValue(obj, null); }
        catch (TargetInvocationException) { }

        if (value == null)
          formattedValue = "null";
        else
        {
          if (info.PropertyType.IsEnum)
            formattedValue = Enum.GetName(info.PropertyType, value);
          else if (value is ICollection)
            formattedValue = DebugUtility.GetDebugString((ICollection)value);
          else if (value is IEnumerable && !(value is string))
            formattedValue = DebugUtility.GetDebugString((IEnumerable)value);
          else
            formattedValue = value.ToString();
        }

        if (formattedValue == null)
          formattedValue = "null";

        switch (textFormat)
        {
          case TextFormat.ASCII: sb.Append(string.Format("\n{0}: {1}", info.Name, formattedValue)); break;
          case TextFormat.HTML: sb.Append(string.Format("<tr><td><strong>{0}</strong></td><td>{1}</td></tr>", info.Name, formattedValue)); break;
        }

      }
      switch (textFormat)
      {
        case TextFormat.HTML: sb.Insert(0, "<table>").Append("</table>"); break;
      }
      return sb.ToString().Trim().TrimEnd(',');
    }


    /// <summary>
    /// Traces the data contract serializable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The obj.</param>
    public static void TraceDataContractSerializableObject<T>(T obj)
    {
      Type type = typeof(T);
      DataContractSerializer dataContractSerializer = new DataContractSerializer(type);
      using (StringWriter sw = new StringWriter())
      {
        using (XmlWriter xmlWriter = XmlWriter.Create(Console.Out, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true }))
        {
          dataContractSerializer.WriteObject(xmlWriter, obj);
        }
      }
    }

    /// <summary>
    /// Traces the data contract json serializable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public static string TraceDataContractJsonSerializableObject<T>(T obj)
    {
      Type type = typeof(T);
      DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(type);

      string json;
      using (MemoryStream ms = new MemoryStream())
      {
        dataContractJsonSerializer.WriteObject(ms, obj);
        ms.Position = 0;
        json = Encoding.Default.GetString(ms.ToArray());
      }
      Trace.WriteLine(json);

      return json;
    }
    #endregion

    #region timing
    /// <summary>
    /// 
    /// </summary>
    public delegate void MethodToExecuteDelegate();
    /// <summary>
    /// Stops the time.
    /// </summary>
    /// <param name="methodToExecute">The method to execute.</param>
    /// <param name="repetitions">The repetitions.</param>
    /// <returns></returns>
    public static ElapsedTimeInfo StopTime(MethodToExecuteDelegate methodToExecute, int repetitions)
    {
      if (repetitions <= 0)
        throw new ArgumentOutOfRangeException("You need to set the repetitions to a positive value");

      Stopwatch sw = new Stopwatch();
      long[] totalMilliseconds = new long[repetitions];

      for (int i = 0; i < repetitions; i++)
      {
        sw.Reset();
        sw.Start();
        methodToExecute.Invoke();
        sw.Stop();

        totalMilliseconds[i] = sw.ElapsedMilliseconds;
      }

      return new ElapsedTimeInfo(totalMilliseconds.Average(), totalMilliseconds[repetitions / 2], repetitions, totalMilliseconds.Sum());
    }

    #endregion
  }
}
