using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Services
{
    public interface IListingRentService
    {
        //Task<ReturnModel<TlListingRent>> ProccessListingRentViewData(long listingRentId, string currentViewCode, string nextViewCode, string nextStepView, int ownerUserId,
        //    List<KeyValuePair<string, object>> currentViewData, Dictionary<string, string> clientData, bool useTechnicalMessages);
        //Task<ReturnModel<ListingRentDetails>> ProccessListingRentViewData(long listingRentId, string currentViewCode, string nextViewCode,
        //    string nextStepView, int ownerUserId, Dictionary<string, object> currentViewData, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<string> GetListingData(int? ownerId, int listingRentId, bool UseTechnicalMessages);
        Task<string> GetViewsData(int ownerUserId, int listingRentId);
        Task<ReturnModel<List<TlListingRent>>> GetAllListingsRentsData(int ownerUserId, bool UseTechnicalMessages);
        Task<ReturnModel> ChangeStatus(long listingRentId, int ownerUserId, string newStatusCode, Dictionary<string, string> clientData,
            bool useTechnicalMessages);
        Task<string> GetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate);
        Task<string> SetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate, List<CalendarEvent> calendarEvents);
        Task<string> SetCalendarByDay(long listingRentId, int ownerUserId, string startDate, string endDate, List<YearCalendarEvent> Years);
        Task<string> GetOwnerCalendar(int ownerUserId, string startDate, string endDate);
        Task<string> GetPriceNightlySuggestion(int spaces, int year, int amenities, bool UseTechnicalMessages);
        Task<ReturnModel<ListingProcessDataResultModel>> ProcessData(long? listingRentId, string viewCode, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
