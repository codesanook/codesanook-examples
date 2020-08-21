using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.DotNetCore.SQL.DatabaseConnections
{
    public class SqliteConnectionExample
    {
        private readonly ITestOutputHelper output;
        public SqliteConnectionExample(ITestOutputHelper output) => this.output = output;

        /*
            Install-Package Microsoft.Data.Sqlite
        */

        [Fact]
        public void Connect_ValidInput_ReturnCorrectId()
        {
            // Get SQLite version by in memory connection
            string cs = "Data Source=:memory:";
            using var con = new SqliteConnection(cs);
            con.Open();
            using var cmd = new SqliteCommand("SELECT SQLITE_VERSION()", con);
            string version = cmd.ExecuteScalar().ToString();
            output.WriteLine($"SQLite version: {version}");

            // Create a database file if not exist, create table, insert and read data .
            const string connectionString = "data source=databaseFile.db3";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY, -- This column is the alias of the rowid column.
                    first_name TEXT NOT NULL,
                    last_name TEXT NOT NULL,
                    date_of_birth INTEGER NOT NULL
                )";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO users VALUES (NULL, 'Jose', 'Realman', '2018-01-01')";
            command.ExecuteNonQuery();

            command.CommandText = "SELECT * FROM users WHERE id = @userId";//Same as SQL Server now
            const int userIdParameterValue = 1;
            var parameter = new SqliteParameter()
            {
                ParameterName = "userId",
                Value = userIdParameterValue,
                SqliteType = SqliteType.Integer
            };
            command.Parameters.Add(parameter);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var userId = reader.GetInt32(reader.GetOrdinal("id"));
                var userFirstName = reader.GetString(reader.GetOrdinal("first_name"));
                output.WriteLine("\t{0}\t{1}", userId, userFirstName);
                Assert.Equal(1, userId);
            }
        }
    }
}

