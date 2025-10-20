using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class AssertFeeRepository(
        InfraAssertDbContext _context,
        IExceptionLoggerService _exceptionLoggerService,
        IListingRentRepository _listingRentRepository) 
        : IAssertFeeRepository
    {
        public async Task<TAssertFee> GetAssertFee(long listingRentId)
        {
            var listing = await _listingRentRepository.Get(listingRentId, 0, true);
            if (listing == null)
            {
                throw new NotFoundException("Listing not found");
            }
            int? cityid = listing.TpProperties.FirstOrDefault()?.CityId;
            int? countyId = listing.TpProperties.FirstOrDefault()?.CountyId;
            int? stateId = listing.TpProperties.FirstOrDefault()?.StateId;
            int? countryId = listing.TpProperties.FirstOrDefault()?.CountryId;
            TAssertFee result = null;
            if (cityid.HasValue)
            {
                result = await _context.TAssertFees
                    .Where(x => x.CityId == cityid.Value).FirstOrDefaultAsync();
            }
            if (result == null && countyId.HasValue)
            {
                result = await _context.TAssertFees
                    .Where(x => x.CountyId == countyId.Value).FirstOrDefaultAsync();
            }
            if (result == null && stateId.HasValue)
            {
                result = await _context.TAssertFees
                    .Where(x => x.StateId == stateId.Value).FirstOrDefaultAsync();
            }
            if (result == null && countryId.HasValue)
            {
                result = await _context.TAssertFees
                    .Where(x => x.CountryId == countryId.Value).FirstOrDefaultAsync();
            }
            if(result == null)
            {
                result = await _context.TAssertFees
                    .Where(x => x.CityId == null && x.CountyId == null && x.StateId == null && x.CountryId == null).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<string> UpsertAssertFeeByCountry(int countryId, decimal? feePercent, decimal? feeBase)
        {
            try
            {
                var existing = await _context.TAssertFees
                    .Where(x => x.CountryId == countryId && x.IsEnabled == true)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    // Update fields
                    existing.FeePercent = feePercent;
                    existing.FeeBase = feeBase;

                    _context.TAssertFees.Update(existing);
                    await _context.SaveChangesAsync();
                    return "SAVED";
                }

                // Create new country-level fee
                var toCreate = new TAssertFee
                {
                    CountryId = countryId,
                    FeePercent = feePercent,
                    FeeBase = feeBase,
                    IsEnabled = true
                };

                await _context.TAssertFees.AddAsync(toCreate);
                await _context.SaveChangesAsync();
                return "SAVED";
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { countryId, feePercent, feeBase });
                throw new InfrastructureException(ex.Message);
            }
        }
    }
}
