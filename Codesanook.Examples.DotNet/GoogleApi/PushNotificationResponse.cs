using Newtonsoft.Json;
using System;

namespace Codesanook.Examples.DotNet.GoogleApi
{
    public class PushNotificationResponse
    {
        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => 
            writer.WriteValue(((bool)value) ? 1 : 0);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => 
            reader.Value.ToString() == "1";

        public override bool CanConvert(Type objectType) => 
            objectType == typeof(bool); // Handle only boolean
    }
}

