using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookRepository
{
    Task<List<TbBook>> GetByUserId(long userId);
    Task<TbBook> GetByIdAsync(long bookId);
    Task<int> UpsertBookAsync(TbBook book);
}
