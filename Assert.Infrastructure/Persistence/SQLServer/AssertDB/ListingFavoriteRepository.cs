using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingFavoriteRepository : IListingFavoriteRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingFavoriteRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TlListingFavoriteGroup>> GetFavoriteGroups(int userId)
        {
            List<TlListingFavoriteGroup> groups = await _context.TlListingFavoriteGroups
                .Where(x => x.UserId == userId)
                //.Include(x => x.TlListingFavorites)
                .ToListAsync();

            foreach (var group in groups)
            {
                var last = await _context.TlListingFavorites
                                        .Where(x => x.FavoriteGroupId == group.FavoriteGroupListingId && x.UserId == userId)
                                        .OrderByDescending(x => x.CreateAt)
                                        .FirstOrDefaultAsync();
                group.TlListingFavorites = new TlListingFavorite[]
                {
                    last ?? new TlListingFavorite
                    {
                        ListingRentId = 0, // Default value if no favorites exist
                        CreateAt = DateTime.UtcNow // Set to current time if no favorites exist
                    }
                };
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

        public async Task ToggleFavorite(long listingRentId, long groupId, bool setAsFavorite, int userId)
        {
            TlListingFavoriteGroup group = _context.TlListingFavoriteGroups.Where(x => x.FavoriteGroupListingId == groupId && x.GroupStatus == 1).FirstOrDefault();

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
                        FavoriteGroupId = groupId
                    };
                    await _context.TlListingFavorites.AddAsync(listing);
                }
            }
            else
            {
                if (listing != null)
                {
                    _context.TlListingFavorites.Remove(listing);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
