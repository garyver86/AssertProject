using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IPropertySubTypeRepository
    {
        Task<TpPropertySubtype> Get(int? subtypeId);
        Task<TpPropertySubtype> GetActive(int? subtypeId);
    }
}
