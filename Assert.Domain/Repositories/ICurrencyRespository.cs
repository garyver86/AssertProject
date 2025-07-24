using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface ICurrencyRespository
{
    Task<List<TCurrency>> GetAllAsync();
}
