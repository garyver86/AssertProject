namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    //public partial class InfraAssertDbContext
    //{
    //    private static IConfiguration? _configuration;
    //    public InfraAssertDbContext(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            var connectionString = _configuration?.GetConnectionString("AssertDB");
    //            if (string.IsNullOrEmpty(connectionString))
    //            {
    //                throw new InvalidOperationException("La cadena de conexión 'WebsiteDB' no se encuentra en appsettings.json.");
    //            }

    //            optionsBuilder.UseSqlServer(connectionString,
    //                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
    //                    maxRetryCount: 3,
    //                    maxRetryDelay: TimeSpan.FromSeconds(10),
    //                    errorNumbersToAdd: null
    //                ));
    //        }
    //    }
    //}
}
