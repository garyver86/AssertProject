using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IStepsTypeRepository
    {
        Task<TlStepsType> Get(int id);
        Task<TlStepsType> Get(string code);
        Task<List<TlStepsType>> GetAll();
        Task<List<TlStepsType>> GetAllActives();
    }
}
