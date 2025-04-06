using Assert.Application.DTOs;

namespace Assert.Application.Interfaces
{
    public interface IAppUserService
    {
        Task<ReturnModelDTO> EnableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO> DisableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
