using Xunit;

namespace Codesanook.Examples.DotNet.LanguageFeatures
{
    public class Person
    {
        public Address Address { get; } = new();
    }

    public class Address
    {
        public string Province { get; set; }
    }

    public class NestedObjectInitializerTest
    {
        [Fact]
        public void NestedPropertyInitialize_PropertyHasInitializedValue_ObjectCreatedAndIsNotNull()
        {
            var person = new Person
            {
                Address = { Province = "Bangkok" }
            };

            Assert.NotNull(person);
        }
    }
}

