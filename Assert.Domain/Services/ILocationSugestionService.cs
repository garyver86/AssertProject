using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface ILocationSugestionService
    {
        Task<ReturnModel<List<LocationSuggestion>>> GetLocationSuggestions(string filter);
    }
}
