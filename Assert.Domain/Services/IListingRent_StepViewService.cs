using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IListingRent_StepViewService
    {
        Task<ReturnModel<ListingProcessDataResultModel>> GetNextListingStepViewData(int? nextViewTypeId, TlListingRent? data, bool useTechnicalMessages);
        Task<ReturnModel> ProccessListingRentData(TlViewType viewType, TlListingRent result, int userId, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
