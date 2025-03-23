using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IListingLogRepository _listingLogrepository;
        public PropertyRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository listingLogrepository)
        {
            _context = infraAssertDbContext;
            _listingLogrepository = listingLogrepository;
        }
        public async Task<TpProperty> GetFromListingId(long listingRentId)
        {
            var result = await _context.TpProperties.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
            return result;
        }

        public Task SetLocation(long propertyId, double? latitude, double? longitude)
        {
            throw new NotImplementedException();
        }

        public TpProperty SetPropertySubType(long propertyId, int? subtypeId)
        {
            throw new NotImplementedException();
        }

        public async Task Update(long propertyId, TpProperty tp_property)
        {
            TpProperty property = await _context.TpProperties.Where(x => x.PropertyId == propertyId).FirstOrDefaultAsync();

            property.ExternalReference = tp_property.ExternalReference;
            property.PropertySubtypeId = tp_property.PropertySubtypeId;
            _context.SaveChanges();
        }
    }
}
