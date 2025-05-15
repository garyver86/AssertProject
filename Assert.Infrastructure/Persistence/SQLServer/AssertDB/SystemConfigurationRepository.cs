using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly InfraAssertDbContext _context;
        public SystemConfigurationRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository listingLogrepository)
        {
            _context = infraAssertDbContext;
        }
        public async Task<string> GetListingResourcePath()
        {
            string resourcePath = string.Empty;
            resourcePath = (await _context.TSystemConfigurations.Where(x => x.Name == "vgg_resource_listing").FirstOrDefaultAsync())?.Value;
            return resourcePath;
        }
        public async Task<string> GetListingResourceUrl()
        {
            string resourcePath = string.Empty;
            resourcePath = (await _context.TSystemConfigurations.Where(x => x.Name == "vgg_resource_url_listing").FirstOrDefaultAsync())?.Value;
            return resourcePath;
        }
        public async Task<string> GetIconsResourceUrl()
        {
            string resourcePath = string.Empty;
            resourcePath = (await _context.TSystemConfigurations.Where(x => x.Name == "vgg_resource_url_icons").FirstOrDefaultAsync())?.Value;
            return resourcePath;
        }
    }
}
