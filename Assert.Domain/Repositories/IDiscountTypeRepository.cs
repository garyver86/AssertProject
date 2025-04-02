using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IDiscountTypeRepository
    {
        Task<List<TDiscountTypeForTypePrice>> GetActives();
    }
}
