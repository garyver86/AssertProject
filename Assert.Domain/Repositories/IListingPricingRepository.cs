namespace Assert.Domain.Repositories
{
    public interface IListingPricingRepository
    {
        Task SetPricing(long listingRentId, decimal? pricing, int? currencyId);
        Task SetPricing(long listingRentId, decimal? pricing, decimal? weekendPrice, int? currencyId);
    }
}
