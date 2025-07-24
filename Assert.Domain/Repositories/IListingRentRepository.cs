using Assert.Domain.Entities;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IListingRentRepository
    {
        Task<TlListingRent> Get(long id, int guestId, bool onlyActive);
        Task<TlListingRent> Get(long id, long ownerID);
        Task<TlListingRent> ChangeStatus(long id, int ownerID, int newStatus, Dictionary<string, string> userInfo);
        Task<List<TlListingRent>> GetAll(int ownerUserId);
        Task<TlListingRent> Register(TlListingRent listingRent, Dictionary<string, string> clientData);
        Task<bool> HasStepInProcess(long listingRentId);
        Task<TlListingRent> SetAccomodationType(long propertyId, int? subtypeId);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, bool? allDoorsLocked, int? maxGuests, int? privateBathroom, int? privateBathroomLodging, int? sharedBathroom);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, int? bathrooms, int? maxGuests, int? privateBathroom, int? privateBathroomLodging, int? sharedBathroom);
        Task SetAsConfirmed(long listingRentId);
        Task SetSecurityConfirmationData(long listingRentId, bool? presenceOfWeapons, bool? noiseDesibelesMonitor, bool? externalCameras);
        Task SetApprovalPolicy(long listingRentId, int? approvalPolicyTypeId);
        Task SetDescription(long listingRentId, string description);
        Task SetName(long listingRentId, string title);
        Task SetNameAndDescription(long listingRentId, string title, string description);
        Task<(List<TlListingRent>, PaginationMetadata)> GetFeatureds(long userId, int pageNumber = 1, int pageSize = 10, int? countryId = null);
        
        Task<string> UpdateBasicData(long listingRentId, string title, string description, List<int> aspectTypeIdList);
        Task SetReservationTypeApprobation(long listingRentId, int value1, int value2, int value3);
        Task SetCheckInPolicies(long listingRentId, string checkinTime, string checkoutTime, string instructions, string maxCheckinTime);
        Task SetCancellationPolicy(long listingRentId, int? cancellationPolicyTypeId);
        Task<List<TlListingRent>> GetAllResumed(int userID, bool onlyPublish);
    }
}
