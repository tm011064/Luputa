using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using System.IO;

namespace CodeGenerationTool.Components
{
  public static class CodeGenerator
  {
    #region members
    static List<Regex> _Regexes = new List<Regex>()
    {
      new Regex(@"\[\b([{a-zA-Z0-1_}]+)\b\]"), // " [a]        "
      new Regex(@"as\s+'([{a-zA-Z0-1_ }]+)'", RegexOptions.IgnoreCase),// "table.ColumnName as 'MyAlias Hi'"
      new Regex(@"as\s+([{a-zA-Z0-1_}]+)", RegexOptions.IgnoreCase), // "table.ColumnName as MyAlias"
      new Regex(@"\.([{a-zA-Z0-1_}]+)"), // "table.ColumnName"
      new Regex(@"[^@]\b([{a-zA-Z0-1_}]+)\b"), // any value but ", @somevalue        "
      new Regex(@"^([{a-zA-Z0-1_}]+)$") // "abc"
    };

    #region templates

    #endregion

    #endregion

    #region sql helpers
    private static StoredProcedure GetStoredProcedure(string connection, string spName, string schemaName)
    {
      Server server = new Server(new ServerConnection(new SqlConnection(connection)));
      SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connection);
      Database database = new Database(server, connectionStringBuilder.InitialCatalog);

      StoredProcedure sp = new StoredProcedure(database, spName, schemaName);
      sp.Refresh();

      return sp;
    }
    private static Table GetTable(string connection, string tableName, string schemaName)
    {
      Server server = new Server(new ServerConnection(new SqlConnection(connection)));
      SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connection);
      Database database = new Database(server, connectionStringBuilder.InitialCatalog);
      Table table = new Table(database, tableName, schemaName);
      table.Refresh();

      return table;
    }
    #endregion

    #region helper methods
    
    private static List<string> GetParameters(StoredProcedure sp)
    {
      List<string> list = new List<string>();
      foreach (StoredProcedureParameter parameter in sp.Parameters)
      {
        list.Add((parameter.IsOutputParameter ? "out " : string.Empty) + " " + FormatMethodParameter(parameter.Name.Substring(1)));
      }
      return list;
    }
    private static List<string> GetMethodParameters(StoredProcedure sp)
    {
      List<string> list = new List<string>();
      foreach (StoredProcedureParameter parameter in sp.Parameters)
      {
        string str = GetColumnCodeType(parameter.DataType.SqlDataType, true);
        list.Add((parameter.IsOutputParameter ? "out " : string.Empty) + str + " " + FormatMethodParameter(parameter.Name.Substring(1)));
      }
      return list;
    }
    private static List<string> GetMethodParameters(Table table)
    {
      List<string> list = new List<string>();
      foreach (Column column in table.Columns)
      {
        string str = GetColumnCodeType(column.DataType.SqlDataType, column.Nullable);
        list.Add(str + " " + FormatMethodParameter(column.Name));
      }
      return list;
    }

    private static string GetColumnCodeType(SqlDataType sqlDataType, bool isNullable)
    {
      string nullableText = isNullable ? "?" : string.Empty;
      switch (sqlDataType)
      {
        case SqlDataType.BigInt: return "long" + nullableText;
        case SqlDataType.Binary: return "byte[]";
        case SqlDataType.Bit: return "bool" + nullableText;
        case SqlDataType.Char: return "string";
        case SqlDataType.DateTime: return "DateTime" + nullableText;
        case SqlDataType.Decimal: return "decimal" + nullableText;
        case SqlDataType.Float: return "double" + nullableText;
        case SqlDataType.Int: return "int" + nullableText;
        case SqlDataType.NChar: return "string";
        case SqlDataType.NVarChar: return "string";
        case SqlDataType.NVarCharMax: return "string";
        case SqlDataType.SmallDateTime: return "DateTime" + nullableText;
        case SqlDataType.SmallInt: return "short" + nullableText;
        case SqlDataType.TinyInt: return "byte" + nullableText;
        case SqlDataType.UniqueIdentifier: return "Guid" + nullableText;
        case SqlDataType.VarBinary: return "byte[]";
        case SqlDataType.VarBinaryMax: return "byte[]";
        case SqlDataType.VarChar: return "string";
        case SqlDataType.VarCharMax: return "string";
        case SqlDataType.Xml: return "string";
      }

      return string.Empty;
    }

    private static List<string> GetParameterAliases(List<string> parameters)
    {
      List<string> list = new List<string>();
      string returnName;
      Match match;
      int index;
      foreach (string str in parameters)
      {
        returnName = str.Trim();
        index = returnName.IndexOf(',');
        if (index >= 0 && index < returnName.Length)
          returnName = returnName.Substring(index + 1);

        index = returnName.LastIndexOf(')');
        if (index >= 0 && index < returnName.Length)
          returnName = returnName.Substring(index + 1);
        returnName = returnName.Trim();

        foreach (Regex regex in _Regexes)
        {
          match = regex.Match(returnName);
          if (match.Success)
          {
            list.Add(match.Groups[1].Value);
            break;
          }
        }
      }
      return list;
    }
    private static string FormatMethodParameter(string parameter)
    {
      if (string.IsNullOrEmpty(parameter))
        return parameter;

      return parameter.Substring(0, 1).ToLower()
        + (parameter.Length > 1 ? parameter.Substring(1) : string.Empty);
    }
    #endregion

    #region stored procedures

    #region sp gets
    private static List<string> GetOutputParameters(StoredProcedure sp)
    {
      List<string> list = new List<string>();
      foreach (StoredProcedureParameter parameter in sp.Parameters)
      {
        if (!parameter.IsOutputParameter)
          continue;

        string dbTypeString = parameter.DataType.SqlDataType.ToString();
        if (dbTypeString.EndsWith("Max"))
          dbTypeString.TrimEnd("Max".ToCharArray());
        
        string str = GetColumnCodeType(parameter.DataType.SqlDataType, true);
        list.Add(string.Format(@"{0} = ({1})cmd.Parameters[""{2}""].Value;"
          , FormatMethodParameter(parameter.Name.Substring(1))
          , str
          , parameter.Name));
      }
      return list;
    }

    private static List<string> GetSelectVariables(StoredProcedure sp)
    {
      List<string> list = new List<string>();
      int index = 0
        , length = sp.TextBody.Length;
      Dictionary<int, string> indexes = new Dictionary<int, string>();

      Regex selectRegex = new Regex(@"\bSELECT\b", RegexOptions.IgnoreCase);
      Regex fromRegex = new Regex(@"\bFROM\b", RegexOptions.IgnoreCase);
      Match match;

      index = 0;
      while (true)
      {
        match = selectRegex.Match(sp.TextBody, index);
        if (match.Success)
        {
          indexes.Add(match.Groups[0].Index, "SELECT");
          index = match.Groups[0].Index + 6;
        }
        else
          break;
      }
      index = 0;
      while (true)
      {
        match = fromRegex.Match(sp.TextBody, index);
        if (match.Success)
        {
          indexes.Add(match.Groups[0].Index, "FROM");
          index = match.Groups[0].Index + 4;
        }
        else
          break;
      }

      int bracketCount = 0;
      int startIndex = -1
          , endIndex = -1;
      foreach (int key in indexes.Keys.OrderByDescending(c => c))
      {
        if (indexes[key] == "SELECT")
        {
          if (bracketCount <= 1)
          {
            startIndex = key;
            break;
          }
          else
            bracketCount--;
        }

        if (indexes[key] == "FROM")
        {
          if (bracketCount == 0)
            endIndex = key;

          bracketCount++;
        }
      }

      if (startIndex > 0 && endIndex > startIndex)
      {
        startIndex += 6;
        string selectStatement = sp.TextBody.Substring(startIndex, endIndex - startIndex);

        selectStatement = selectStatement.Replace(Environment.NewLine, string.Empty);
        length = selectStatement.Length;
        StringReader sr = new StringReader(selectStatement);
        int intCharacter;
        char convertedCharacter;
        List<string> returnParameters = new List<string>();
        StringBuilder sb = new StringBuilder();

        bracketCount = 0;
        while (true)
        {
          intCharacter = sr.Read();
          if (intCharacter == -1) break;

          convertedCharacter = Convert.ToChar(intCharacter);
          switch (convertedCharacter)
          {
            case ',':
              if (bracketCount == 0)
              {
                returnParameters.Add(sb.ToString());
                sb.Remove(0, sb.Length);
              }
              break;

            case '(': bracketCount++; break;
            case ')': bracketCount--; break;

          }
          sb.Append(convertedCharacter);
        }
        if (sb.Length > 0)
          returnParameters.Add(sb.ToString());

        foreach (string alias in GetParameterAliases(returnParameters))
        {
          list.Add(@"record." + alias + @" = dr[""" + alias + @"""] == DBNull.Value ? null : (__VALUETYPE__)dr[""" + alias + @"""];");
        }
      }

      return list;
    }
    private static List<string> GetCommandParameters(StoredProcedure sp)
    {
      List<string> list = new List<string>();
      foreach (StoredProcedureParameter parameter in sp.Parameters)
      {
        string dbTypeString = parameter.DataType.SqlDataType.ToString();
        if (dbTypeString.EndsWith("Max"))
          dbTypeString.TrimEnd("Max".ToCharArray());

        list.Add(string.Format(@"cmd.Parameters.Add(new SqlParameter(""{0}"", SqlDbType.{1}, {2}, ParameterDirection.{3}, {4}, {5}, {6}, {7}, {8}, {9}));"
           , parameter.Name
           , parameter.DataType.SqlDataType.ToString()
           , parameter.Properties["Length"].Value
           , parameter.IsOutputParameter ? "Output" : "Input"
           , "false"
           , "((byte)(" + parameter.DataType.NumericPrecision + "))"
           , "((byte)(" + parameter.DataType.NumericScale + "))"
           , "string.Empty"
           , "DataRowVersion.Default"
           , parameter.IsOutputParameter ? "null"
                                         : FormatMethodParameter(parameter.Name.Substring(1)) + " == null ? (object)DBNull.Value : " + FormatMethodParameter(parameter.Name.Substring(1))));
      }
      return list;
    }
    #endregion

    public static string GetStoredProcedureWrapperCode(string connection, string spName, string schemaName, string template)
    {
      StoredProcedure sp = GetStoredProcedure(connection, spName, schemaName);

      StringBuilder methodParameters = new StringBuilder();
      foreach (string s in GetMethodParameters(sp))
        methodParameters.Append(", " + s);
      StringBuilder parameters = new StringBuilder();
      foreach (string s in GetParameters(sp))
        parameters.Append(", " + s);
      StringBuilder commandParameters = new StringBuilder();
      foreach (string s in GetCommandParameters(sp))
        commandParameters.AppendLine("      " + s);
      StringBuilder selectVariables = new StringBuilder();
      foreach (string s in GetSelectVariables(sp))
        selectVariables.AppendLine("          " + s);
      StringBuilder outputParameters = new StringBuilder();
      foreach (string s in GetOutputParameters(sp))
        outputParameters.AppendLine("  " + s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# storedProcdureName #>", spName)
                     .Replace("<# dataReaderTemplates #>", selectVariables.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# methodParameters #>", methodParameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()).TrimStart(", ".ToCharArray()))
                     .Replace("<# parameters #>", parameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()).TrimStart(", ".ToCharArray()))
                     .Replace("<# commandParameters #>", commandParameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# outputParameters #>", outputParameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }
    #endregion

    #region crud

    #region general
    private static string GetDataTypeDeclaration(DataType dataType)
    {
      string result = dataType.Name.ToUpper();
      switch (dataType.SqlDataType)
      {
        case SqlDataType.Binary:
        case SqlDataType.Char:
        case SqlDataType.NChar:
        case SqlDataType.NVarChar:
        case SqlDataType.VarBinary:
        case SqlDataType.VarChar:
          result += string.Format("({0})", dataType.MaximumLength);
          break;

        case SqlDataType.NVarCharMax:
        case SqlDataType.VarBinaryMax:
        case SqlDataType.VarCharMax:
          result += "(max)";
          break;

        case SqlDataType.Decimal:
        case SqlDataType.Numeric:
          result += string.Format("({0}, {1})", dataType.NumericPrecision, dataType.NumericScale);
          break;
      }
      return result;
    }

    private static string GetEmptyString(int length)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < length; i++)
        sb.Append(" ");
      return sb.ToString();
    }

    /// <summary>
    /// Generates where clause for UPDATE and DELETE statements for the specified
    /// table.
    /// </summary>
    private static List<string> GetWhereClause(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        if (!column.InPrimaryKey)
          continue;

        name = column.Name;
        parameters.Add(new string[2] { name, "= @" + name });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 3);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}[{2}]{3}{4}"
          , isFirst ? string.Empty : indent
          , (i > 0) ? "AND " : "    "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
        isFirst = false;
      }
      return list;
    }
    #endregion

    #region insert sp
    private static List<string> GetInsertParameters(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        if (column.InPrimaryKey == true)
          continue;

        name = "@" + column.Name;
        parameters.Add(new string[2] { name, GetDataTypeDeclaration(column.DataType) });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent);
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}{2}{3}{4}"
          , indent
          , (i > 0) ? ", " : "  "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
      }
      return list;
    }

    /// <summary>
    /// Writes list of column names for the INSERT statement
    /// </summary>
    private static List<string> GetInsertClause(Table table, int tabIndent)
    {
      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 2);
      int count = table.Columns.Count;
      bool isFirst = true;

      for (int i = 0; i < count; i++)
      {
        Column column = table.Columns[i];
        if (column.InPrimaryKey == true)
          continue;

        list.Add(string.Format("{0}{1}[{2}]"
          , indent
          , isFirst ? "  " : ", "
          , column.Name));
        isFirst = false;
      }
      return list;
    }

    /// <summary>
    /// Writes list of parameter names for VALUES clause of the INSERT statement
    /// </summary>
    private static List<string> GetInsertValuesClause(Table table, int tabIndent)
    {
      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 2);
      int count = table.Columns.Count;
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        Column column = table.Columns[i];
        if (column.InPrimaryKey == true)
          continue;

        list.Add(string.Format("{0}{1}@{2}"
          , indent
          , isFirst ? "  " : ", "
          , column.Name));

        isFirst = false;
      }
      return list;
    }

    public static string GetInsertStoredProcedure(string connectionString, string tableName, string schemaName, int tabIndent, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder parameters = new StringBuilder();
      foreach (string s in GetInsertParameters(table, tabIndent))
        parameters.AppendLine(s);
      StringBuilder insertClause = new StringBuilder();
      foreach (string s in GetInsertClause(table, tabIndent))
        insertClause.AppendLine(s);
      StringBuilder valuesClause = new StringBuilder();
      foreach (string s in GetInsertValuesClause(table, tabIndent))
        valuesClause.AppendLine(s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# tableName #>", tableName)
                     .Replace("<# insertParameters #>", parameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# insertClause #>", insertClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# insertValues #>", valuesClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }
    #endregion

    #region update
    private static List<string> GetUpdateParameters(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        name = "@" + column.Name;
        parameters.Add(new string[2] { name, GetDataTypeDeclaration(column.DataType) });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}{2}{3}{4}"
          , isFirst ? string.Empty : indent
          , (i > 0) ? ", " : "  "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
        isFirst = false;
      }
      return list;
    }

    /// <summary>
    /// Writes set clause of the update statement.
    /// </summary>
    private static List<string> GetUpdateSetClause(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        if (column.InPrimaryKey == true)
          continue;

        name = column.Name;
        parameters.Add(new string[2] { name, "= @" + name });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 5);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}[{2}]{3}{4}"
          , isFirst ? string.Empty : indent
          , (i > 0) ? ", " : "  "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
        isFirst = false;
      }
      return list;
    }

    public static string GetUpdateStoredProcedure(string connectionString, string tableName, string schemaName, int tabIndent, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder parameters = new StringBuilder();
      foreach (string s in GetUpdateParameters(table, tabIndent))
        parameters.AppendLine(s);
      StringBuilder setClause = new StringBuilder();
      foreach (string s in GetUpdateSetClause(table, tabIndent))
        setClause.AppendLine(s);
      StringBuilder whereClause = new StringBuilder();
      foreach (string s in GetWhereClause(table, tabIndent))
        whereClause.AppendLine(s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# tableName #>", tableName)
                     .Replace("<# updateParameters #>", parameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# updateSetParameters #>", setClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# whereClause #>", whereClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }
    #endregion

    #region select
    private static List<string> GetSelectParameters(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        if (column.InPrimaryKey == false)
          continue;

        name = "@" + column.Name;
        parameters.Add(new string[2] { name, GetDataTypeDeclaration(column.DataType) });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}{2}{3}{4}"
          , isFirst ? string.Empty : indent
          , (i > 0) ? ", " : "  "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
        isFirst = false;
      }
      return list;
    }

    /// <summary>
    /// Writes set clause of the update statement.
    /// </summary>
    private static List<string> GetSelectValues(Table table, int tabIndent)
    {
      List<string> parameters = new List<string>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        name = column.Name;
        parameters.Add(name);

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 5);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}[{2}]"
          , isFirst ? string.Empty : indent
          , (i > 0) ? ", " : "  "
          , parameters[i]
        ));
        isFirst = false;
      }
      return list;
    }

    public static string GetSelectStoredProcedure(string connectionString, string tableName, string schemaName, int tabIndent, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder parameters = new StringBuilder();
      foreach (string s in GetSelectParameters(table, tabIndent))
        parameters.AppendLine(s);
      StringBuilder selectValues = new StringBuilder();
      foreach (string s in GetSelectValues(table, tabIndent))
        selectValues.AppendLine(s);
      StringBuilder whereClause = new StringBuilder();
      foreach (string s in GetWhereClause(table, tabIndent))
        whereClause.AppendLine(s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# tableName #>", tableName)
                     .Replace("<# selectParameters #>", parameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# selectValues #>", selectValues.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# whereClause #>", whereClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }

    #endregion

    #region select all
    /// <summary>
    /// Writes set clause of the update statement.
    /// </summary>
    private static List<string> GetSelectAllValues(Table table, int tabIndent)
    {
      List<string> parameters = new List<string>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];
        name = column.Name;
        parameters.Add(name);

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent * 5);
      bool isFirst = true;
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}[{2}]"
          , isFirst ? string.Empty : indent
          , (i > 0) ? ", " : "  "
          , parameters[i]
        ));
        isFirst = false;
      }
      return list;
    }

    public static string GetSelectAllStoredProcedure(string connectionString, string tableName, string schemaName, int tabIndent, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder selectValues = new StringBuilder();
      foreach (string s in GetSelectAllValues(table, tabIndent))
        selectValues.AppendLine(s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# tableName #>", tableName)
                     .Replace("<# selectAllValues #>", selectValues.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }

    #endregion

    #region delete
    private static List<string> GetDeleteParameters(Table table, int tabIndent)
    {
      List<string[]> parameters = new List<string[]>();
      string name;
      int maxNameLength = 0;
      for (int i = 0; i < table.Columns.Count; i++)
      {
        Column column = table.Columns[i];

        if (!column.InPrimaryKey)
          continue;

        name = "@" + column.Name;
        parameters.Add(new string[2] { name, GetDataTypeDeclaration(column.DataType) });

        if (maxNameLength < name.Length)
          maxNameLength = name.Length;
      }

      int count = parameters.Count;
      maxNameLength = ((maxNameLength / tabIndent) + 1) * tabIndent;

      List<string> list = new List<string>();
      string indent = GetEmptyString(tabIndent);
      for (int i = 0; i < count; i++)
      {
        list.Add(string.Format("{0}{1}{2}{3}{4}"
          , indent
          , (i > 0) ? ", " : "  "
          , parameters[i][0]
          , GetEmptyString(maxNameLength - parameters[i][0].Length)
          , parameters[i][1]
        ));
      }
      return list;
    }
    public static string GetDeleteStoredProcedure(string connectionString, string tableName, string schemaName, int tabIndent, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder parameters = new StringBuilder();
      foreach (string s in GetDeleteParameters(table, tabIndent))
        parameters.AppendLine(s);
      StringBuilder whereClause = new StringBuilder();
      foreach (string s in GetWhereClause(table, tabIndent))
        whereClause.AppendLine(s);

      return template.Replace("<# schemaName #>", schemaName)
                     .Replace("<# tableName #>", tableName)
                     .Replace("<# deleteParameters #>", parameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()))
                     .Replace("<# whereClause #>", whereClause.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }
    #endregion

    #endregion

    #region classes

    private static List<string> GetClassProperties(Table table, string template)
    {
      List<string> list = new List<string>();
      foreach (Column column in table.Columns)
      {
        string str = GetColumnCodeType(column.DataType.SqlDataType, column.Nullable);
        list.Add(template.Replace("<# propertyType #>", str)
                         .Replace("<# propertyName #>", column.Name));
      }
      return list;
    }
    private static List<string> GetClassPropertyAssignments(Table table)
    {
      List<string> list = new List<string>();
      foreach (Column column in table.Columns)
      {
        list.Add(string.Format("this.{0} = {1};"
          , column.Name
          , FormatMethodParameter(column.Name))
        );
      }
      return list;
    }
    private static List<string> GetClassConstructorParameters(Table table)
    {
      List<string> list = new List<string>();
      foreach (Column column in table.Columns)
      {
        list.Add(string.Format("this.{0} = {1};"
          , column.Name
          , FormatMethodParameter(column.Name))
        );
      }
      return list;
    }


    class RepeatingGroupInfo
    {
      public int StartIndex { get; set; }
      public int EndIndex { get; set; }
      public int InnerOpenBracketIndex { get; set; }
      public int InnerCloseBracketIndex { get; set; }

      public string Template { get; set; }

      public bool IsValid()
      {
        return this.StartIndex >= 0
               && this.EndIndex >= 0
               && this.InnerOpenBracketIndex >= 0
               && this.InnerCloseBracketIndex >= 0
               && this.EndIndex > this.StartIndex
               && this.InnerCloseBracketIndex > this.InnerOpenBracketIndex;
      }

      public RepeatingGroupInfo(int startIndex)
      {
        this.StartIndex = startIndex;
        this.EndIndex = int.MinValue;
        this.InnerOpenBracketIndex = int.MinValue;
        this.InnerCloseBracketIndex = int.MinValue;
      }
    }
    private static string ReplaceClassCodeProperties(Table table, string template)
    {
      int length = template.Length;
      int index = 0;

      List<RepeatingGroupInfo> repeatingGroupInfos = new List<RepeatingGroupInfo>();
      RepeatingGroupInfo repeatingGroupInfo;
      int bracketCount = 0;
      int placeHolderTagCount = 0;
      while (index >= 0)
      {
        index = template.IndexOf("<# properties", index);
        if (index >= 0)
        {
          bracketCount = 0;
          placeHolderTagCount = 1;

          repeatingGroupInfo = new RepeatingGroupInfo(index);
          for (int i = index + 13; i < length - 1; i++)
          {
            if (template[i] == '{')
            {
              if (bracketCount == 0)
                repeatingGroupInfo.InnerOpenBracketIndex = i;

              bracketCount++;
            }
            if (template[i] == '}')
            {
              bracketCount--;

              if (bracketCount == 0)
                repeatingGroupInfo.InnerCloseBracketIndex = i;
            }

            if (template.Substring(i, 2) == "<#")
            {
              placeHolderTagCount++;
            }
            if (template.Substring(i, 2) == "#>")
            {
              placeHolderTagCount--;

              if (placeHolderTagCount == 0)
              {
                repeatingGroupInfo.EndIndex = i + 1;
                break; // exit
              }
            }
          }

          if (repeatingGroupInfo.IsValid())
          {
            repeatingGroupInfo.Template = template.Substring(repeatingGroupInfo.InnerOpenBracketIndex + 1, repeatingGroupInfo.InnerCloseBracketIndex - repeatingGroupInfo.InnerOpenBracketIndex - 1);
            repeatingGroupInfos.Insert(0, repeatingGroupInfo);
            index = repeatingGroupInfo.EndIndex;
          }
        }
      }

      StringBuilder sb = new StringBuilder();
      foreach (RepeatingGroupInfo item in repeatingGroupInfos)
      {
        sb.Remove(0, sb.Length);
        foreach (string s in GetClassProperties(table, item.Template))
        {
          sb.Append(s);
        }
        template = template.Remove(item.StartIndex, item.EndIndex - item.StartIndex + 1)
                           .Insert(item.StartIndex, sb.ToString());
      }

      return template;
    }

    public static string GetClassCode(string connectionString, string tableName, string schemaName, string template)
    {
      Table table = GetTable(connectionString, tableName, schemaName);

      StringBuilder classPropertyAssignments = new StringBuilder();
      foreach (string s in GetClassPropertyAssignments(table))
        classPropertyAssignments.AppendLine(s);
      StringBuilder constructorParameters = new StringBuilder();
      foreach (string s in GetMethodParameters(table))
        constructorParameters.Append(", " + s);

      template = ReplaceClassCodeProperties(table, template);

      return template.Replace("<# className #>", tableName)
                     .Replace("<# constructorParameters #>", constructorParameters.ToString().TrimEnd(Environment.NewLine.ToCharArray()).TrimStart(", ".ToCharArray()))
                     .Replace("<# constructorPropertyAssignments #>", classPropertyAssignments.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
    }

    #endregion
  }
}
