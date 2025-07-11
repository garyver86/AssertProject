using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICancelationPoliciesTypesRepository
    {
        Task<List<TCancelationPolicyType>> GetActives();
    }
}
