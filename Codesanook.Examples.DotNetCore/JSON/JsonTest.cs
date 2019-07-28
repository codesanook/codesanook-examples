using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Codesanook.Examples.CSharp.JSON
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
    }
}
