
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Data;

namespace OrderSort.Common
{
    public static class JsonHelper
    {

        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static object DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(json);
        }

        public static string SerializeToJson(object data, string DateTimeFormats = "yyyy-MM-dd HH:mm:ss")
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = DateTimeFormats };
            return JsonConvert.SerializeObject(data, Formatting.Indented, timeConverter);
        }

        public static object DeserializeObject(string json, Type type)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(json, type);
        }

        public static string SerializeObject(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static bool IsJArray(object obj)
        {
            return obj is Newtonsoft.Json.Linq.JArray;
        }

        public static bool IsJValue(object obj)
        {
            return obj is Newtonsoft.Json.Linq.JValue;
        }

        /// <summary>
        /// 根据DataTable返回json字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
        }
    }
   
}
