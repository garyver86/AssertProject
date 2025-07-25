﻿using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertyRepository : IPropertyRepository
    {
        private InfraAssertDbContext _context;
        private readonly IListingLogRepository _listingLogrepository;
        public PropertyRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository listingLogrepository)
        {
            _context = infraAssertDbContext;
            _listingLogrepository = listingLogrepository;
        }
        public async Task<TpProperty> GetFromListingId(long listingRentId)
        {
            var result = await _context.TpProperties.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();

            if (result is null)
                throw new NotFoundException($"No se encontro la propiedad para el ListingRent con ID: {listingRentId}");

            return result;
        }

        public async Task<TpProperty> Register(long listingRentId)
        {
            var property = await _context.TpProperties.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
            if (property == null)
            {
                property = new TpProperty
                {
                    ListingRentId = listingRentId
                };
                _context.TpProperties.Add(property);
                await _context.SaveChangesAsync();
            }
            return property;
        }

        public async Task SetLocation(long propertyId, double? latitude, double? longitude)
        {
            TpProperty property = await _context.TpProperties.Where(x => x.PropertyId == propertyId).FirstOrDefaultAsync();

            property.Longitude = longitude;
            property.Latitude = latitude;
            await _context.SaveChangesAsync();
        }

        public async Task<TpProperty> SetPropertySubType(long propertyId, int? subtypeId)
        {
            TpProperty property = await _context.TpProperties.Where(x => x.PropertyId == propertyId).FirstOrDefaultAsync();

            property.PropertySubtypeId = subtypeId;
            await _context.SaveChangesAsync();
            return property;
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
