using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ReturnModel<TuUser>> Get(int ownerUserId);
        Task<ReturnModel> Login(string username, string password);
        Task<ReturnModel> ValidateUserName(string userName, bool validateStatusActive);
        Task<int> Create(string userName, Platform platform, string name,
                string lastName, int genderTypeId, DateTime? dateOfBirth, string photoLink,
                int accountTypeId, string socialId, int? timeZoneId);
    }
}
