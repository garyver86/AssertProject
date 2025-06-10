using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Domain.Services;

public interface IAppUserService
{

    Task<ReturnModelDTO> LoginAndEnrollment(string platform, string token, string user, string password);
    Task<ReturnModelDTO> EnableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
    Task<ReturnModelDTO> DisableHostRole(long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
    Task<ReturnModelDTO> LocalUserEnrollment(LocalUserRequest userRequest);
    Task<ReturnModelDTO> UpdatePersonalInformation(UpdatePersonalInformationRequest request);

}
