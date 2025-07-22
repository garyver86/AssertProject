using Assert.Domain.Entities;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Assert.Domain.Services
{
    public interface IImageService
    {
        Task<ReturnModel> DeleteListingRentImage(long listingRentId, int photoId, int userId, bool v);
        Task RemoveListingRentImage(string fileName);
        Task<List<ReturnModel>> SaveListingRentImages(IEnumerable<IFormFile> imageFiles, bool useTechnicalMessages);
        Task<List<ReturnModel>> SaveListingRentImages(long listingRentId, List<UploadImageListingRent> imageFiles, int userId, bool useTechnicalMessages);
        Task<ReturnModel<TlListingPhoto>> UpdatePhoto(long listingRentId, int photoId, UploadImageListingRent request, int userId, bool v);
        Task<bool> VerifyListingRentImage(string fileName);
        Task<ReturnModel> UploadProfilePhoto(int userId, IFormFile profilePhoto);
    }
}
