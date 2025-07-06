using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories;

public interface IAccountRepository
{
    Task<ReturnModel<TuAccount>> Get(int accountId);
    Task<ReturnModel<TuAccount>> GetByUserId(int userId);
    Task<int> Create(int userId, string password);
    Task UpdateLastSessionInfo();
    Task<string> ChangePassword(string newPassword);
}
