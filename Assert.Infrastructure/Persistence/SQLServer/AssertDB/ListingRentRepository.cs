using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

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
            // Paso 1: Obtener datos básicos del listing
            var listingData = await _context.TlListingRents
         .AsNoTracking()
         .Where(x => x.ListingRentId == id && x.ListingStatusId != 5)
         .Select(x => new
         {
             Listing = x,
             Status = x.ListingStatus,
             AccomodationType = x.AccomodationType,
             ApprovalPolicy = x.ApprovalPolicyType,
             CancelationPolicy = x.CancelationPolicyType,
             Owner = x.OwnerUser
         })
         .FirstOrDefaultAsync();

            if (listingData == null) return null;

            var listing = listingData.Listing;

            // Paso 2: Cargar relaciones secuencialmente (evitando problemas de concurrencia)
            listing.TlListingAmenities = await LoadAmenitiesAsync(_context, id);
            listing.TlListingFeaturedAspects = await LoadFeaturesAsync(_context, id);
            listing.TpProperties = await LoadPropertiesAsync(_context, id);
            listing.TlListingPhotos = await LoadPhotosAsync(_context, id);
            listing.TlListingPrices = await LoadPricesAsync(_context, id);
            listing.TlListingRentRules = await LoadRulesAsync(_context, id);
            listing.TlListingSecurityItems = await LoadSecurityItemsAsync(_context, id);
            listing.TlListingSpaces = await LoadSpacesAsync(_context, id);
            listing.TlListingSpecialDatePrices = await LoadSpecialDatesAsync(_context, id);
            listing.TlStayPresences = await LoadStayPresencesAsync(_context, id);
            listing.TlListingReviews = await LoadReviewsAsync(_context, id);
            listing.TlCheckInOutPolicies = await LoadCheckInOutPoliciesAsync(_context, id);

            // Asignar propiedades de navegación
            listing.ListingStatus = listingData.Status;
            listing.AccomodationType = listingData.AccomodationType;
            listing.ApprovalPolicyType = listingData.ApprovalPolicy;
            listing.CancelationPolicyType = listingData.CancelationPolicy;
            listing.OwnerUser = listingData.Owner;

            return listing;

            //return tlListingRent;
        }

        // Métodos auxiliares para cargar cada relación
        private async Task<List<TlListingAmenity>> LoadAmenitiesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingAmenities
                .AsNoTracking()
                .Where(a => a.ListingRentId == listingId)
                .Include(a => a.AmenitiesType)
                .ToListAsync();
        }

        private async Task<List<TlListingFeaturedAspect>> LoadFeaturesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingFeaturedAspects
                .AsNoTracking()
                .Where(f => f.ListingRentId == listingId)
                .Include(f => f.FeaturesAspectType)
                .ToListAsync();
        }

        private async Task<List<TpProperty>> LoadPropertiesAsync(InfraAssertDbContext _context, long listingId)
        {
            var result = await _context.TpProperties
                .AsNoTracking()
                .Where(p => p.ListingRentId == listingId)
                //.Include(p => p.TpPropertyAddresses)
                //    .ThenInclude(a => a.City)
                //    .ThenInclude(c => c.County)
                //    .ThenInclude(co => co.State)
                //    .ThenInclude(s => s.Country)
                .Include(p => p.PropertySubtype)
                    .ThenInclude(ps => ps.PropertyType)
                .ToListAsync();

            foreach (var prop in result)
            {
                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                {
                    new TpPropertyAddress
                    {
                        Address1 = prop.Address1,
                        Address2 = prop.Address2,
                        CityId = prop.CityId,
                        CountyId = prop.CountyId,
                        ZipCode = prop.ZipCode,
                        StateId = prop.StateId,
                        City = new TCity
                        {
                            CityId = prop.CityId??0,
                            Name = prop.CityName,
                            CountyId = prop.CountyId??0,
                            County = new TCounty
                            {
                                CountyId = prop.CountyId ?? 0,
                                Name = prop.CountyName,
                                State = new TState
                                {
                                    Name = prop.StateName,
                                    StateId = prop.StateId ?? 0,
                                    Country = new TCountry
                                    {
                                        Name = prop.CountryName,
                                        CountryId = prop.CountryId ?? 0
                                    }
                                },
                                StateId = prop.StateId??0
                            }
                        }
                    }
                };
                prop.City = prop.TpPropertyAddresses.FirstOrDefault().City;
            }
            return result;
        }

        private async Task<List<TlListingPhoto>> LoadPhotosAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingPhotos
                .AsNoTracking()
                .Where(p => p.ListingRentId == listingId)
                .ToListAsync();
        }

        private async Task<List<TlListingPrice>> LoadPricesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingPrices
                .AsNoTracking()
                .Where(p => p.ListingRentId == listingId)
                .ToListAsync();
        }

        private async Task<List<TlListingRentRule>> LoadRulesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingRentRules
                .AsNoTracking()
                .Where(r => r.ListingId == listingId)
                .Include(r => r.RuleType)
                .ToListAsync();
        }

        private async Task<List<TlListingSecurityItem>> LoadSecurityItemsAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingSecurityItems
                .AsNoTracking()
                .Where(s => s.ListingRentId == listingId)
                .Include(s => s.SecurityItemType)
                .ToListAsync();
        }

        private async Task<List<TlListingSpace>> LoadSpacesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingSpaces
                .AsNoTracking()
                .Where(s => s.ListingId == listingId)
                .Include(s => s.SpaceType)
                .ToListAsync();
        }

        private async Task<List<TlListingSpecialDatePrice>> LoadSpecialDatesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingSpecialDatePrices
                .AsNoTracking()
                .Where(s => s.ListingRentId == listingId)
                .ToListAsync();
        }

        private async Task<List<TlStayPresence>> LoadStayPresencesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlStayPresences
                .AsNoTracking()
                .Where(s => s.ListingRentId == listingId)
                .Include(s => s.StayPrecenseType)
                .ToListAsync();
        }

        private async Task<List<TlListingReview>> LoadReviewsAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlListingReviews
                .AsNoTracking()
                .Where(r => r.ListingRentId == listingId)
                .ToListAsync();
        }

        private async Task<List<TlCheckInOutPolicy>> LoadCheckInOutPoliciesAsync(InfraAssertDbContext _context, long listingId)
        {
            return await _context.TlCheckInOutPolicies
                .AsNoTracking()
                .Where(c => c.ListingRentid == listingId)
                .ToListAsync();
        }

        public async Task<TlListingRent> Get(long id, int ownerID)
        {
            var listingData = await _context.TlListingRents
            .AsNoTracking()
            .Where(x => x.ListingRentId == id && x.OwnerUserId == ownerID && x.ListingStatusId != 5)
            .Select(x => new
            {
                Listing = x,
                Status = x.ListingStatus,
                AccomodationType = x.AccomodationType,
                ApprovalPolicy = x.ApprovalPolicyType,
                CancelationPolicy = x.CancelationPolicyType,
                Owner = x.OwnerUser
            })
            .FirstOrDefaultAsync();

            if (listingData == null) return null;

            var listing = listingData.Listing;

            // Paso 2: Cargar relaciones secuencialmente (evitando problemas de concurrencia)
            listing.TlListingAmenities = await LoadAmenitiesAsync(_context, id);
            listing.TlListingFeaturedAspects = await LoadFeaturesAsync(_context, id);
            listing.TpProperties = await LoadPropertiesAsync(_context, id);
            listing.TlListingPhotos = await LoadPhotosAsync(_context, id);
            listing.TlListingPrices = await LoadPricesAsync(_context, id);
            listing.TlListingRentRules = await LoadRulesAsync(_context, id);
            listing.TlListingSecurityItems = await LoadSecurityItemsAsync(_context, id);
            listing.TlListingSpaces = await LoadSpacesAsync(_context, id);
            listing.TlListingSpecialDatePrices = await LoadSpecialDatesAsync(_context, id);
            listing.TlStayPresences = await LoadStayPresencesAsync(_context, id);
            listing.TlListingReviews = await LoadReviewsAsync(_context, id);
            listing.TlCheckInOutPolicies = await LoadCheckInOutPoliciesAsync(_context, id);

            // Asignar propiedades de navegación
            listing.ListingStatus = listingData.Status;
            listing.AccomodationType = listingData.AccomodationType;
            listing.ApprovalPolicyType = listingData.ApprovalPolicy;
            listing.CancelationPolicyType = listingData.CancelationPolicy;
            listing.OwnerUser = listingData.Owner;

            return listing;
        }

        public async Task<List<TlListingRent>> GetAll(int ownerID)
        {
            var result = _context.TlListingRents.Where(x => x.OwnerUserId == ownerID && x.ListingStatusId != 5).ToList();
            return await Task.FromResult(result);
        }

        public async Task<List<TlListingRent>> GetFeatureds(int pageNumber = 1, int pageSize = 10, int? countryId = null)
        {
            var skipAmount = (pageNumber - 1) * pageSize;

            var query = _context.TlListingRents
                .Include(x => x.ListingStatus)
                .Include(x => x.AccomodationType)
                .Include(x => x.OwnerUser)
                .Include(x => x.TpProperties)
                //    .ThenInclude(y => y.TpPropertyAddresses)
                //    .ThenInclude(y => y.City)
                //    .ThenInclude(y => y.County)
                //    .ThenInclude(y => y.State)
                //    .ThenInclude(y => y.Country)
                //.Include(x => x.TpProperties)
                //    .ThenInclude(y => y.TpPropertyAddresses)
                //    .ThenInclude(y => y.County)
                //    .ThenInclude(y => y.State)
                //    .ThenInclude(y => y.Country)
                //.Include(x => x.TpProperties)
                //    .ThenInclude(y => y.TpPropertyAddresses)
                //    .ThenInclude(y => y.State)
                //    .ThenInclude(y => y.Country)
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.PropertySubtype)
                        .ThenInclude(y => y.PropertyType)
                .Include(x => x.TlListingPhotos)
                .Include(x => x.TlListingPrices)
                .Include(x => x.TlListingSpecialDatePrices)
                .Include(x => x.TlListingReviews)
                .AsNoTracking()
                .Where(x => x.ListingStatusId == 3 && (countryId == null || countryId == 0 || x.TpProperties.FirstOrDefault().CountryId == countryId))
                .OrderByDescending(x => x.TlListingReviews.Average(y => y.Calification));

            var result = await query
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
            if (result != null)
            {
                foreach (var listing in result)
                {
                    foreach (var prop in listing.TpProperties)
                    {
                        prop.TpPropertyAddresses = new List<TpPropertyAddress>
                        {
                            new TpPropertyAddress
                            {
                                Address1 = prop.Address1,
                                Address2 = prop.Address2,
                                CityId = prop.CityId,
                                CountyId = prop.CountyId,
                                ZipCode = prop.ZipCode,
                                StateId = prop.StateId,
                                City = new TCity
                                {
                                    CityId = prop.CityId??0,
                                    Name = prop.CityName,
                                    CountyId = prop.CountyId??0,
                                    County = new TCounty
                                    {
                                        CountyId = prop.CountyId ?? 0,
                                        Name = prop.CountyName,
                                        StateId = prop.StateId??0,
                                        State = new TState
                                        {
                                            Name = prop.StateName,
                                            StateId = prop.StateId ?? 0,
                                            Country = new TCountry
                                            {
                                                Name = prop.CountryName,
                                                CountryId = prop.CountryId ?? 0
                                            }
                                        }
                                    }
                                }
                            }
                        };
                    }
                }
            }
            return result;
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
