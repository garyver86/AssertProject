using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IViewTypeRepository
    {
        Task<TlViewType> Get(int? viewTypeId);
        Task<TlViewType> GetByCode(string? viewCode);
        Task<List<TlViewType>> GetByType(int stepsTypeId);
        Task<TlViewType> GetParent(int? viewTypeId);
        Task<TlViewType> GetPrevious(int? viewTypeId);
    }
}
