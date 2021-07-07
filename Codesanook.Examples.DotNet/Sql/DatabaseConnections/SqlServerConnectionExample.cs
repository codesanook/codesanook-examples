using System.Data;
using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.DotNetCore.Sql.DatabaseConnections
{
    public class SqlServerConnectionExample
    {
        private readonly ITestOutputHelper output;
        public SqlServerConnectionExample(ITestOutputHelper output) => this.output = output;

        /*
        Install-Package System.Data.SqlClient -Version 4.6.1

        CREATE DATABASE codesanook;

        CREATE TABLE Users (
            Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
            FirstName VARCHAR(50) NOT null,
            LastName VARCHAR(50) NOT NULL,
            DateOfBirth DATETIME NOT NULL
        );

        INSERT INTO Users VALUES ('Jose', 'Realman', '2018-01-01');
        */
        [Fact]
        public void Connect()
        {
            // User Windows Authentication log in as a recommended log in
            const string connectionString = "Server=localhost;Database=codesanook;Trusted_Connection=True;";
            using var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE Id = @userId";

            const int userIdParameterValue = 1;
            var parameter = new SqlParameter()
            {
                ParameterName = "userId",
                Value = userIdParameterValue,
                DbType = DbType.Int32
            };
            command.Parameters.Add(parameter);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var userId = reader.GetInt32(reader.GetOrdinal("Id"));
                var userFirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                output.WriteLine("\t{0}\t{1}", userId, userFirstName);
                Assert.Equal(1, userId);
            }
        }
    }
}
