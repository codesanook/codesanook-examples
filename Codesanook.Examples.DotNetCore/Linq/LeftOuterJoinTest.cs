using Codesanook.Examples.CSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Codesanook.Examples.CSharp.Linq
{
    public class LeftOuterJoinTest
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
                Debug.WriteLine($"category name: {item.CategoryName}, product name: {item.ProdName}");
            }
            /*
            output
            category name: Beverages, product name: Cola
            category name: Beverages, product name: Tea
            category name: Condiments, product name: Mustard
            category name: Condiments, product name: Pickles
            category name: Vegetables, product name: Carrots
            category name: Vegetables, product name: Bok Choy
            category name: Grains, product name: 
            category name: Fruit, product name: Peaches
            category name: Fruit, product name: Melons
            */
            // group join https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause#group-join
            // left outer join https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause#left-outer-join
        }

        [Fact]
        public void LeftOuterJoinWithWhereClause_ValidInput_ReturnCorrectLeftOuterJoinResult()
        {
            var tableASet = new[]
            {
                new TableA() { CreateAtTime = CreateDateFromString("2020-06-05 12:02:25") },
                new TableA() { CreateAtTime = CreateDateFromString("2020-06-05 10:00:11") },
                new TableA() { CreateAtTime = CreateDateFromString("2020-06-05 18:00:00") },
            };

            var tableBSet = new[]
            {
                new TableB()
                {
                    StartedAtTime = CreateDateFromString("2020-06-05 10:00:00"),
                    EndedAtTime = CreateDateFromString("2020-06-05 12:00:00")
                },
                new TableB() 
                {
                    StartedAtTime = CreateDateFromString("2020-06-05 12:01:00"),
                    EndedAtTime = CreateDateFromString("2020-06-05 14:00:00")
                }
            };

            static DateTime CreateDateFromString(string dateString)
               => DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            var createdInTimeRanges = (
                from a in tableASet
                from b in tableBSet.Where(
                    bb => a.CreateAtTime >= bb.StartedAtTime && a.CreateAtTime <= bb.EndedAtTime
                ).DefaultIfEmpty()
                select new
                {
                    a.CreateAtTime,
                    b?.StartedAtTime,
                    b?.EndedAtTime
                }
            ).ToArray();

            Assert.Equal(tableASet.Length, createdInTimeRanges.Length);
            Assert.All(createdInTimeRanges, x =>
            {
                Assert.True(
                    (x.StartedAtTime == null && x.EndedAtTime == null) ||
                    (x.CreateAtTime >= x.StartedAtTime && x.CreateAtTime <= x.EndedAtTime)
                );
            });
        }
    }
}

class TableA
{
    public DateTime CreateAtTime { get; set; }
}

class TableB
{
    public DateTime StartedAtTime { get; set; }
    public DateTime EndedAtTime { get; set; }
}

