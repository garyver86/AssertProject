using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
using Assert.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class SearchService : ISearchService
    {
        private readonly InfraAssertDbContext _context;
        private readonly ICityRepository _cityRepository;
        private readonly IListingFavoriteRepository _favoritesRepository;
        public SearchService(InfraAssertDbContext dbContext, ICityRepository cityRepository, IListingFavoriteRepository listingFavoriteRepository)
        {
            _context = dbContext;
            _cityRepository = cityRepository;
            _favoritesRepository = listingFavoriteRepository;
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

        public async Task<ReturnModel<(List<TlListingRent>, PaginationMetadata)>> SearchPropertiesAsync(SearchFilters filters, int pageNumber, int pageSize, long userId)
        {
            List<long> favoritesList = await _favoritesRepository.GetAllFavoritesList(userId);

            var skipAmount = (pageNumber - 1) * pageSize;
            var query = _context.TlListingRents.AsQueryable();

            query = query.Where(x => x.ListingStatus.Code == "PUBLISH");

            if (filters.CityId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().CityId == filters.CityId);
            }

            if (filters.CountyId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().CountyId == filters.CountyId);
            }

            if (filters.StateId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().StateId == filters.StateId);
            }

            if (filters.CountryId > 0)
            {
                query = query.Where(p => p.TpProperties.FirstOrDefault().CountryId == filters.CountryId);
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
                query = query.Where(listing => filters.AmenityIds.All(amenityId =>
                   listing.TlListingAmenities.Any(listingAmenity => listingAmenity.AmenitiesTypeId == amenityId)));

            }

            if (filters.CheckInDate.HasValue && filters.CheckOutDate.HasValue)
            {
                query = query.Where(p => !p.TbBooks.Any(b =>
                    (b.StartDate <= filters.CheckOutDate.Value && b.EndDate >= filters.CheckInDate.Value)));
            }

            if (filters?.Rules?.AllowedPets != null)
            {
                query = query.Where(p => p.TlListingRentRules.Any(b => b.RuleTypeId == 1 &&
                  (filters.Rules.AllowedPets == b.Value )));
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
                    .Include(x => x.OwnerUser)
                    .Include(x => x.TlListingAmenities)
                        .ThenInclude(y => y.AmenitiesType)
                    .Include(x => x.TlListingFeaturedAspects)
                        .ThenInclude(y => y.FeaturesAspectType)
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
                        //.Include(x => x.TpProperties)
                        .ThenInclude(y => y.PropertySubtype)
                            .ThenInclude(y => y.PropertyType)
                    .Include(x => x.TlListingPhotos)
                    .Include(x => x.TlListingPrices)
                    .Include(x => x.TlListingRentRules)
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
                foreach (var listing in result)
                {
                    listing.isFavorite = favoritesList.Contains(listing.ListingRentId);
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
            PaginationMetadata pagination = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItemCount = await query.CountAsync(),
                TotalPageCount = (int)Math.Ceiling((double)await query.CountAsync() / pageSize)
            };

            return new ReturnModel<(List<TlListingRent>, PaginationMetadata)>
            {
                StatusCode = ResultStatusCode.OK,
                Data = (result, pagination)
            };
        }
    }
}
