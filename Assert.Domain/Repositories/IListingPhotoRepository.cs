using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingPhotoRepository
    {
        Task<ReturnModel> DeleteListingRentImage(long listingRentId, int photoId);
        Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId);
        Task<List<TlListingPhoto>> GetByListingRentId(long listinRentId, int userID);
        Task<ReturnModel> UploadPhoto(long listingRentId, string fileName, string description, int? spaceType, bool isMain);
        Task<TlListingPhoto> UpdatePhoto(long listingRentId, ProcessData_PhotoModel photo);
    }
}
