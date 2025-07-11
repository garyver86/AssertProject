using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ISecurityItemsRepository
    {
        Task<List<TpSecurityItemType>> GetActives();
    }
}
