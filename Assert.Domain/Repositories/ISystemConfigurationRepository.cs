namespace Assert.Domain.Repositories
{
    public interface ISystemConfigurationRepository
    {
        Task<string> GetListingResourcePath();
        Task<string> GetListingResourceUrl();
    }
}
