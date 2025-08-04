using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingAdditionalFeeRepository(
        InfraAssertDbContext _context,
        IExceptionLoggerService _exceptionLoggerService) : IListingAdditionalFeeRepository
    {
        public async Task<List<TlListingAdditionalFee>> GetByListingRentId(long listingRentId, int userID)
        {
            List<TlListingAdditionalFee> result = new List<TlListingAdditionalFee>();
            result.AddRange(await _context.TlListingAdditionalFees.Where(x => x.ListingRentId == listingRentId)
                .Include(x => x.AdditionalFee).ToListAsync());

            var general_additional_fees = await _context.TlGeneralAdditionalFees.Where(x => x.UserId == userID).Include(x => x.AdditionalFee)
                .Select(x => new TlListingAdditionalFee
                {
                    ListingRentId = -1,
                    AmountFee = x.AmountFee,
                    AdditionalFeeId = x.AdditionalFeeId,
                    IsPercent = x.IsPercent,
                    AdditionalFee = x.AdditionalFee,
                }).ToListAsync();
            if (general_additional_fees != null && general_additional_fees.Count > 0)
            {
                foreach (var fee in general_additional_fees)
                {
                    // Check if the fee already exists in the result
                    if (!result.Any(x => x.AdditionalFeeId == fee.AdditionalFeeId))
                    {
                        result.Add(fee);
                    }
                }
            }

            return result;
        }
    }
}
