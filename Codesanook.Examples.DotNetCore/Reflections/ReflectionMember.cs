using Codesanook.Examples.CSharp.Models;
using Xunit;

namespace Codesanook.Examples.CSharp.Reflections
{
    public class ReflectionMember
    {
        [Fact]
        public void GetFullName_ValidPropertyValues_ReturnCorrectFullName()
        {
            var user = new User();
            user.FirstName = "Anthony";
            user.LastName = "CodeSanook";
            var fulllName = user.GetFullName();

            Assert.Equal("Anthony CodeSanook", fulllName);
        }

        [Fact]
        public void GetFullNameWithReflection_ValidPropertyValues_ReturnCorrectFullName()
        {
            var user = new User();
            var userType = user.GetType();
            var firstNameProperty = userType.GetProperty("FirstName");
            var lastNameProperty = userType.GetProperty("LastName");

            firstNameProperty.SetValue(user, "Anthony");
            lastNameProperty.SetValue(user, "CodeSanook");

            var getfullNameMethod = userType.GetMethod("GetFullName");
            var fullName = getfullNameMethod.Invoke(user, null);
            Assert.Equal("Anthony CodeSanook", fullName);
        }

    }
}
