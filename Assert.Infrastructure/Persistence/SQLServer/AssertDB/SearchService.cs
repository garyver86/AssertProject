using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class SearchService : ISearchService
    {
        private readonly InfraAssertDbContext _context;
        private readonly ICityRepository _cityRepository;
        public SearchService(InfraAssertDbContext dbContext, ICityRepository cityRepository)
        {
            _context = dbContext;
            _cityRepository = cityRepository;
        }

        public async Task<ReturnModel<List<TCity>>> SearchCities(string filter, int filterType)
        {
            var result = await _cityRepository.FindByFilter(filter, filterType);
            return new ReturnModel<List<TCity>>
            {
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        public async Task<ReturnModel<List<TlListingRent>>> SearchPropertiesAsync(SearchFilters filters, int pageNumber, int pageSize)
        {
            var skipAmount = (pageNumber - 1) * pageSize;
            var query = _context.TlListingRents.AsQueryable();

            query = query.Where(x => x.ListingStatus.Code == "PUBLISH");

            if (filters.CityId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().TpPropertyAddresses.FirstOrDefault().CityId == filters.CityId);
            }

            if (filters.CountryId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().TpPropertyAddresses.FirstOrDefault().City.County.State.CountryId == filters.CountryId);
            }

            if (filters.MinPrice.HasValue && !filters.MaxPrice.HasValue)
            {
                query = query.Where(p => p.TlListingPrices.FirstOrDefault().PriceNightly >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue && !filters.MinPrice.HasValue)
            {
                query = query.Where(p => p.TlListingPrices.FirstOrDefault().PriceNightly <= filters.MaxPrice.Value);
            }

            if (filters.MaxPrice.HasValue && filters.MinPrice.HasValue)
            {
                query = query.Where(p => filters.MinPrice.Value <= p.TlListingPrices.FirstOrDefault().PriceNightly && p.TlListingPrices.FirstOrDefault().PriceNightly <= filters.MaxPrice.Value);
            }

            if (filters.Guests.HasValue)
            {
                query = query.Where(p => p.MaxGuests >= filters.Guests.Value);
            }

            if (filters.Bedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms >= filters.Bedrooms.Value);
            }
            if (filters.Bathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms >= filters.Bathrooms.Value);
            }

            if (filters.Beds.HasValue)
            {
                query = query.Where(p => p.Beds >= filters.Beds.Value);
            }

            if (filters.PropertyTypeId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().PropertySubtypeId == filters.PropertyTypeId);
            }

            if (filters.AmenityIds != null && filters.AmenityIds.Any())
            {
                //query = query.Where(p => p.TlListingAmenities.Any(pa => filters.AmenityIds.Contains(pa.AmenitiesTypeId)));
                query = query.Where(listing => filters.AmenityIds.All(amenityId =>
                   listing.TlListingAmenities.Any(listingAmenity => listingAmenity.AmenitiesTypeId == amenityId)));

            }

            if (filters.CheckInDate.HasValue && filters.CheckOutDate.HasValue)
            {
                query = query.Where(p => !p.TbBooks.Any(b =>
                    (b.StartDate <= filters.CheckOutDate.Value && b.EndDate >= filters.CheckInDate.Value)));
            }

            List<double> boundingBox = null;
            if (filters.Latitude.HasValue && filters.Longitude.HasValue && filters.Radius.HasValue)
            {
                boundingBox = GeoUtils.CalculateBoundingBox(filters.Latitude.Value, filters.Longitude.Value, filters.Radius ?? 0);

                var minLat = boundingBox[0];
                var maxLat = boundingBox[1];
                var minLon = boundingBox[2];
                var maxLon = boundingBox[3];

                var referenceLat = filters.Latitude.Value;
                var referenceLon = filters.Longitude.Value;
                var radius = filters.Radius.Value;

                query = query.Where(p =>
                    p.TpProperties.FirstOrDefault().Latitude >= minLat && p.TpProperties.FirstOrDefault().Latitude <= maxLat &&
                    p.TpProperties.FirstOrDefault().Longitude >= minLon && p.TpProperties.FirstOrDefault().Longitude <= maxLon);
            }

            query = query.Include(x => x.ListingStatus)
                    .Include(x => x.ListingStatus)
                    //.Include(x => x.AccomodationType)
                    //.Include(x => x.ApprovalPolicyType)
                    //.Include(x => x.CancelationPolicyType)
                    .Include(x => x.OwnerUser)
                    .Include(x => x.TlListingAmenities)
                        .ThenInclude(y => y.AmenitiesType)
                    //.Include(x => x.TlCheckInOutPolicies)
                    .Include(x => x.TlListingFeaturedAspects)
                        .ThenInclude(y => y.FeaturesAspectType)
                    .Include(x => x.TpProperties)
                        .ThenInclude(y => y.TpPropertyAddresses)
                        .ThenInclude(y => y.City)
                        .ThenInclude(y => y.County)
                        .ThenInclude(y => y.State)
                        .ThenInclude(y => y.Country)
                    .Include(x => x.TpProperties)
                        .ThenInclude(y => y.TpPropertyAddresses)
                        .ThenInclude(y => y.County)
                        .ThenInclude(y => y.State)
                        .ThenInclude(y => y.Country)
                    .Include(x => x.TpProperties)
                        .ThenInclude(y => y.TpPropertyAddresses)
                        .ThenInclude(y => y.State)
                        .ThenInclude(y => y.Country)
                    .Include(x => x.TpProperties)
                        .ThenInclude(y => y.PropertySubtype)
                            .ThenInclude(y => y.PropertyType)
                    .Include(x => x.TlListingPhotos)
                    .Include(x => x.TlListingPrices)
                    //.Include(x => x.TlListingRentRules)
                    //    .ThenInclude(y => y.RuleType)
                    //.Include(x => x.TlListingSecurityItems)
                    //    .ThenInclude(y => y.SecurityItemType)
                    //.Include(x => x.TlListingSpaces)
                    //    .ThenInclude(y => y.SpaceType)
                    //.Include(x => x.TlListingSpecialDatePrices)
                    //.Include(x => x.TlStayPresences)
                    //    .ThenInclude(y => y.StayPrecenseType)
                    .AsNoTracking();

            var properties = await query
               .Skip(skipAmount)
               .Take(pageSize)
               .ToListAsync();

            List<TlListingRent> result = new List<TlListingRent>();
            if (boundingBox != null)
            {
                foreach (var prop in properties)
                {
                    bool cumple = false;
                    foreach (var p in prop.TpProperties)
                    {
                        double distance = GeoUtils.CalculateDistanceMeters(filters.Latitude ?? 0, filters.Longitude ?? 0, p.Latitude ?? 0, p.Longitude ?? 0);
                        if (distance <= filters.Radius)
                        {
                            p.DistanceMeters = distance;
                            cumple = true;
                        }
                    }
                    if (cumple)
                    {
                        result.Add(prop);
                    }
                }
            }
            else
            {
                result = properties;
            }

            if (result != null)
            {
                foreach (var prop in result)
                {
                    if (prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault() != null)
                    {
                        if (prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault().City != null)
                        {
                            prop.TpProperties.FirstOrDefault().TpPropertyAddresses.FirstOrDefault().State = prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault().City.County.State;
                            prop.TpProperties.FirstOrDefault().TpPropertyAddresses.FirstOrDefault().County = prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault().City.County;
                        }
                        if (prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault().County != null)
                        {
                            prop.TpProperties.FirstOrDefault().TpPropertyAddresses.FirstOrDefault().State = prop.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault().County.State;
                        }
                    }
                }
            }

            return new ReturnModel<List<TlListingRent>>
            {
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }
    }
}
