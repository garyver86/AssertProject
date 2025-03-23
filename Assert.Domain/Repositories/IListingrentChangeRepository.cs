namespace Assert.Domain.Repositories
{
    public interface IListingrentChangeRepository
    {
        Task Register(long listingRentId, string action, string browseInfo, bool? isMobile, string ipAddress, string additionalData, string applicationCode);
    }
}
