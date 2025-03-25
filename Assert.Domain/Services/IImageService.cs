using Assert.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Assert.Domain.Services
{
    public interface IImageService
    {
        Task<List<ReturnModel>> SaveListingRentImage(IEnumerable<IFormFile> imageFiles, bool useTechnicalMessages);
    }
}
