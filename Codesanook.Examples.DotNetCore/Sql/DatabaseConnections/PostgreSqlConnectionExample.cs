using Npgsql;
using NpgsqlTypes;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.Sql.DatabaseConnections
{
    // EF Core database provider for PostgreSQL uses Npgsql 
    // https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli#current-providers
    public class PostgreSqlConnectionExample
    {
        private readonly ITestOutputHelper output;
        public PostgreSqlConnectionExample(ITestOutputHelper output) => this.output = output;

        /*
        Install-Package Npgsql

        docker exec -it codesanook-examples-postgres psql -h localhost -U postgres -W -d codesanook

        CREATE DATABASE codesanook;

        CREATE TABLE users (
            id SERIAL PRIMARY KEY, -- is equivalent to id integer NOT NULL DEFAULT nextval('table_name_id_seq')
            first_name VARCHAR(50) NOT NULL,
            last_name VARCHAR(50) NOT NULL,
            date_of_birth DATE NOT NULL -- date (no time of day)	
        );

        INSERT INTO users VALUES (DEFAULT, 'Jose', 'Realman', '2018-01-01');
        */

        [Fact]
        public async Task Connect_ValidConfiguration_ReturnUserId1()
        {
            // Password is simple, just for a demo, please use strong password when using on a production environment
            // https://www.npgsql.org/doc/connection-string-parameters.html
            const string connectionString = "Host=localhost;Port=5432;Database=codesanook;Username=postgres;Password=12345Abc%";
            using var connection = new NpgsqlConnection(connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE id = @userId"; // Same as SQL Server now

            const int userIdParameterValue = 1;
            var parameter = new NpgsqlParameter()
            {
                ParameterName = "userId",
                Value = userIdParameterValue,
                NpgsqlDbType = NpgsqlDbType.Integer
            };
            command.Parameters.Add(parameter);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var userId = reader.GetInt32(reader.GetOrdinal("id"));
                var userFirstName = reader.GetString(reader.GetOrdinal("first_name"));
                output.WriteLine("\t{0}\t{1}", userId, userFirstName);
                Assert.Equal(1, userId);
            }
        }
    }
}
