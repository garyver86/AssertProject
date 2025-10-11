using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookCancellationReasonRepository
{
    Task<List<TbBookCancellationReason>> GetFisrtStep(string cancellationTypeCode);
    Task<List<TbBookCancellationReason>> GetNextStep(int cancellationReasonParentId);

    Task<List<TbBookCancellationReason>> GetParents(int cancellationReasonId);
}
