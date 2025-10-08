using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IComplaintReasonRepository
    {
        Task<List<TComplaintReason>> GetAll();
    }
}
