using Codesanook.Examples.CSharp.Models;
using Xunit;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Codesanook.Examples.CSharp.Reflection
{
    public class SerializableTest
    {
        [Fact]
        public void GetFullName_ValidPropertyValues_ReturnCorrectFullName()
        {
            var user = new User(1, "Anthony", "CodeSanook");
            var order = new Order() { User = user };

            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, order);
            var serializedObject = memoryStream.ToArray();
            memoryStream.Close();

            memoryStream = new MemoryStream(serializedObject);
            order = (Order)binaryFormatter.Deserialize(memoryStream);
            memoryStream.Close();

            Assert.Equal("Anthony", order.User.FirstName);
        }
    }
}
