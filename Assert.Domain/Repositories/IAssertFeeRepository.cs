using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IAssertFeeRepository
    {
        Task<TAssertFee> GetAssertFee(long listingRentId);
        Task<string> UpsertAssertFeeByCountry(int countryId, decimal? feePercent, decimal? feeBase);
    }
}
