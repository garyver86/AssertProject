using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookStatusRepository
{
    Task<List<TbBookStatus>> Get();
    Task<TbBookStatus> GetStatusByCode(string statusCode);
}
