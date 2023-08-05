using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MorningStar.Common
{
    public static class JsonConverterExtension
    {
        /// <summary>
        /// 添加 long/long? 类型序列化处理
        /// </summary>
        /// <remarks></remarks>
        public static IList<JsonConverter> AddLongTypeConverters(this IList<JsonConverter> converters)
        {
            converters.Add(new NewtonsoftJsonLongToStringJsonConverter());
            converters.Add(new NewtonsoftJsonNullableLongToStringJsonConverter());

            return converters;
        }
    }

    /// <summary>
    /// 解决 long 精度问题
    /// </summary>
    public class NewtonsoftJsonLongToStringJsonConverter : JsonConverter<long>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jt = JValue.ReadFrom(reader);
            return jt.Value<long>();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }

    /// <summary>
    /// 解决 long? 精度问题
    /// </summary>
    public class NewtonsoftJsonNullableLongToStringJsonConverter : JsonConverter<long?>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jt = JValue.ReadFrom(reader);
            return jt.Value<long?>();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString());
        }
    }
}
