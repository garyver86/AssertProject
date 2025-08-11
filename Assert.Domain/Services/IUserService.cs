using Assert.Domain.Models;
using Assert.Domain.Models.Profile;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Services
{
    public interface IUserService
    {
        Task<ReturnModel> EnableHostRole(long userId);
        Task<ReturnModel> DisableHostRole(long userId);
        Task<ReturnModel> BlockAsHost(int userId, int userBlockedId);
        Task<ReturnModel> UnblockAsHost(int userId, int userBlockedId);
        Task<ReturnModel<(List<Profile>, PaginationMetadata)>> SearchHosts(SearchFilters filters, int pageNumber, int pageSize);
    }
}
