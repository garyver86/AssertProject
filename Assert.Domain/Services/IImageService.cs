using Assert.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Assert.Domain.Services
{
    public interface IImageService
    {
        Task RemoveListingRentImage(string fileName);
        Task<List<ReturnModel>> SaveListingRentImages(IEnumerable<IFormFile> imageFiles, bool useTechnicalMessages);
        Task<bool> VerifyListingRentImage(string fileName);
    }
}
