using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertyAddressRepository : IPropertyAddressRepository
    {

        private readonly InfraAssertDbContext _context;
        public PropertyAddressRepository(InfraAssertDbContext context)
        {
            _context = context;
        }
        public async Task<TpPropertyAddress> Set(TpPropertyAddress addresInput, long propertyId)
        {
            var address = await _context.TpPropertyAddresses.Where(x => x.PropertyId == propertyId).FirstOrDefaultAsync();
            if (address != null)
            {
                address.Address1 = addresInput.Address1;
                address.CityId = addresInput.CityId;
                address.Address2 = addresInput.Address2;
                address.ZipCode = addresInput.ZipCode;
                await _context.SaveChangesAsync();
            }
            else
            {
                address = new TpPropertyAddress
                {
                    Address1 = addresInput.Address1,
                    CityId = addresInput.CityId,
                    Address2 = addresInput.Address2,
                    ZipCode = addresInput.ZipCode,
                    PropertyId = propertyId
                };
                await _context.TpPropertyAddresses.AddAsync(address);
                await _context.SaveChangesAsync();
            }
            return address;
        }
    }
}
