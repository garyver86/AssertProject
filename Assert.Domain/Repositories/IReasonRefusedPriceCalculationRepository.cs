using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IReasonRefusedPriceCalculationRepository
    {
        Task<List<TReasonRefusedPriceCalculation>> GetActives();
    }
}
