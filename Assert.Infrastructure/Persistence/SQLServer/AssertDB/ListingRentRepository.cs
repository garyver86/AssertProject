using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingRentRepository : IListingRentRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IListingLogRepository _logRepository;
        private readonly IListingStatusRepository _listingStatusRepository;
        public ListingRentRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository logRepository, IListingStatusRepository listingStatusRepository)
        {
            _context = infraAssertDbContext;
            _logRepository = logRepository;
            _listingStatusRepository = listingStatusRepository;
        }

        public async Task<TlListingRent> ChangeStatus(long id, int ownerID, int newStatus, Dictionary<string, string> userInfo)
        {
            var listing = _context.TlListingRents.Where(x => x.ListingRentId == id).FirstOrDefault();
            listing.ListingStatusId = newStatus;
            var result = await _context.SaveChangesAsync();

            var statusListing = await _listingStatusRepository.Get(newStatus);

            _logRepository.RegisterLog(id, "Update Status " + statusListing.Code + " (StatusId:" + statusListing.ListingStatusId.ToString() + ")", userInfo["BrowserInfo"], userInfo["IsMobile"] == "True", userInfo["IpAddress"], userInfo["AdditionalData"], userInfo["ApplicationCode"]);

            return listing;
        }

        public async Task<TlListingRent> Get(long id, bool onlyActive)
        {
            var result = _context.TlListingRents
                .Include(x => x.ListingStatus)
                .Include(x => x.AccomodationType)
                .Include(x => x.ApprovalPolicyType)
                .Include(x => x.CancelationPolicyType)
                .Include(x => x.OwnerUser)
                .Include(x => x.TlListingAmenities)
                    .ThenInclude(y => y.AmenitiesType)
                .Include(x => x.TlCheckInOutPolicies)
                .Include(x => x.TlListingFeaturedAspects)
                    .ThenInclude(y => y.FeaturesAspectType)
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.TpPropertyAddresses)
                .Include(x => x.TlListingPhotos)
                .Include(x => x.TlListingPrices)
                .Include(x => x.TlListingRentRules)
                    .ThenInclude(y => y.RuleType)
                .Include(x => x.TlListingSecurityItems)
                    .ThenInclude(y => y.SecurityItemType)
                .Include(x => x.TlListingSpaces)
                    .ThenInclude(y => y.SpaceType)
                .Include(x => x.TlListingSpecialDatePrices)
                .Include(x => x.TlStayPresences)
                    .ThenInclude(y => y.StayPrecenseType)
                .AsNoTracking()
                .Where(x => x.ListingRentId == id && (!onlyActive || (onlyActive && x.ListingStatusId == 3))).FirstOrDefault();

            TlListingRent? tlListingRent = await Task.FromResult(result);
            return tlListingRent;
        }

        public async Task<TlListingRent> Get(long id, int ownerID)
        {
            var result = _context.TlListingRents.Where(x => x.ListingRentId == id && x.OwnerUserId == ownerID).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<List<TlListingRent>> GetAll(int ownerID)
        {
            var result = _context.TlListingRents.Where(x => x.OwnerUserId == ownerID).ToList();
            return await Task.FromResult(result);
        }

        public async Task<bool> HasStepInProcess(long listingRentId)
        {
            var result = await _context.TlListingSteps.Where(x => x.ListingRentId == listingRentId && x.ListingStepsStatusId == 1).FirstOrDefaultAsync() != null;
            return result;
        }

        public async Task<TlListingRent> Register(TlListingRent listingRent, Dictionary<string, string> clientData)
        {
            _context.TlListingRents.Add(listingRent);
            await _context.SaveChangesAsync();

            _logRepository.RegisterLog(listingRent.ListingRentId, "Create Listing Rent " + listingRent.ListingRentId, clientData["BrowserInfo"], clientData["IsMobile"] == "True", clientData["IpAddress"], clientData["AdditionalData"], clientData["ApplicationCode"]);

            return listingRent;
        }

        public Task<TlListingRent> SetAccomodationType(long propertyId, int? subtypeId)
        {
            throw new NotImplementedException();
        }

        public Task SetApprovalPolicy(long listingRentId, int? approvalPolicyTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task SetCapacity(long listingRentId, int? beds, int? bedrooms, bool? allDoorsLocked, int? maxGuests)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId).FirstOrDefault();
            listing.Beds = beds;
            listing.Bedrooms = bedrooms;
            listing.AllDoorsLocked = allDoorsLocked;
            listing.MaxGuests = maxGuests;
            await _context.SaveChangesAsync();
        }

        public async Task SetCapacity(long listingRentId, int? beds, int? bedrooms, int? bathrooms, int? maxGuests)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId).FirstOrDefault();
            listing.Beds = beds;
            listing.Bedrooms = bedrooms;
            listing.Bathrooms = bathrooms;
            listing.MaxGuests = maxGuests;
            await _context.SaveChangesAsync();
        }

        public Task SetDescription(long listingRentId, string description)
        {
            throw new NotImplementedException();
        }

        public Task SetName(long listingRentId, string title)
        {
            throw new NotImplementedException();
        }

        public Task SetNameAndDescription(long listingRentId, string title, string description)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityConfirmationData(long listingRentId, bool? presenceOfWeapons, bool? noiseDesibelesMonitor, bool? externalCameras)
        {
            throw new NotImplementedException();
        }

        Task IListingRentRepository.SetAsConfirmed(long listingRentId)
        {
            throw new NotImplementedException();
        }
    }
}
