using Assert.Domain.Repositories;

namespace Assert.Application.Interfaces
{
    public interface IUrlBaseService
    {
        string BaseUrlListings { get; }
        string BaseUrlIcons { get; }
    }

    public class UrlBaseService : IUrlBaseService, IDisposable
    {
        private readonly ISystemConfigurationRepository _configRepo;
        private readonly string _baseUrlListings;
        private readonly string _baseUrlIcons;

        public UrlBaseService(ISystemConfigurationRepository configRepo)
        {
            _configRepo = configRepo;
            _baseUrlListings = _configRepo.GetListingResourceUrl().Result;
            _baseUrlIcons = _configRepo.GetIconsResourceUrl().Result;
        }

        public string BaseUrlIcons => _baseUrlIcons;
        public string BaseUrlListings => _baseUrlListings;

        public void Dispose()
        {
            // Limpieza si es necesaria
        }
    }
}
