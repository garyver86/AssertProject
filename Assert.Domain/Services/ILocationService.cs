using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface ILocationService
    {
        Task<LocationModel?> ResolveLocation(string? country, string? state, string? county, string? city, string? street);
        Task<GroupedLocationResponse> SearchAndGroupLocations(string filter, int filterType);

    }
}
