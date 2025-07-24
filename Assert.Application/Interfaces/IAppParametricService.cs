using Assert.Application.DTOs.Responses;

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


    }
}
