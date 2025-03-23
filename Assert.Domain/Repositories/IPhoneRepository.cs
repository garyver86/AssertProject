using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IPhoneRepository
    {
        Task<List<TuPhone>> GetByUser(int ownerUserId);
    }
}
