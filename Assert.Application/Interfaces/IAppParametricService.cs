using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;

namespace Assert.Application.Interfaces
{
    public interface IAppParametricService
    {
        Task<ReturnModelDTO<List<AccomodationTypeDTO>>> GetAccomodationTypes(Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<AmenityDTO>>> GetAmenityTypes(Dictionary<string, string> requestInfo, bool v);
        Task<ReturnModelDTO<List<DiscountDTO>>> GetDiscountTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<FeaturedAspectDTO>>> GetFeaturedAspects(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<PropertyTypeDTO>>> GetPropertyTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<LanguageDTO>>> GetLanguageTypes();
        Task<ReturnModelDTO<List<CancellationPolicyDTO>>> GetCancellationPolicies(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<RentRuleDTO>>> GetRentRules(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<CurrencyDTO>>> GetCurrencies();
        Task<ReturnModelDTO<List<SecurityItemDTO>>> GetSecurityItems(Dictionary<string, string> requestInfo, bool v);
        Task<ReturnModelDTO<List<ApprovalPolicyDTO>>> GetReservationTypes(Dictionary<string, string> requestInfo, bool v);
        Task<ReturnModelDTO<List<ReasonRefusedBookDTO>>> GetReasonRefusedBook(Dictionary<string, string> requestInfo, bool v);
        Task<ReturnModelDTO<List<BookStatusDTO>>> GetBookStatuses(Dictionary<string, string> requestInfo, bool v);
        Task<ReturnModelDTO<List<AddionalFeeTypeDTO>>> GetAdditionalFeeTypes();
        
        Task<ReturnModelDTO<List<BookCancellationReasonDTO>>> GetCancellationReason(
            string cancellationTypeCode, int cancellationReasonOwnerId);
        Task<ReturnModelDTO<List<AppComplaintReasonHierarchyDto>>> GetComplaintReasons();
    }
}
