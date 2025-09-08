namespace Assert.Domain.Repositories
{
    public interface IListingLogRepository
    {
        Task RegisterLog(long ListingRentId, string Action, string BrowserInfoInfo, bool? IsMobile, string _IPAddress, string AdditionalData, string ApplicationCode);
        Task RegisterBulkLog(List<long> listingRentIds,
            string action, string browserInfo, bool? isMobile, string ipAddress,
            string additionalData, string applicationCode);
    }
}
