using Codesanook.Examples.CSharp.Models;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace Codesanook.Examples.CSharp.Linqs
{
    public class ExpressionTree
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
