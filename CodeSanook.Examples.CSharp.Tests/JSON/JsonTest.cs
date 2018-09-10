using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace CodeSanook.Examples.CSharp.Tests.JSON
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
        public void Test()
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

            Assert.Equal("", json);
        }
    }
}
