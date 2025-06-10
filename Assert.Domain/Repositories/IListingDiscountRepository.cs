namespace Assert.Domain.Repositories
{
    public interface IListingDiscountRepository
    {
        Task SetDiscounts(long listingRentId, List<(int, decimal)> discountList);
    }
}
