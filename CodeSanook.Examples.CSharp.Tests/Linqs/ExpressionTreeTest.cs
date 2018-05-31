using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace CodeSanook.Examples.CSharp.Tests.Linqs
{
    public class Profile
    {
        public int Id { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Profile Profile { get; set; }
    }


    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }

    public class ExpressionTreeTest
    {

        [Fact]
        public void GetPropertyName_OneLevelNestedProperty_ReturnCorrectName()
        {
            var referenceKey = GetPropertyName<Order, int>(model => model.User.Id);
            Assert.Equal("User.Id", referenceKey);
        }

        [Fact]
        public void GetPropertyName_TwoLevelNestedProperty_ReturnCorrectName()
        {
            var referenceKey = GetPropertyName<Order, int>(model => model.User.Profile.Id);
            Assert.Equal("User.Profile.Id", referenceKey);
        }


        [Fact]
        public void GetPropertyName_DirectProperty_ReturnCorrectName()
        {
            var referenceKey = GetPropertyName<Order, int>(model => model.Id);
            Assert.Equal("Id", referenceKey);
        }


        public static string GetPropertyName<P, T>(Expression<Func<P, T>> expression)
        {
            string path = "";
            var memberExpression = (MemberExpression)expression.Body;
            var memberExpressionOrg = memberExpression;
            while (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var propInfo = memberExpression.Expression.GetType().GetProperty("Member");
                var propValue = propInfo.GetValue(memberExpression.Expression, null) as PropertyInfo;
                path = propValue.Name + "." + path;
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return path + memberExpressionOrg.Member.Name;

        }
    }

}
