using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IListingDiscountRepository
    {
        Task SetDiscounts(long listingRentId, IEnumerable<int>? enumerable);
    }
}
