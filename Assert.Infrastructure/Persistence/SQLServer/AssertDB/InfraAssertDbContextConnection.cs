using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public partial class InfraAssertDbContext
    {
        private readonly IConfiguration _configuration;

        //public InfraAssertDbContext(DbContextOptions<InfraAssertDbContext> options, IConfiguration configuration)
        //    : base(options)
        //{
        //    _configuration = configuration;
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration?.GetConnectionString("AssertDB");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("La cadena de conexión 'AssertDB' no se encuentra en appsettings.json.");
                }

                optionsBuilder.UseSqlServer(connectionString,
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    ));
            }
        }
    }
}
