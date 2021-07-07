using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xunit;

namespace Codesanook.Examples.DotNetCore.Orm.DapperExamples
{
    public class DapperQueryTest
    {
        [Fact]
        public void QueryOfficeWithStoredProc_SomeOfficesInDatabase_ReturnNotEmptyList()
        {
            // Password is simple, just for demo, please use strong password when using in a production environment
            const string connectionString = "Server=localhost, 1433;Database=Codesanook;User Id=SA;Password=12345Abc%";
            using var connection = new SqlConnection(connectionString);
            var offices = connection.Query<Office>(
                "spGetOfficesWithBranchName", 
                commandType: CommandType.StoredProcedure
            ).ToList();

            Assert.NotEmpty(offices);
        }
    }
}
