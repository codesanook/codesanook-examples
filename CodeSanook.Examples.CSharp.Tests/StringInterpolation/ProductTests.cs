using CodeSanook.Examples.CSharp.StringInterpolation;
using System;
using Xunit;

namespace CodeSanook.Examples.CSharp.Tests.StringInterpolation
{
    public class ProductTests
    {
        [Fact]
        public void GetFullNameWithReflection_ValidPropertyValues_ReturnCorrectFullName()
        {
            //arrange
            var product = new Product()
            {
                Name = "Basic Programming",
                Price = 499.99M,
                CreatedDate = new DateTime(2018, 3, 5)//in .NET month value starts from 1 
            };

            //act
            var formattedString = 
                $"product {product.Name} costs {product.Price:0.00} is added on {product.CreatedDate:yyyy-MMM-dd}";

            //assert
            Assert.Equal("product Basic Programming costs 499.99 is added on 2018-Mar-05", formattedString);
        }
    }
}
