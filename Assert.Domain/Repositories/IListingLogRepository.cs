namespace Assert.Domain.Repositories
{
    public interface IListingLogRepository
    {
        Task RegisterLog(long ListingRentId, string Action, string BrowserInfoInfo, bool? IsMobile, string _IPAddress, string AdditionalData, string ApplicationCode);
    }
}
