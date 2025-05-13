using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingRentRepository(InfraAssertDbContext _context,
        IListingLogRepository _logRepository,
        IListingStatusRepository _listingStatusRepository,
        IExceptionLoggerService _exceptionLoggerService,
        ILogger<ListingRentRepository> _logger) : IListingRentRepository
    {
        public async Task<TlListingRent> ChangeStatus(long id, int ownerID, int newStatus, Dictionary<string, string> userInfo)
        {
            var listing = _context.TlListingRents.Where(x => x.ListingRentId == id).FirstOrDefault();
            listing.ListingStatusId = newStatus;
            var result = await _context.SaveChangesAsync();

            var statusListing = await _listingStatusRepository.Get(newStatus);

            _logRepository.RegisterLog(id, "Update Status " + statusListing.Code + " (StatusId:" + statusListing.ListingStatusId.ToString() + ")", userInfo["BrowserInfo"], userInfo["IsMobile"] == "True", userInfo["IpAddress"], null, userInfo["ApplicationCode"]);

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
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.PropertySubtype)
                        .ThenInclude(y => y.PropertyType)
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
                .Where(x => x.ListingRentId == id && x.ListingStatusId != 5).FirstOrDefault();

            TlListingRent? tlListingRent = await Task.FromResult(result);
            return tlListingRent;
        }

        public async Task<TlListingRent> Get(long id, int ownerID)
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
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.PropertySubtype)
                        .ThenInclude(y => y.PropertyType)
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
                .Where(x => x.ListingRentId == id && x.OwnerUserId == ownerID && x.ListingStatusId != 5).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<List<TlListingRent>> GetAll(int ownerID)
        {
            var result = _context.TlListingRents.Where(x => x.OwnerUserId == ownerID && x.ListingStatusId != 5).ToList();
            return await Task.FromResult(result);
        }

        public async Task<List<TlListingRent>> GetFeatureds(int? countryId, int? limit)
        {
            var result = await _context.TlListingRents
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
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.PropertySubtype)
                        .ThenInclude(y => y.PropertyType)
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
                .Where(x => x.ListingStatusId == 3 && (countryId == null || countryId == 0 || x.TpProperties.FirstOrDefault().City.County.State.CountryId == countryId))
                .OrderByDescending(x => x.TlListingReviews.Average(y => y.Calification))
                .Take(limit ?? 10)
                .ToListAsync();

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

            await _logRepository.RegisterLog(listingRent.ListingRentId, "Create Listing Rent " + listingRent.ListingRentId, clientData["BrowserInfo"], clientData["IsMobile"] == "True", clientData["IpAddress"], null, clientData["ApplicationCode"]);

            return listingRent;
        }

        public async Task<TlListingRent> SetAccomodationType(long listingRentId, int? accomodationTypeId)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.AccomodationTypeId = accomodationTypeId;
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task SetApprovalPolicy(long listingRentId, int? approvalPolicyTypeId)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.ApprovalPolicyTypeId = approvalPolicyTypeId;
            await _context.SaveChangesAsync();
        }

        public async Task SetCapacity(long listingRentId, int? beds, int? bedrooms, bool? allDoorsLocked, int? maxGuests)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.Beds = beds;
            listing.Bedrooms = bedrooms;
            listing.AllDoorsLocked = allDoorsLocked;
            listing.MaxGuests = maxGuests;
            await _context.SaveChangesAsync();
        }

        public async Task SetCapacity(long listingRentId, int? beds, int? bedrooms, int? bathrooms, int? maxGuests)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.Beds = beds;
            listing.Bedrooms = bedrooms;
            listing.Bathrooms = bathrooms;
            listing.MaxGuests = maxGuests;
            await _context.SaveChangesAsync();
        }

        public async Task SetDescription(long listingRentId, string description)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.Description = description;
            await _context.SaveChangesAsync();
        }

        public async Task SetName(long listingRentId, string title)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.Name = title;
            await _context.SaveChangesAsync();
        }

        public async Task SetNameAndDescription(long listingRentId, string title, string description)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.Description = description;
            listing.Name = title;
            await _context.SaveChangesAsync();
        }

        public async Task SetSecurityConfirmationData(long listingRentId, bool? presenceOfWeapons, bool? noiseDesibelesMonitor, bool? externalCameras)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.PresenceOfWeapons = presenceOfWeapons;
            listing.NoiseDesibelesMonitor = noiseDesibelesMonitor;
            listing.ExternalCameras = externalCameras;
            await _context.SaveChangesAsync();
        }

        public async Task SetAsConfirmed(long listingRentId)
        {
            TlListingRent listing = _context.TlListingRents.Where(x => x.ListingRentId == listingRentId && x.ListingStatusId != 5).FirstOrDefault();
            listing.ListingRentConfirmationDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateBasicData(long listingRentId, string title, string description, List<int> aspectTypeIdList)
        {
            try
            {
                var listing = await _context.TlListingRents.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync();
                if (listing != null)
                {
                    listing.Name = string.IsNullOrEmpty(title) ? listing.Name : title;
                    listing.Description = string.IsNullOrEmpty(description) ? listing.Description : description;

                    if (aspectTypeIdList?.Count > 0)
                    {
                        var currentAspects = await _context.TlListingFeaturedAspects
                            .Where(l => l.ListingRentId == listingRentId).ToListAsync();

                        _context.TlListingFeaturedAspects.RemoveRange(currentAspects);

                        var newAspects = aspectTypeIdList
                            .Select(id => new TlListingFeaturedAspect
                            {
                                ListingRentId = listingRentId,
                                FeaturesAspectTypeId = id,
                                FeaturedAspectValue = ""
                            }).ToList();

                        await _context.TlListingFeaturedAspects.AddRangeAsync(newAspects);
                    }

                    await _context.SaveChangesAsync();
                    return "UPDATED";
                }
                else
                {
                    var errorMsg = $"El identificador de la actual renta no existe: {listingRentId}";
                    _logger.LogWarning(errorMsg);
                    throw new DatabaseUnavailableException(errorMsg);
                }
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { listingRentId, title, description, aspectTypeIdList });

                throw new DatabaseUnavailableException(ex.Message);
            }
        }
    }
}
