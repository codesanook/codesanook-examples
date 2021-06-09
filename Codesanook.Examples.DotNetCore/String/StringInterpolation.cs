using System;
using Codesanook.Examples.DotNetCore.Models;
using Xunit;

namespace Codesanook.Examples.DotNetCore.StringInterpolation
{
    public class StringInterpolation
    {
        [Fact]
        public void GetFullNameWithReflection_ValidPropertyValues_ReturnCorrectFullName()
        {
            // Arrange
            var product = new Product()
            {
                Name = "Basic Programming",
                Price = 499.99M,
                CreatedDate = new DateTime(2018, 3, 5)//in .NET month value starts from 1 
            };

            // Act
            var formattedString = 
                $"product {product.Name} costs {product.Price:0.00} is added on {product.CreatedDate:yyyy-MMM-dd}";

            // Assert
            Assert.Equal("product Basic Programming costs 499.99 is added on 2018-Mar-05", formattedString);
        }
    }
}
