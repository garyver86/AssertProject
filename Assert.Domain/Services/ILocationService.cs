using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface ILocationService
    {
        Task<GroupedLocationResponse> SearchAndGroupLocations(string filter, int filterType);

    }
}
