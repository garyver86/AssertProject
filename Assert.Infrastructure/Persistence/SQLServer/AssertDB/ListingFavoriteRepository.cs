using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingFavoriteRepository : IListingFavoriteRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;

        public ListingFavoriteRepository(InfraAssertDbContext infraAssertDbContext, IServiceProvider serviceProvider)
        {
            _context = infraAssertDbContext;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }

        public async Task<List<TlListingFavoriteGroup>> GetFavoriteGroups(int userId)
        {
            List<TlListingFavoriteGroup> groups = await _context.TlListingFavoriteGroups
                .Where(x => x.UserId == userId && x.GroupStatus == 1)
                //.Include(x => x.TlListingFavorites)
                .AsNoTracking()
                .ToListAsync();

            foreach (var group in groups)
            {
                var _favorites = await _context.TlListingFavorites
                                        .Where(x => x.FavoriteGroupId == group.FavoriteGroupListingId && x.UserId == userId)
                                        .OrderByDescending(x => x.CreateAt).ToListAsync();
                group.UserId = _favorites.Count(); // Set the userId to the count of favorites in the group
                if (_favorites?.FirstOrDefault() != null)
                {
                    var last = _favorites.First();
                    var ListingRent = await _context.TlListingRents.Where(x => x.ListingRentId == last.ListingRentId)
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
                                                .AsNoTracking().FirstOrDefaultAsync();
                    last.ListingRent = ListingRent;
                    group.TlListingFavorites = new TlListingFavorite[] { last };
                }
                else
                {
                    group.TlListingFavorites = new TlListingFavorite[]
                    {
                        new TlListingFavorite
                        {
                            ListingRentId = 0, // Default value if no favorites exist
                            CreateAt = DateTime.UtcNow // Set to current time if no favorites exist
                        }
                    };

                }
            }
            return groups;
        }

        public async Task<TlListingFavoriteGroup?> GetFavoriteGroupById(long groupId, int userId)
        {
            TlListingFavoriteGroup? group = await _context.TlListingFavoriteGroups
                .Where(x => x.FavoriteGroupListingId == groupId && x.UserId == userId && x.GroupStatus == 1)
                .Include(x => x.TlListingFavorites).ThenInclude(x => x.ListingRent)
                .FirstOrDefaultAsync();
            return group;
        }

        public async Task<TlListingFavoriteGroup> CreateFavoriteGroup(string groupName, int userId)
        {
            TlListingFavoriteGroup group = new TlListingFavoriteGroup
            {
                FavoriteGroupName = groupName,
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                GroupStatus = 1
            };
            await _context.TlListingFavoriteGroups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task RemoveFavoriteGroup(long groupId, int userId)
        {
            TlListingFavoriteGroup? group = await _context.TlListingFavoriteGroups
                .Where(x => x.FavoriteGroupListingId == groupId && x.UserId == userId)
                .FirstOrDefaultAsync();
            if (group == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to remove this favorite group.");
            }
            //_context.TlListingFavoriteGroups.Remove(group);
            group.GroupStatus = 0;
            await _context.SaveChangesAsync();
        }

        public async Task ToggleFavorite(long listingRentId, long? groupId, bool setAsFavorite, int userId)
        {
            TlListingFavoriteGroup group = null;
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                if (groupId > 0)
                {
                    group = dbContext.TlListingFavoriteGroups.Where(x => x.FavoriteGroupListingId == groupId && x.GroupStatus == 1)
                        .Include(x => x.TlListingFavorites).FirstOrDefault();
                }
                else
                {
                    group = dbContext.TlListingFavorites.Include(x => x.FavoriteGroup).Where(x => x.ListingRentId == listingRentId && x.UserId == userId && x.FavoriteGroup.GroupStatus == 1).ToList().OrderByDescending(x => x.FavoriteGroupId).FirstOrDefault()?.FavoriteGroup;
                    if (group == null)
                    {
                        group = dbContext.TlListingFavoriteGroups.Where(x => x.UserId == userId && x.GroupStatus == 1).OrderByDescending(x => x.CreationDate).ToList().FirstOrDefault();
                    }
                }

                if (group == null)
                {
                    if (setAsFavorite)
                    {
                        throw new Exceptions.NotFoundException("No cuenta con una lista de favoritos para asignar la propiedad.");
                    }
                    else
                    {
                        return;
                    }
                }

                if (group.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to modify this favorite group.");
                }

                TlListingFavorite listing = group.TlListingFavorites.Where(x => x.ListingRentId == listingRentId).FirstOrDefault();
                if (setAsFavorite)
                {
                    if (listing == null)
                    {
                        listing = new TlListingFavorite
                        {
                            ListingRentId = listingRentId,
                            UserId = userId,
                            CreateAt = DateTime.UtcNow,
                            FavoriteGroupId = groupId?? group.FavoriteGroupListingId
                        };
                        await dbContext.TlListingFavorites.AddAsync(listing);
                    }
                }
                else
                {
                    if (listing != null)
                    {
                        dbContext.TlListingFavorites.Remove(listing);
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<long>> GetAllFavoritesList(long userId)
        {
            List<long> favoriresList = await _context.TlListingFavorites
               .Where(x => x.UserId == userId && x.FavoriteGroup.GroupStatus == 1)
               .AsNoTracking()
               .Select(x => x.ListingRentId).ToListAsync();
            return favoriresList;
        }
    }
}
