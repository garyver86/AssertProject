using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IListingPricingRepository
    {
        Task SetPricing(long listingRentId, decimal? pricing, int? currencyId);
    }
}
