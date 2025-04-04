using Assert.Application.DTOs;

namespace Assert.Application.Interfaces
{
    public interface IAppParametricService
    {
        Task<ReturnModelDTO<List<AccomodationTypeDTO>>> GetAccomodationTypes(Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
