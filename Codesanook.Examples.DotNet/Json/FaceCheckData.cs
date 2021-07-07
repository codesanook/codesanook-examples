using Newtonsoft.Json;
using System.Collections.Generic;

namespace Codesanook.Examples.DotNetCore.Json
{
    public class FaceCheckData
    {
        [JsonConverter(typeof(ArrayAsStringConverter<int>))]
        [JsonProperty("Key")]
        public IList<int> Keys { get; set; }
        public string Name { get; set; }
    }
}
