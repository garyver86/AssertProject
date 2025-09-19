using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories;

public interface IAccountRepository
{
    Task<string> GenerateOtpToCreateUser(string email,
        string name, string lastName);
    Task<string> ValidateOtpToCreateUser(
        string email, string otpCode);
    Task<ReturnModel<TuAccount>> Get(int accountId);
    Task<ReturnModel<TuAccount>> GetByUserId(int userId);
    Task<int> Create(int userId, string password);
    Task UpdateLastSessionInfo();
    Task<string> ChangePassword(string oldPassword, string newPassword, string otpCode);
    Task<string> ForgotPassword(string userName, string otpCode, string newPassword);
}
