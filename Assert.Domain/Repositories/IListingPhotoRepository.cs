using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingPhotoRepository
    {
        Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId);
        Task<List<TlListingPhoto>> GetByListingRentId(long listinRentId, int userID);
        Task UpdatePhotos(long listingRentId, List<ProcessData_PhotoModel> photos);
    }
}
