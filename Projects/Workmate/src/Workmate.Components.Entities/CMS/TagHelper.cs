using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Workmate.Components.Entities.CMS
{
  public static class TagHelper
  {
    internal const string TRIPLE_TAG = "wm:{0}={1}";
    internal const string TRIPLE_TAG_REGEX = "^wm\\:([a-zA-Z0-9]*)\\=([a-zA-Z0-9_\\-]*)$";
    internal const string TRIPLE_TAG_REGEX_PLACEHOLDER = "^wm\\:{0}\\=([a-zA-Z0-9_\\-]*)$";

    internal static Regex _TripleTagRegex = new Regex("^wm\\:([a-zA-Z0-9]*)\\=([a-zA-Z0-9_\\-]*)$");
    internal static Regex TripleTagRegex
    {
      get { return TagHelper._TripleTagRegex; }
    }
    internal static KeyValuePair<string, string>? GetTripleTagKeyValuePair(string tag)
    {
      if (TagHelper.TripleTagRegex.IsMatch(tag))
      {
        string[] array = TagHelper.TripleTagRegex.Split(tag);
        if (array.Length == 4)
        {
          return new KeyValuePair<string, string>?(new KeyValuePair<string, string>(array[1], array[2]));
        }
      }
      return null;
    }
    internal static string GetValueFromTripleTag(string tagString, string tripleTagKey, char separator)
    {
      string[] array = new string[0];
      if (tagString != null)
      {
        array = tagString.Split(new char[]
				{
					separator
				});
      }
      Regex regex = new Regex(string.Format("^fl\\:{0}\\=([a-zA-Z0-9_\\-]*)$", tripleTagKey));
      string[] array2 = array;
      for (int i = 0; i < array2.Length; i++)
      {
        string input = array2[i];
        if (regex.IsMatch(input))
        {
          string[] array3 = regex.Split(input);
          if (array3.Length == 3)
          {
            return array3[1];
          }
        }
      }
      return null;
    }

    private static string TrimSafely(this string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }
      return text.Trim();
    }

    public static void FormatTags(List<string> tags, out List<string> normalTags, out List<KeyValuePair<string, string>> tripleTags)
    {
      normalTags = new List<string>();
      tripleTags = new List<KeyValuePair<string, string>>();
      foreach (string current in tags)
      {
        KeyValuePair<string, string>? tripleTagKeyValuePair = TagHelper.GetTripleTagKeyValuePair(current);
        if (tripleTagKeyValuePair.HasValue)
        {
          tripleTags.Add(tripleTagKeyValuePair.Value);
        }
        else
        {
          normalTags.Add(current);
        }
      }
    }
    public static string GetTagString(List<string> tags, char separator)
    {
      return TagHelper.GetTagString(tags, separator, true);
    }
    public static string GetTagString(List<string> tags, char separator, bool excludeTripleTags)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string current in tags)
      {
        if (!excludeTripleTags || !TagHelper.TripleTagRegex.IsMatch(current))
        {
          stringBuilder.Append(current + separator.ToString());
        }
      }
      return stringBuilder.ToString().TrimEnd(new char[]
			{
				separator
			});
    }
    public static string GetTagString(List<string> tags, char separator, List<string> tripleTagsFilter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string current in tags)
      {
        KeyValuePair<string, string>? tripleTagKeyValuePair = TagHelper.GetTripleTagKeyValuePair(current);
        if (tripleTagKeyValuePair.HasValue)
        {
          if (tripleTagsFilter.Contains(tripleTagKeyValuePair.Value.Key))
          {
            stringBuilder.Append(current + separator.ToString());
          }
        }
        else
        {
          stringBuilder.Append(current + separator.ToString());
        }
      }
      return stringBuilder.ToString().TrimEnd(new char[]
			{
				separator
			});
    }
    public static List<string> GetTags(string tagString, char separator)
    {
      return TagHelper.GetTags(tagString, separator, null);
    }
    public static List<string> GetTags(string tagString, char separator, List<KeyValuePair<string, string>> tripleTags)
    {
      List<string> list = new List<string>();
      string[] array = new string[0];
      if (tagString != null)
      {
        array = tagString.Split(new char[]
				{
					separator
				});
      }
      string[] array2 = array;
      for (int i = 0; i < array2.Length; i++)
      {
        string text = array2[i];
        string text2 = text.TrimSafely();
        if (!string.IsNullOrEmpty(text2))
        {
          list.Add(text2);
        }
      }
      if (tripleTags != null)
      {
        foreach (KeyValuePair<string, string> current in tripleTags)
        {
          string text2 = current.Key.TrimSafely();
          string text3 = current.Value.TrimSafely();
          if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
          {
            list.Add(string.Format("fl:{0}={1}", text2, text3));
          }
        }
      }
      return list;
    }
    public static List<string> GetTagsFromQuery(string query, char tagSeparator)
    {
      List<string> list = new List<string>();
      query = query.TrimSafely();
      if (string.IsNullOrEmpty(query))
      {
        return list;
      }
      query = query.Replace(tagSeparator, ' ');
      List<string> list2 = new List<string>();
      List<string> list3 = new List<string>();
      string[] array = query.Split(new char[]
			{
				'"'
			});
      if (query.IndexOf("\"") >= 0)
      {
        if (!query.StartsWith("\""))
        {
          list3.Add(query.Remove(query.IndexOf("\"")));
        }
        bool flag = false;
        for (int i = 1; i < array.Length; i++)
        {
          if (i + 1 < array.Length && !flag)
          {
            list2.Add(array[i].Trim());
          }
          else
          {
            list3.Add(array[i].Trim());
          }
          flag = !flag;
        }
      }
      else
      {
        list3.Add(query);
      }
      foreach (string current in list3)
      {
        string[] array2 = current.Split(new char[]
				{
					' '
				});
        for (int j = 0; j < array2.Length; j++)
        {
          string text = array2[j];
          if (!string.IsNullOrEmpty(text))
          {
            list.Add(text);
          }
        }
      }
      foreach (string current2 in list2)
      {
        if (!string.IsNullOrEmpty(current2))
        {
          list.Add(current2);
        }
      }
      return list;
    }
    public static string GetFormattedTripleTag(string tripleTagKey, string value)
    {
      return string.Format("fl:{0}={1}", tripleTagKey, value);
    }
  }
}