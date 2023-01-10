
using System.Data;
using Newtonsoft.Json;

namespace ModernWMS.Core.Utility
{
    /// <summary>
    /// JSON Helper
    /// </summary>
    public static class JsonHelper
    {
        #region JSON Helper

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="obj">data</param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj
                , new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                });
        }

        /// <summary>
        /// Serialize to DataTable
        /// </summary>
        /// <param name="table">data</param>
        /// <param name="replaceNullToEmpty">""</param>
        /// <returns></returns>
        public static string SerializeDataTable(DataTable table,bool replaceNullToEmpty = false)
        {
            string json = JsonConvert.SerializeObject(table
                , new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                });
            if (replaceNullToEmpty)
            {
                json = json.Replace("null", "\"\"");
            }
            return json;
        }
        #endregion

    }
}
