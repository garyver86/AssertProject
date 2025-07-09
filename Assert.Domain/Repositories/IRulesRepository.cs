using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IRulesTypeRepository
    {
        Task<List<TpRuleType>> GetActives();
    }
}
