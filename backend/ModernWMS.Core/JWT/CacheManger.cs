using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ModernWMS.Core.JWT
{
    public class CacheManager
    {

        public static CacheManager Default = new CacheManager();

        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public CacheManager()
        {

        }

        /// <summary>
        /// get value by key
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            T value;
            _cache.TryGetValue<T>(key, out value);
            return value;
        }


        /// <summary>
        /// set cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Set_NotExpire<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            T v;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set(key, value);
        }

        /// <summary>
        /// set cache with expire
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Set_SlidingExpire<T>(string key, T value, TimeSpan span)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            T v;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = span
            });
        }


        public void Set_AbsoluteExpire<T>(string key, T value, TimeSpan span)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            T v;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set(key, value, span);
        }

        public void Set_SlidingAndAbsoluteExpire<T>(string key, T value, TimeSpan slidingSpan, TimeSpan absoluteSpan)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            T v;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = slidingSpan,
                AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(absoluteSpan.TotalMilliseconds)
            });
        }

        /// <summary>
        /// remove cache by key
        /// </summary> 
        /// <param name="key">key</param>
        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }

        #region TokenHelper
        public bool Is_Token_Exist<T>(int userID, string type, int expireMinute)
        {
            var  key = $"ModernWMS_{type}_{userID}";
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            T value;
            if (_cache.TryGetValue<T>(key, out value))
            {
                Set_SlidingExpire(key, value,  TimeSpan.FromMinutes(expireMinute) );
                return true;
            }
            return false;
        }
        public async Task<bool> TokenSet(int userID, string type, string token, int expireMinute)
        {
            string key = $"ModernWMS_{type}_{userID}";
            try
            {
                Set_AbsoluteExpire(key, token, TimeSpan.FromMinutes(expireMinute));
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

