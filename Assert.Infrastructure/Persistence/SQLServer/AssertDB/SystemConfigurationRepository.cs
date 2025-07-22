using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;
        public SystemConfigurationRepository(InfraAssertDbContext infraAssertDbContext,
            IListingLogRepository listingLogrepository,
            IServiceProvider serviceProvider)
        {
            _context = infraAssertDbContext;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }
        public async Task<string> GetListingResourcePath()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                string resourcePath = string.Empty;
                resourcePath = (await dbContext.TSystemConfigurations.Where(x => x.Name == "vgg_resource_listing").FirstOrDefaultAsync())?.Value;
                return resourcePath;
            }
        }
        public async Task<string> GetProfilePhotoResourcePath()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                string resourcePath = string.Empty;
                resourcePath = (await dbContext.TSystemConfigurations
                    .Where(x => x.Name == "vgg_resource_profile_photos")
                    .FirstOrDefaultAsync())?.Value ?? "";
                return resourcePath;
            }
        }
        public async Task<string> GetProfilePhotoResourceUrl()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                string resourcePath = string.Empty;
                resourcePath = (await dbContext.TSystemConfigurations
                    .Where(x => x.Name == "vgg_resource_url_profile_photos")
                    .FirstOrDefaultAsync())?.Value ?? "";
                return resourcePath;
            }
        }
        public async Task<string> GetListingResourceUrl()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                string resourcePath = string.Empty;
                resourcePath = (await dbContext.TSystemConfigurations.Where(x => x.Name == "vgg_resource_url_listing").FirstOrDefaultAsync())?.Value;
                return resourcePath;
            }
        }
        public async Task<string> GetIconsResourceUrl()
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                string resourcePath = string.Empty;
                resourcePath = (await dbContext.TSystemConfigurations.Where(x => x.Name == "vgg_resource_url_icons").FirstOrDefaultAsync())?.Value;
                return resourcePath;
            }
        }
    }
}
