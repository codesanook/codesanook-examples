using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Codesanook.Examples.DotNet.Json
{

    public class ErrorResponse
    {
        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class SuccessResponse
    {
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }
    }

    public class TryParseJsonTest
    {
        private static T TryParse<T>(string jsonData) where T : new()
        {
            var result = JsonConvert.DeserializeObject<dynamic>(jsonData);
            var list = result as IEnumerable<dynamic>;
            if (list != null)
            {
                var anyErrors = list.Any(e => ((JValue)e.key).Value.ToString().StartsWith("errors"));
                if (anyErrors)
                {

                }
            }

            return default(T);
        }

        [Fact]
        public void Deserialize_ErrorJsonInput_NotThrowExceptionAndReturnErrorArray()
        {
            const string jsonData =
                "[{\"attribute\":\"year\",\"key\":\"errors.invalid\",\"message\":\"Year is invalid\",\"status\":422}]";

            var result = JsonConvert.DeserializeObject<SuccessResponse>(jsonData, new JsonSerializerSettings
            {
                Error = (_, args) => args.ErrorContext.Handled = true
            });

            if (result == null)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse[]>(jsonData);
            }
        }
    }
}
