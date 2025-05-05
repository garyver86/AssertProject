namespace Assert.Domain.Repositories
{
    public interface IListingDiscountRepository
    {
        Task SetDiscounts(long listingRentId, IEnumerable<int>? enumerable);
    }
}
