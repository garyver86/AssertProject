using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IUserTypeRepository
    {
        Task<TuUserType> GetByCode(string v);
    }
}
