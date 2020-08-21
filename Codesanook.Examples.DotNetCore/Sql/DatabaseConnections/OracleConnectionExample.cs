using Oracle.ManagedDataAccess.Client;
using Xunit;
using Xunit.Abstractions;

namespace Codesanook.Examples.Sql.DatabaseConnections
{
    public class OracleConnectionExample
    {
        //https://xunit.net/docs/capturing-output
        //For capturing an output to Visual Studio Test Explorer
        private readonly ITestOutputHelper output;
        public OracleConnectionExample(ITestOutputHelper output) => this.output = output;

        //Install-Package Oracle.ManagedDataAccess.Core
        /*
        -- In the example, we are using xe database which already created for us. 
        --
        CREATE TABLE USERS (
            ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            FIRST_NAME NVARCHAR2(50) NOT NULL,
            LAST_NAME NVARCHAR2(50) NOT NULL,
            DATE_OF_BIRTH DATE NOT NULL
        );

        INSERT INTO users (first_name, last_name, date_of_birth) 
        VALUES ('Jose', 'Realman', TO_DATE('2018/01/01 00:00:00', 'yyyy/mm/dd hh24:mi:ss'));
        */

        [Fact]
        public void Connect()
        {
            const string connectionString = "Data Source=localhost:1521/xe;User Id=system;Password=1234;";
            using var connection = new OracleConnection(connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM USERS WHERE ID = :userId";
            //https://stackoverflow.com/a/49411028/1872200, default is false and will bind a parameter by order
            command.BindByName = true;

            const int userIdParameterValue = 1;
            var parameter = new OracleParameter()
            {
                ParameterName = "userId",
                Value = userIdParameterValue,
                OracleDbType = OracleDbType.Int32
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
