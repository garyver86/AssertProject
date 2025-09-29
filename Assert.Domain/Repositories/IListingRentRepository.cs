using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IListingRentRepository
    {
        Task<TlListingRent> Get(long id, int guestId, bool onlyActive);
        Task<TlListingRent> Get(long id, long ownerID);
        Task<TlListingRent> ChangeStatus(long id, int ownerID, int newStatus, Dictionary<string, string> userInfo);
        Task<string> ChangeStatusByOwnerIdAsync(
            int ownerId, string statusCode, Dictionary<string, string> userInfo);
        Task<List<TlListingRent>> GetAll(int ownerUserId);

        Task<(List<TlListingRent>, PaginationMetadata)> GetPublished(SearchFiltersToListingRent filters, int pageNumber, int pageSize);
        Task<(List<TlListingRent>, PaginationMetadata)> GetSortedByMostRentalsAsync(SearchFiltersToListingRent filters, int pageNumber, int pageSize);
        Task<TlListingRent> Register(TlListingRent listingRent, Dictionary<string, string> clientData);
        Task<bool> HasStepInProcess(long listingRentId);
        Task<TlListingRent> SetAccomodationType(long listingRentId, int? subtypeId, int userId);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, bool? allDoorsLocked, int? maxGuests, int? privateBathroom, int? privateBathroomLodging, int? sharedBathroom, int userId);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, int? bathrooms, int? maxGuests, int? privateBathroom, int? privateBathroomLodging, int? sharedBathroom, int userId);
        Task SetAsConfirmed(long listingRentId);
        Task SetSecurityConfirmationData(long listingRentId, bool? presenceOfWeapons, bool? noiseDesibelesMonitor, bool? externalCameras, int userId);
        Task SetApprovalPolicy(long listingRentId, int? approvalPolicyTypeId, int userId);
        Task SetDescription(long listingRentId, string description, int userId);
        Task SetName(long listingRentId, string title, int userId);
        Task SetNameAndDescription(long listingRentId, string title, string description, int userId);
        Task<(List<TlListingRent>, PaginationMetadata)> GetFeatureds(long userId, int pageNumber = 1, int pageSize = 10, int? countryId = null);
        
        Task<string> UpdateBasicData(long listingRentId, string title, string description, List<int> aspectTypeIdList, int userId);
        Task SetReservationTypeApprobation(long listingRentId, int value1, int value2, int value3, int userId);
        Task SetCheckInPolicies(long listingRentId, string checkinTime, string checkoutTime, string instructions, string maxCheckinTim, int userId);
        Task SetCancellationPolicy(long listingRentId, int? cancellationPolicyTypeId, int userId);
        Task<List<TlListingRent>> GetAllResumed(int userID, bool onlyPublish);
        Task<List<TlListingRent>> GetCalendarData(int userID);
        Task<List<TlListingRent>> GetUnfinishedList(int ownerId);

        Task<string> SetMaxMinStay(long listingRentId, bool setMaxStay, int maxStayValue,
            bool setMinStay, int minStayValue, int userId);
        Task<string> SetMinimumNotice(long listingRentId, int minimumNoticeDay,
            TimeSpan? minimumNoticeHours, int userId);
        
        Task<string> SetAdditionalFee(long listingRentId,
            List<int> additionalFeeId, List<decimal> value);
        Task<List<TlListingAdditionalFee>> GetAdditionalFeesByListingRentId(
            long listingRentId);
            TimeSpan? minimumNoticeHours, int userId);
        Task<string> SetPreparationDay(long listingRentId, int preparationDay, int userId);
    }
}
