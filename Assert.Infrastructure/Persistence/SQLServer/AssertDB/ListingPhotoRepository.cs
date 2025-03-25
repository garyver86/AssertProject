using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPhotoRepository : IListingPhotoRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IImageService _imageService;
        public ListingPhotoRepository(InfraAssertDbContext infraAssertDbContext, IImageService imageService)
        {
            _context = infraAssertDbContext;
            _imageService = imageService;
        }
        public List<TlListingPhoto> GetByListingRent(long listingRentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
    }
}
