using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Models;
using Assert.Domain.Models.Profile;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ReturnModel<TuUser>> Get(int ownerUserId);
        Task<ReturnModel> Login(string username, string password);

        Task<string> ChangeUserStatusAsync(int userId, string statusCode);
        Task<ReturnModel> ValidateUserName(string userName, 
            bool validateStatusActive, Platform platform);
        Task<int> Create(string userName, Platform platform, string name,
                string lastName, int genderTypeId, DateTime? dateOfBirth, string photoLink,
                int accountTypeId, string socialId, int? timeZoneId, string phoneNumber);
        Task<int> Update(TuUser user);
        Task<ReturnModel<List<TuUserRole>>> GetRoles(int userId, 
            bool getOnlyActives);
        Task<ReturnModel<bool>> ExistLocalUser(string userName);

        Task<string> UpdatePersonalInformation(int userId,
            string name, string lastName, string favoriteName,
            string email, string phone);

        Task<TuUser> GetPersonalInformationById(int userId);

        Task<Profile> GetAllProfile();

        Task<TuUser> GetAdditionalProfile();
        Task EnableHostRol(int userId);

        Task<int> UpsertAdditionalProfile(int objectId, string whatIDo,
            string wantedToGo, string pets, DateTime? birthday, List<TLanguage>? languages,
            string introduceYourself, int cityId, string cityName, string location);

        Task<string> UpdateProfilePhoto(string photoLink);
        Task<ReturnModel> BlockAsHost(int userId, int id);
        Task<ReturnModel> UnblockAsHost(int userId, int id);
        Task<ReturnModel<(List<Profile>, PaginationMetadata)>> SearchHostAsync(SearchFilters filters, int pageNumber, int pageSize);
        Task<ReturnModel<(List<TuUser>, PaginationMetadata)>> GetUserByRoleCodeAsync(
            SearchFiltersToUser filters, string roleCode, int pageNumber, int pageSize);

        Task<List<TuUserSelectionOption>> GetUserSelectionOptionsAsync();
        Task<string> UpsertUserAccountClosed(int userAccountClosedId,
            int userSelectionOptionsId);
        Task<string> UpsertUserAccountRestore(int userAccountClosedId);
        Task<TuUserAccountClosed> GetUserAccountClosedAsync();
    }
}
