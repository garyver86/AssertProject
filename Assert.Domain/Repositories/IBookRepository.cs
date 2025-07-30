using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookRepository
{
    Task<List<TbBook>> GetByUserId(long userId);
    Task<TbBook> GetByIdAsync(long bookId);
    Task<long> UpsertBookAsync(TbBook book);
    Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId);
}
