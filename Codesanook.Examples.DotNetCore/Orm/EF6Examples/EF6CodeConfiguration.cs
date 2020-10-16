using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace Codesanook.Examples.DotNetCore.Orm.EF6Examples
{
    public class EF6CodeConfiguration : DbConfiguration
    {
        public EF6CodeConfiguration()
        {
            // The default connection factory is used when pass context string to db context.
            SetDefaultConnectionFactory(new SQLiteConnectionFactory());
            // https://stackoverflow.com/a/23237737/1872200
            // https://www.entityframeworktutorial.net/entityframework6/code-based-configuration.aspx
            // https://docs.microsoft.com/en-us/ef/ef6/fundamentals/configuring/config-file?redirectedfrom=MSDN#ef-database-providers-ef6-onwards
            // Register ADO.NET provider
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            // Register an Entity Framework provider
            var providedService = (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices));
            SetProviderServices("System.Data.SQLite", providedService);
        }
    }

    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var connection = new SQLiteConnection(nameOrConnectionString);
            // Open connection immediately to get SQLite in memory work
            connection.Open();
            return connection;
        }
    }
}
