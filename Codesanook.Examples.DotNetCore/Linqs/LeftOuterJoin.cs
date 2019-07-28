using Codesanook.Examples.CSharp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Codesanook.Examples.CSharp.Linqs
{
    public class LeftOuterJoin
    {
        [Fact]
        public void Test()
        {
            // Specify the first data source.
            var categories = new List<Category>() {
                    new Category(){ Name="Beverages", Id=001 },
                    new Category(){ Name="Condiments", Id=002 },
                    new Category(){ Name="Vegetables", Id=003 },
                    new Category(){ Name="Grains", Id=004 },
                    new Category(){ Name="Fruit", Id=005 }
                };

            // Specify the second data source.
            var products = new List<Product>()
            {
                new Product{ Name="Cola", CategoryId=001 },
                new Product{ Name="Tea", CategoryId=001 },
                new Product{ Name="Mustard", CategoryId=002 },
                new Product{ Name="Pickles", CategoryId=002 },
                new Product{ Name="Carrots", CategoryId=003 },
                new Product{ Name="Bok Choy", CategoryId=003 },
                new Product{ Name="Peaches", CategoryId=005 },
                new Product{ Name="Melons", CategoryId=005 },
            };

            var innerGroupJoinQuery =
                from category in categories
                join prod in products on category.Id equals prod.CategoryId into prodGroup
                from item in prodGroup.DefaultIfEmpty()
                select new
                {
                    CategoryName = category.Name,
                    ProdName = item?.Name
                };

            foreach (var item in innerGroupJoinQuery)
            {
                Debug.WriteLine($"category name: {item.CategoryName},  product name: {item.ProdName}");
            }
        /*
        output
        category name: Beverages,  product name: Cola
        category name: Beverages,  product name: Tea
        category name: Condiments,  product name: Mustard
        category name: Condiments,  product name: Pickles
        category name: Vegetables,  product name: Carrots
        category name: Vegetables,  product name: Bok Choy
        category name: Grains,  product name: 
        category name: Fruit,  product name: Peaches
        category name: Fruit,  product name: Melons
        */
        // group join https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause#group-join
        // left outer join https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause#left-outer-join
        }
    }
}
