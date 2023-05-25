using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace BigglzPetJson.Dll.Util
{
    public static class CControlsUtil
    {
        /// <summary>
        /// Jarray에 Jobject Add
        /// </summary>
        public static void AddJobject(this JArray self, string key, string value)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add(key, value);
                self.Add(jObject);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Jarray에 Jobject Add
        /// </summary>
        public static void AddJobject(this JArray self, string key, int value)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add(key, value);
                self.Add(jObject);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Jarray에 Jobject Add
        /// </summary>
        public static void AddJobject(this JArray self, string key, JArray value)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add(key, value);
                self.Add(jObject);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Jarray에 Jobject Add
        /// </summary>
        public static void AddJobject(this JArray self, string key, DateTime value)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add(key, value);
                self.Add(jObject);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// index 포함 foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
         => self.Select((item, index) => (item, index));
    }
}
