using Codesanook.Examples.DotNetCore.Models;
using Xunit;

namespace Codesanook.Examples.DotNetCore.Reflection
{
    public class ReflectionMember
    {
        [Fact]
        public void GetFullName_ValidPropertyValues_ReturnCorrectFullName()
        {
            var user = new User(1, "Anthony", "CodeSanook");
            var fulllName = user.GetFullName();

            Assert.Equal("Anthony CodeSanook", fulllName);
        }

        [Fact]
        public void GetFullNameWithReflection_ValidPropertyValues_ReturnCorrectFullName()
        {
            var userType = typeof(User);
            var firstNameProperty = userType.GetProperty("FirstName");
            var lastNameProperty = userType.GetProperty("LastName");

            var user = new User(0, string.Empty, string.Empty);
            firstNameProperty.SetValue(user, "Anthony");
            lastNameProperty.SetValue(user, "CodeSanook");

            var getfullNameMethod = userType.GetMethod("GetFullName");
            var fullName = getfullNameMethod.Invoke(user, null);
            Assert.Equal("Anthony CodeSanook", fullName);
        }
    }
}
