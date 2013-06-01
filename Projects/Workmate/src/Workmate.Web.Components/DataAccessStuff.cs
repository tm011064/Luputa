using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;

namespace Workmate.Web.Components
{
  public static class Extensions
  {
    public static List<dynamic> ToDynamic(this IDataReader dataReader)
    {
      var result = new List<dynamic>();
      while (dataReader.Read())
      {
        dynamic item = new ExpandoObject();
        var dc = (IDictionary<string, object>)item;
        int count = dataReader.FieldCount;
        for (int i = 0; i < count; i++)
          dc.Add(dataReader.GetName(i), dataReader[i]);
        result.Add(item);
      }
      return result;
    }
  }
}
