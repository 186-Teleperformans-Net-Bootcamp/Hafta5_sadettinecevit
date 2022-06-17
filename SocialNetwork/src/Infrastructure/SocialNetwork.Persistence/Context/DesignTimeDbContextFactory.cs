using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SocialNetwork.Persistence.Context
{
    public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {   
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
        
        public TContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<TContext> builder = new DbContextOptionsBuilder<TContext>();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/SocialNetwork.WebAPI"))
                .AddJsonFile("appsettings.json")
                .Build();

            builder.UseSqlServer(configuration.GetConnectionString("Default"));

            return CreateNewInstance(builder.Options);
        }
    }
}
