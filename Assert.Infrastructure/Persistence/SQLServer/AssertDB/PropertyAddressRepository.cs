using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;

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
            TpProperty address = null;
            if (propertyId > 0)
            {
                address = await _context.TpProperties.Where(x => x.PropertyId == propertyId).FirstOrDefaultAsync();
            }
            else
            {
                address = new TpProperty
                {
                    Address1 = addresInput.Address1,
                    CityId = addresInput.CityId,
                    Address2 = addresInput.Address2,
                    ZipCode = addresInput.ZipCode,
                    CountyId = addresInput.CountyId,
                    StateId = addresInput.StateId,
                    ListingRentId = (-1) * propertyId
                };
                _context.TpProperties.Add(address);
            }
            if (address != null)
            {
                address.Address1 = addresInput.Address1;
                address.CityId = addresInput.CityId;
                address.Address2 = addresInput.Address2;
                address.ZipCode = addresInput.ZipCode;
                address.CountyId = addresInput.CountyId;
                address.StateId = addresInput.StateId;
                //await _context.SaveChangesAsync();

                if (addresInput.CityId > 0)
                {
                    var city = await _context.TCities.Where(x => x.CityId == addresInput.CityId)
                        .Include(c => c.County)
                            .ThenInclude(co => co.State)
                                .ThenInclude(s => s.Country).FirstOrDefaultAsync();
                    if (city != null)
                    {
                        address.CityName = city.Name;
                        address.CountyId = city.CountyId;
                        address.CountyName = city.County?.Name;
                        address.StateId = city.County?.StateId;
                        address.StateName = city.County?.State?.Name;
                        address.CountryId = city.County?.State?.CountryId;
                        address.CountryName = city.County?.State?.Country?.Name;

                        addresInput.CountyId = city.CountyId;
                        addresInput.StateId = city.County?.StateId;
                        addresInput.CityId = city.CityId;
                    }
                }
                else if (addresInput.CountyId > 0)
                {
                    var county = await _context.TCounties.Where(x => x.CountyId == addresInput.CountyId)
                        .Include(co => co.State)
                                .ThenInclude(s => s.Country).FirstOrDefaultAsync();
                    if (county != null)
                    {
                        address.CountyName = county.Name;
                        address.StateId = county.StateId;
                        address.StateName = county.State?.Name;
                        address.CountryId = county.State?.CountryId;
                        address.CountryName = county.State?.Country?.Name;

                        addresInput.CountyId = county.CountyId;
                        addresInput.StateId = county.StateId;
                    }
                }
                else if (addresInput.StateId > 0)
                {
                    var state = await _context.TStates.Where(x => x.StateId == addresInput.StateId)
                        .Include(s => s.Country).FirstOrDefaultAsync();
                    if (state != null)
                    {
                        address.StateName = state.Name;
                        address.CountryId = state.CountryId;
                        address.CountryName = state.Country?.Name;
                        addresInput.StateId = state.StateId;
                    }
                }
                await _context.SaveChangesAsync();
                return addresInput;
            }
            throw new Exception("La propiedad no fue encontrada o no puede ser modificada.");
        }
    }
}
