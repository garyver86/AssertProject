using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IQuickTipRepository
    {
        Task<List<TQuickTip>> GetByViewType(int viewTypeId, string urlResources);
    }
}
