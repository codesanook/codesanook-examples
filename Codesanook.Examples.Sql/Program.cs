using System;
using Oracle.ManagedDataAccess.Client;

namespace Codesanook.Examples.Sql
{
    public class Program
    {
        //Install-Package Oracle.ManagedDataAccess.Core
        /*
        CREATE TABLE USERS (
            ID NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            NAME NVARCHAR2(50) NOT NULL,
            DATE_OF_BIRTH DATE NOT NULL
        );
        */ 

        public static void Main(string[] args)
        {
            string connectionString = "Data Source=localhost:1521;User Id=system;Password=1234;";
            using (var connection = new OracleConnection(connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM USERS WHERE ID = :userId";

                //https://stackoverflow.com/a/49411028/1872200
                command.BindByName = true;

                const int userId = 1;
                var parameter = new OracleParameter()
                {
                    ParameterName = "userId",//:userId work as well
                    Value = userId,
                    OracleDbType = OracleDbType.Int32
                };
                command.Parameters.Add(parameter);

                try
                {
                    connection.Open();
                    OracleDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}", reader["id"], reader["name"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
