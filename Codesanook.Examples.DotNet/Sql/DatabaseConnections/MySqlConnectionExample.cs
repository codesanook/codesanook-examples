using MySql.Data.MySqlClient;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.DotNet.Sql.DatabaseConnections
{
    public class MySqlConnectionExample
    {
        private readonly ITestOutputHelper output;
        public MySqlConnectionExample(ITestOutputHelper output) => this.output = output;

        /*
        Install-Package MySql.Data -Version 8.0.17

        CREATE DATABASE codesanook;

        CREATE TABLE users (
            id INT NOT NULL AUTO_INCREMENT,
            first_name VARCHAR(50) NOT null,
            last_name VARCHAR(50) NOT NULL,
            date_of_birth DATETIME NOT NULL,
            PRIMARY KEY (`id`)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8;

        INSERT INTO users VALUES (NULL, 'Jose', 'Realman', '2018-01-01');
        */

        [Fact]
        public void Connect()
        {
            // Password is simple, just for demo, please use strong password when using in a production environment
            const string connectionString = "Server=localhost;Database=codesanook;Uid=root;Pwd=1234;";
            using var connection = new MySqlConnection(connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE id = @userId";//Same as SQL Server now

            const int userIdParameterValue = 1;
            var parameter = new MySqlParameter()
            {
                ParameterName = "userId",
                Value = userIdParameterValue,
                MySqlDbType = MySqlDbType.Int32
            };
            command.Parameters.Add(parameter);

            connection.Open();
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
