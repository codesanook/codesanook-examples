using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codesanook.Examples.DotNetCore.JSON
{
    // https://www.codeproject.com/Articles/1079324/Reflection-deserialization-and-custom-attributes
    // https://stackoverflow.com/a/45284854/1872200
    public class ArrayAsStringConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(IEnumerable<>);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var listItem = value as IEnumerable<T>;
            var listAsJsonString = JsonConvert.SerializeObject(listItem.OrderBy(i => i));
            writer.WriteValue(listAsJsonString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            // Aleternatively read from JTokent
            //JToken token = JToken.ReadFrom(reader);
            //var value = ((JValue)token).Value.ToString();
            var listItem = JsonConvert.DeserializeObject(value, objectType);
            return listItem;
        }
    }
}
