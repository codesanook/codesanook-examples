using System;
using System.Collections.Generic;
using System.Linq;
using Codesanook.Examples.DotNet.Models;
using Xunit;

namespace Codesanook.Examples.DotNet.Linq
{
    public class ReusableExpressionTest
    {
        [Fact]
        public void WhereFilter_PassReusableFilterMethod_ReturnCorrectOneUser()
        {
            var userList = new List<User>()
            {
                new User(){ FirstName = "User A", CreatedDateUtc = DateTime.UtcNow.AddDays(-20) },
                new User(){ FirstName = "User B", CreatedDateUtc = DateTime.UtcNow.AddDays(-20) }
            };

            var result = userList.Where(u =>
                FilterByCreatedDateUtcAndIsDeleted(u, DateTime.UtcNow.AddDays(-10)) && u.FirstName == "User B"
            );

            Assert.Single(result);
        }

        [Fact]
        public void WhereFilter_PassReusableFilterMethod_ReturnCorrectOneProduct()
        {
            var productList = new List<Product>()
            {
                new Product(){ Name = "Product A", CreatedDateUtc = DateTime.UtcNow.AddDays(-20) , IsDeleted = true},
                new Product(){ Name = "Product B", CreatedDateUtc = DateTime.UtcNow.AddDays(-20) }
            };
            var result = productList.Where(p =>
                FilterByCreatedDateUtcAndIsDeleted(p, DateTime.UtcNow.AddDays(-10), true) && p.Name == "Product A"
            );

            Assert.Single(result);
        }

        private static bool FilterByCreatedDateUtcAndIsDeleted<T>(T entity, DateTime createdBefore, bool isDelete = false) where T : Entity =>
            entity.CreatedDateUtc < createdBefore && entity.IsDeleted == isDelete;
    }
}
