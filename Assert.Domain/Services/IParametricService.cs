using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IParametricService
    {
        Task<ReturnModel<List<TlAccommodationType>>> GetAccomodationTypesActives();
        Task<ReturnModel<List<TDiscountTypeForTypePrice>>> GetDiscountTypes();
        Task<ReturnModel<List<TFeaturedAspectType>>> GetFeaturedAspects();
    }
}
