using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IEmergencyContactRepository
{
    Task<TuEmergencyContact> GetByUserId(int userId);
    Task<string> Upsert(int emergencyContactId, int userId,
        string name, string lastName, string relationship,
        int languageId, string email, string phoneNumber);
}
