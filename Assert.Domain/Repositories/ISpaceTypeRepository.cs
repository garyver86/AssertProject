using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ISpaceTypeRepository
    {
        Task<List<TSpaceType>> Get();
    }
}
