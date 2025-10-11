using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IComplaintReasonRepository
    {
        Task<List<TComplaintReason>> GetAll();
        Task<List<ComplaintReasonHierarchyDto>> GetComplaintReasonsHierarchyAsync(int? parentId = null, bool includeInactive = false);
    }
}
