using Newtonsoft.Json;
using Xunit;

namespace Codesanook.Examples.DotNet.Json
{
    public class JsonCustomConverterTest
    {
        [Fact]
        public void SerializeObject_UnorderedInteterInput_ReturnCorrectJsonString()
        {
            var data = new FaceCheckData()
            {
                Keys = new[] { 131, 132, 130 },
                Name = "Codesanook"
            };

            var jsonString = JsonConvert.SerializeObject(data);
            Assert.Equal("{\"Key\":\"[130,131,132]\",\"Name\":\"Codesanook\"}", jsonString);
        }

        [Fact]
        public void DeserializeObject_JsonArrayAsStringInput_ReturnCorrectPropertyValue()
        {
            var jsonString = "{\"Key\":\"[130,131,132]\",\"Name\":\"Codesanook\"}";
            var result = JsonConvert.DeserializeObject<FaceCheckData>(jsonString);
            Assert.Equal(new[] { 130, 131, 132 }, result.Keys);
            Assert.Equal("Codesanook", result.Name);
        }
    }
}
