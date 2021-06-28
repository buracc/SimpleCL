using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SimpleCL.Util.Extension
{
    public static class CollectionExt
    {
        public static bool IsEmpty(this ICollection collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty(this ICollection collection)
        {
            return collection.Count > 0;
        }

        public static Dictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source[k]);
        }
        
        public static string Print(this NameValueCollection collection)
        {
            return string.Join(",", collection.AllKeys.Select(key => collection[key]));
        }
    }
}