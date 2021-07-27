using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Codesanook.Examples.DotNet.Collections
{
    public class Student { public string Name { set; get; } }

    public class DataModel
    {
        public string Token { get; } = "xxxx";
        public string LifeTime { get; } = "yyyy";
    }

    public static class ObjectExtensions
    {
        public static IDictionary<string, string> ToDictionaryWithReflection(this object obj) =>
            obj.GetType().GetProperties().ToDictionary(
                property => property.Name,
                property => property.GetValue(obj).ToString()
            );

        public static IDictionary<string, string> ToDictionary(this object obj) =>
            JObject.FromObject(obj).ToObject<Dictionary<string, string>>();
    }

    public class DictionaryTest
    {
        [Fact]
        public void CreateDictionary_WithManyWayToInitialize_InvalidInitializeThrowException()
        {
            var s1 = new Dictionary<int, Student>
            {
                { 1,  new Student { Name = "a1" } },
                { 2, new Student { Name = "a2"} },
            };

            var s2 = new Dictionary<int, Student>
            {
                [1] = { },
                [2] = { },
            };

            // C# 9 https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/#target-typed-new-expressions
            var s3 = new Dictionary<int, Student>
            {
                [1] = new(),
                [2] = new(),
            };

            var s4 = new Dictionary<int, Student>
            {
                [1] = new(),
                [2] = new(),
                [1] = { Name = "a1" },
                [2] = { Name = "a2" },
            };

            // error
            //var s5 = new Dictionary<int, Student>
            //{
            //    [1] = { Name = "a1" },
            //    [2] = { Name = "a2" },
            //};
        }

        [Fact]
        public void ToDictionaryWithJsonDotNet_ValidObjectInput_ReturnCorrectDictionary()
        {
            // Arrange 
            var data = new DataModel();
            var expectedResult = new Dictionary<string, string>()
            {
                { nameof(data.LifeTime), data.LifeTime },
                { nameof(data.Token), data.Token },
            };

            // Act
            var actualResult = data.ToDictionary();

            // Assert, order of dictionary does not matter
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ToDictionaryWithReflection_ValidObjectInput_ReturnCorrectDictionary()
        {
            // Arrange 
            var data = new DataModel();
            var expectedResult = new Dictionary<string, string>()
            {
                { nameof(data.LifeTime), data.LifeTime },
                { nameof(data.Token), data.Token },
            };

            // Act
            var actualResult = data.ToDictionaryWithReflection();

            // Assert, order of dictionary does not matter
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ToDictionaryWithAnnonymous_ValidObjectInput_ReturnCorrectDictionary()
        {
            // Arrange 
            var data = new
            {
                Token = "xxxxx",
                LifeTime = "200",
            };

            var expectedResult = new Dictionary<string, string>()
            {
                { nameof(data.LifeTime), data.LifeTime },
                { nameof(data.Token), data.Token },
            };

            // Act
            var actualResult = data.ToDictionaryWithReflection();

            // Assert, order of dictionary does not matter
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
