using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Codesanook.Examples.DotNetCore.JSON
{
    public class Person
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Student : Person
    {
        [JsonProperty]
        public new virtual int Id
        {
            get { return base.Id; }
            set { base.Id = value; ; }
        }

        [JsonProperty]
        public virtual string School { get; set; }
    }

    public class JsonTest
    {
        [Fact]
        public void Serialize_OptInOption_ReturnOnlyJsonPropertyAttribute()
        {
            var student = new Student()
            {
                Id = 1,
                FirstName = "Anthony",
                LastName = "CodeSanook",
                School = "Ammart"
            };

            var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            var json = JsonConvert.SerializeObject(student, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            Assert.Equal("{\"id\":1,\"school\":\"Ammart\"}", json);
        }

        [Fact]
        public void Serialize_NullObject_ReturnNullString()
        {
            var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };

            Student student = null;
            var person = (Person)student;
            var json = JsonConvert.SerializeObject(person?.Id, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            Assert.Equal("null", json);
        }

        [Fact]
        public void Deserialize_OrderDynamicObject_OrderedCorrectly()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var jsonPath = Path.Combine(directory, "JSON/app-service-runtime.json");

            using var fileStream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);

            //   var runtime = JsonConvert.DeserializeObject<dynamic>(streamReader.ReadToEnd());
            dynamic data = JObject.Parse(streamReader.ReadToEnd());
            var sortedNodeVersion = ((IEnumerable<dynamic>)data.nodejs)
                .Select(n =>
                {
                    var versionString = n.version.Value as string;
                    var version = new Version(versionString);
                    return new { Version = version, NPM = n.npm.Value };
                })
                .OrderByDescending(n => n.Version)
                .ToArray();

            var formattedNode = string.Join("\n", sortedNodeVersion.Select(x => $"Node.js: {x.Version}, npm: {x.NPM}"));
        }

        [Fact]
        public void Test()
        {
            dynamic foo = 1;
            Assert.Equal(2, foo + 1);

            foo = "test";
            Assert.Equal("test1", foo + 1);
        }

    }
}
