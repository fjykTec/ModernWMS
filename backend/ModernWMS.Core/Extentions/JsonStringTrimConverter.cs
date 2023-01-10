using Newtonsoft.Json;
using System;

namespace ModernWMS.Core.Extentions
{
    /// <summary>
    /// JSON extentions
    /// </summary>
    public class JsonStringTrimConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        /// <summary>
        ///  Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader"> The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(string) && reader.Value != null)
            {
                if (reader.TokenType == JsonToken.Date)
                {
                    return Convert.ToDateTime(reader.Value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return (reader.Value as string).Trim();
                }
            }
            return reader.Value;
        }
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = (string)value;
            if (text == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(text.Trim());
            }
        }
    }
}
