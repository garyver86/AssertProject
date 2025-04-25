using Assert.Application.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace Assert.Application.Interfaces
{
    public interface IAppListingRentService
    {
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetAllListingsRentsData(int ownerUserId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<ListingRentDTO>> Get(long istingRentId, bool onlyActive, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<ProcessDataResult>> ProcessListingData(long listinRentId, ProcessDataRequest request, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<List<ReturnModelDTO>> UploadImages(IEnumerable<IFormFile> imageFiles, Dictionary<string, string> clientData);
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetFeaturedListings(int? countryId, int? limit, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetByOwner(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<PhotoDTO>>> GetPhotoByListigRent(long listinRentId, Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<ReviewDTO>>> GetListingRentReviews(int listingRentId, bool UseTechnicalMessages, Dictionary<string, string> requestInfo);
    }
}
