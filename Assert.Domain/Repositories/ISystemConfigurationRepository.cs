namespace Assert.Domain.Repositories
{
    public interface ISystemConfigurationRepository
    {
        Task<string> GetListingResourcePath();
        Task<string> GetListingResourceUrl();
        Task<string> GetIconsResourceUrl();
        Task<string> GetProfilePhotoResourcePath();
        Task<string> GetProfilePhotoResourceUrl();
    }
}
