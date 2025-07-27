using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Assert.Application.Interfaces
{
    public interface IAppListingRentService
    {
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetAllListingsRentsData(int ownerUserId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<ListingRentDTO>> Get(long istingRentId, bool onlyActive, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<ProcessDataResult>> ProcessListingData(long listinRentId, ProcessDataRequest request, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<List<ReturnModelDTO>> UploadImages(IEnumerable<IFormFile> imageFiles, Dictionary<string, string> clientData);
        Task<(ReturnModelDTO<List<ListingRentDTO>>, PaginationMetadataDTO)> GetFeaturedListings(long userId, int? countryId, int pageNumber, int pageSize, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetByOwner(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<PhotoDTO>>> GetPhotoByListigRent(long listinRentId, Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<ReviewDTO>>> GetListingRentReviews(int listingRentId, bool UseTechnicalMessages, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<ListingReviewSummaryDTO>> GetListingRentReviewsSummary(long listingRentId, int topCount, bool UseTechnicalMessages, Dictionary<string, string> requestInfo); Task<ReturnModelDTO> DeletePhoto(long listingRentId, int photoId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<PhotoDTO>> UpdatePhoto(long listingRentId, int photoId, UploadImageListingRent request, Dictionary<string, string> requestInfo);
        Task<List<ReturnModelDTO>> UploadImagesDescription(long listingRentId, List<UploadImageRequest> imagesDescription, Dictionary<string, string> clientData);

        Task<ReturnModelDTO<string>> UpdateBasicData(long listingRentId, BasicListingRentData basicData);

        Task<ReturnModelDTO<string>> UpdatePricesAndDiscounts(long listingRentId,
            PricesAndDiscountRequest pricingData);
        Task<ReturnModelDTO<ListingRentDTO>> Get(long istingRentId, long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<ListingRentDTO>>> GetByOwnerResumed(Dictionary<string, string> requestInfo, bool useTechnicalMessages);


        Task<ReturnModelDTO<string>> UpdatePropertyAndAccomodationTypes(long listingRentId,
                int? propertyTypeId, int? accomodationTypeId);
        Task<ReturnModelDTO<string>> UpdateCapacity(long listingRentId,
            int beds, int bedrooms, int bathrooms, int maxGuests,
            int privateBathroom, int privateBathroomLodging, int sharedBathroom);

        Task<ReturnModelDTO<string>> UpdatePropertyLocation(long listingRentId,
            int cityId, int countyId, int stateId, double latitude, double longitude,
            string address1, string address2, string zipCode);

        Task<ReturnModelDTO<string>> UpdateCharasteristics(long listingRentId,
            Dictionary<string, string> clientData, List<int> featuredAmenities, List<int> featureAspects,
            List<int> securityItems);

        Task<ReturnModelDTO<string>> UpdateCancellationPolicy(long listingRentId,
            int cancellationPolicyId);

        Task<ReturnModelDTO<string>> UpdateReservation(long listingRentId,
           int approvalPolicyTypeId, int minimunNoticeDays, int preparationDays);

        Task<ReturnModelDTO<List<ListingRentCalendarDTO>>> GetCalendarByOwner(Dictionary<string, string> requestInfo, bool v);
    }
}
