using System.IO;
using ProtoBuf;

namespace Workmate.Messaging
{
  public static class SerializationUtility
  {
    public static byte[] Serialize<T>(T obj)
    {
      byte[] bytes;
      using (MemoryStream ms = new MemoryStream())
      {
        Serializer.Serialize(ms, obj);
        bytes = ms.ToArray();
      }
      return bytes;
    }

    public static T Deserialize<T>(byte[] bytes)
    {
      T item;
      using (MemoryStream ms = new MemoryStream(bytes))
        item = Serializer.Deserialize<T>(ms);
      return item;
    }
  }
}
