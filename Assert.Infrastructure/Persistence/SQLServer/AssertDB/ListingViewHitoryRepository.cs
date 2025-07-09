using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingViewHistoryRepository : IListingViewHistoryRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingViewHistoryRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<(List<TlListingRent>, PaginationMetadata)> GetViewsHistory(int userId, int pageNumber = 1, int pageSize = 10, int? countryId = null)
        {
            var skipAmount = (pageNumber - 1) * pageSize;

            var query = _context.TlListingViewHistories.Where(x=>x.UserId == userId)
                .OrderByDescending(x => x.ViewDate)
                .Select(x => x.ListingRent)
                .Include(x => x.ListingStatus)
                .Include(x => x.AccomodationType)
                .Include(x => x.OwnerUser)
                .Include(x => x.TpProperties)
                .Include(x => x.TpProperties)
                    .ThenInclude(y => y.PropertySubtype)
                        .ThenInclude(y => y.PropertyType)
                .Include(x => x.TlListingPhotos)
                .Include(x => x.TlListingPrices)
                .Include(x => x.TlListingSpecialDatePrices)
                .Include(x => x.TlListingReviews)
                .AsNoTracking()
                .Where(x => x.ListingStatusId == 3 && (countryId == null || countryId == 0 || x.TpProperties.FirstOrDefault().CountryId == countryId))
                ;

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

            PaginationMetadata pagination = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItemCount = await query.CountAsync(),
                TotalPageCount = (int)Math.Ceiling((double)await query.CountAsync() / pageSize)
            };

            return (result, pagination);
        }

        public async Task ToggleFromHistory(long listingRentId, bool setAsFavorite, int userId)
        {
            TlListingViewHistory listing = _context.TlListingViewHistories.Where(x => x.ListingRentId == listingRentId && x.UserId == userId).FirstOrDefault();
            if (setAsFavorite)
            {
                if (listing == null)
                {
                    listing = new TlListingViewHistory
                    {
                        ListingRentId = listingRentId,
                        UserId = userId,
                        ViewDate = DateTime.UtcNow
                    };
                    await _context.TlListingViewHistories.AddAsync(listing);
                }
                else
                {
                    listing.ViewDate = DateTime.UtcNow;
                }
            }
            else
            {
                if (listing != null)
                {
                    _context.TlListingViewHistories.Remove(listing);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
