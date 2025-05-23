﻿using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingRentRepository
    {
        Task<TlListingRent> Get(long id, bool onlyActive);
        Task<TlListingRent> Get(long id, int ownerID);
        Task<TlListingRent> ChangeStatus(long id, int ownerID, int newStatus, Dictionary<string, string> userInfo);
        Task<List<TlListingRent>> GetAll(int ownerUserId);
        Task<TlListingRent> Register(TlListingRent listingRent, Dictionary<string, string> clientData);
        Task<bool> HasStepInProcess(long listingRentId);
        Task<TlListingRent> SetAccomodationType(long propertyId, int? subtypeId);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, bool? allDoorsLocked, int? maxGuests);
        Task SetCapacity(long listingRentId, int? beds, int? bedrooms, int? bathrooms, int? maxGuests);
        Task SetAsConfirmed(long listingRentId);
        Task SetSecurityConfirmationData(long listingRentId, bool? presenceOfWeapons, bool? noiseDesibelesMonitor, bool? externalCameras);
        Task SetApprovalPolicy(long listingRentId, int? approvalPolicyTypeId);
        Task SetDescription(long listingRentId, string description);
        Task SetName(long listingRentId, string title);
        Task SetNameAndDescription(long listingRentId, string title, string description);
        //Task<List<TlListingRent>> GetFeatureds(int? countryId, int? limit);
        Task<List<TlListingRent>> GetFeatureds(int pageNumber = 1, int pageSize = 10, int? countryId = null);
        
        Task<string> UpdateBasicData(long listingRentId, string title, string description, List<int> aspectTypeIdList);
    }
}
