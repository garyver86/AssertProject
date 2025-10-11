using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookCancellationRepository
{
    Task<string> UpsertHostBookCancellation(int bookCancellationId,
        int bookId, int cancellationReasonId, string messageTo, string message);

    Task<List<TbBookCancellation>> GetHostBookCancellations(long bookId);
}
