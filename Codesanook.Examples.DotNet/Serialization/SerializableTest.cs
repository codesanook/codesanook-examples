using Xunit;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Codesanook.Examples.DotNet.Models;

namespace Codesanook.Examples.DotNet.JSON
{
    public class SerializableTest
    {
        [Fact]
        public void GetFullName_ValidPropertyValues_ReturnCorrectFullName()
        {
            // var user = new User(1, "Anthony", "Codesanook");
            // var order = new Order() { User = user };

            // var binaryFormatter = new BinaryFormatter();
            // var memoryStream = new MemoryStream();
            // binaryFormatter.Serialize(memoryStream, order);
            // var serializedObject = memoryStream.ToArray();
            // memoryStream.Close();

            // memoryStream = new MemoryStream(serializedObject);
            // order = (Order)binaryFormatter.Deserialize(memoryStream);
            // memoryStream.Close();

            // Assert.Equal("Anthony", order.User.FirstName);
        }
    }
}
