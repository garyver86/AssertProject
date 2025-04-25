using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces
{
    public interface IAppParametricService
    {
        Task<ReturnModelDTO<List<AccomodationTypeDTO>>> GetAccomodationTypes(Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<DiscountDTO>>> GetDiscountTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<FeaturedAspectDTO>>> GetFeaturedAspects(Dictionary<string, string> requestInfo, bool v);
    }
}
