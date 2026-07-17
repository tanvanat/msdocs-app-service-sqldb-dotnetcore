using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreSqlDb.Data
{
    public class MyDatabaseContextFactory
        : IDesignTimeDbContextFactory<MyDatabaseContext>
    {
        public MyDatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString =
                configuration.GetConnectionString("MyDbConnection");

            var optionsBuilder =
                new DbContextOptionsBuilder<MyDatabaseContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new MyDatabaseContext(optionsBuilder.Options);
        }
    }
}