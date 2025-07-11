using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IApprovalPolityTypeRepository
    {
        Task<List<TApprovalPolicyType>> GetActives();
    }
}
