using CodeSanook.Examples.CSharp.Reflections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace CodeSanook.Examples.CSharp.Tests.Reflections
{
    public class UserTests
    {
        [Fact]
        public void GetFullName_ValidPropertyValues_ReturnCorrectFullName()
        {
            var user = new User();
            user.FirstName = "Ariel";
            user.LastName = "Schmidt";
            var fulllName = user.GetFullName();

            Assert.Equal("Ariel Schmidt", fulllName);

            var userType = user.GetType();
            var firstNameProperty = userType.GetProperty("FirstName");
            var lastNameProperty = userType.GetProperty("LastName");

            firstNameProperty.SetValue(user, "Emma");
            lastNameProperty.SetValue(user, "Jacobson");

            var getfullNameMethod = userType.GetMethod("GetFullName");
            var fullNameValue = getfullNameMethod.Invoke(user, null);
        }
    }
}
