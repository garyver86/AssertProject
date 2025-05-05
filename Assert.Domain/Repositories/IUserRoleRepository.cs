namespace Assert.Domain.Repositories
{
    public interface IUserRoleRepository
    {
        Task<bool> EnableHostPermission(long userId);
        Task<bool> DisableHostPermission(long userId);
    }
}
