using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IListingRent_StepViewService
    {
        Task<ReturnModel<ListingProcessDataResultModel>> GetNextListingStepViewData(int? nextViewTypeId, TlListingRent? data, bool useTechnicalMessages, bool getViewData);
        Task<ReturnModel> ProccessListingRentData(TlViewType viewType, TlListingRent result, int userId, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModel<ListingProcessDataResultModel>> GetAllListingStepViewData(int? nextViewTypeId, TlListingRent? data, bool useTechnicalMessages);
    }
}
