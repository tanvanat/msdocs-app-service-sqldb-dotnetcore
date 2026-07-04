using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetCoreSqlDb.Data
{
    public class MyDatabaseContextFactory
        : IDesignTimeDbContextFactory<MyDatabaseContext>
    {
        public MyDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<MyDatabaseContext>();

            var connectionString =
                Environment.GetEnvironmentVariable(
                    "SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING");

            optionsBuilder.UseSqlServer(connectionString);

            return new MyDatabaseContext(optionsBuilder.Options);
        }
    }
}