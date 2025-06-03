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
    }
}
