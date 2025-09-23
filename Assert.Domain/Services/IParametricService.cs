using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IParametricService
    {
        Task<ReturnModel<List<TlAccommodationType>>> GetAccomodationTypesActives();
        Task<ReturnModel<List<TpAmenitiesType>>> GetAmenityTypesActives();
        Task<ReturnModel<List<TDiscountTypeForTypePrice>>> GetDiscountTypes();
        Task<ReturnModel<List<TFeaturedAspectType>>> GetFeaturedAspects();
        Task<ReturnModel<List<TpPropertySubtype>>> GetPropertySubTypes(bool onlyActives);
        Task<ReturnModel<List<TSpaceType>>> GetSpaceTypes();
        Task<ReturnModel<List<TLanguage>>> GetLanguages();
        Task<ReturnModel<List<TCancelationPolicyType>>> GetCancellationPolicies(bool useTechnicalMessages);
        Task<ReturnModel<List<TpRuleType>>> GetRentRuleTypes(bool useTechnicalMessages);
        Task<ReturnModel<List<TpSecurityItemType>>> GetSecurityItems(bool useTechnicalMessages);
        Task<ReturnModel<List<TApprovalPolicyType>>> GetApprobalPolicyTypes(bool useTechnicalMessages);
        Task<ReturnModel<List<TReasonRefusedBook>>> GetReasonRefusedBook(bool useTechnicalMessages);
        Task<ReturnModel<List<TbBookStatus>>> GetBookStatuses(bool useTechnicalMessages);
    }
}
