using System;
using System.IO;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using ProtoBuf;

namespace CommonTools.Caching
{
    static class DistributedCache
    {
        #region members
        private static MemcachedClient _Client = new MemcachedClient();
        #endregion

        #region public methods

        #region inserts
        /// <summary>
        /// Store the data, but only if the server does not already hold data for a given key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns></returns>
        public static bool Add(string key, object value, DateTime expiresAt)
        {
            return _Client.Store(StoreMode.Add, key, value, expiresAt);
        }

        public static bool Replace(string key, object value, DateTime expiresAt)
        {
            return _Client.Store(StoreMode.Replace, key, value, expiresAt);
        }

        public static bool Insert(string key, byte[] value, int offset, int length, DateTime expiresAt)
        {
            return _Client.Store(StoreMode.Set, key, value, offset, length, expiresAt);
        }
        public static bool Insert(string key, object value, DateTime expiresAt)
        {
            return _Client.Store(StoreMode.Set, key, value, expiresAt);
        }

        public static bool InsertProtocolBufferObject<T>(string key, T value, DateTime expiresAt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, value);
                byte[] bytes = ms.GetBuffer();
                return _Client.Store(StoreMode.Set, key, bytes, 0, bytes.Length, expiresAt);
            }
        }
        public static T GetProtocolBufferObject<T>(string key)
        {
            byte[] bytes = _Client.Get<byte[]>(key);
            if (bytes != null)
            {
                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Position = 0;
                    return ProtoBuf.Serializer.Deserialize<T>(ms);
                }
            }

            return default(T);
        }
        #endregion

        public static object Get(string key)
        {
            return _Client.Get(key);
        }

        public static T Get<T>(string key)
        {
            return _Client.Get<T>(key);
        }

        public static bool Remove(string key)
        {
            return _Client.Remove(key);
        }

        public static void RemoveAll()
        {
            _Client.FlushAll();
        }
        #endregion
    }
}
