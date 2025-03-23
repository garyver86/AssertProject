using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IQuickTypeViewRepository
    {
        Task<List<TlQuickTypeView>> GetByTypeId(int viewTypeId, bool v);
    }
}
