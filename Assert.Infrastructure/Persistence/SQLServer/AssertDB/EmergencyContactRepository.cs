using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class EmergencyContactRepository(
    IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext, ILogger<UserRepository> _logger) 
        : IEmergencyContactRepository
{
    public async Task<TuEmergencyContact> GetByUserId(int userId)
    {
        try
        {
            var emergencyContact = await _dbContext.TuEmergencyContacts
                .FirstOrDefaultAsync(ec => ec.UserId == userId);

            if (emergencyContact == null)
                throw new NotFoundException($"No existe registros de contacto de emergencia para el usuario: {userId}");

            return emergencyContact;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId });

            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<string> Upsert(int emergencyContactId, int userId, 
        string name, string lastName, string relationship, 
        int languageId, string email, string phoneNumber)
    {
        try
        {
            if(emergencyContactId > 0) // update
            {
                var emergencyContact = _dbContext.TuEmergencyContacts
                    .FirstOrDefault(ec => ec.EmergencyContactId == emergencyContactId);

                if (emergencyContact == null)
                    throw new NotFoundException($"No existe un contacto de emergencia con ID: {emergencyContactId} para el usuario: {userId}");

                if(!string.IsNullOrEmpty(name)) emergencyContact.Name = name;
                if (!string.IsNullOrEmpty(lastName)) emergencyContact.LstName = lastName;
                if (!string.IsNullOrEmpty(relationship)) emergencyContact.Relationship = relationship;
                if (languageId > 0) emergencyContact.LanguageId = languageId;
                if (!string.IsNullOrEmpty(email)) emergencyContact.Email = email;
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var phoneParts = phoneNumber.SplitCountryCode();
                    emergencyContact.PhoneCode = phoneParts.CountryCode.Replace("+", "");
                    emergencyContact.PhoneNumber = phoneParts.PhoneNumber;
                }
            }
            else // create
            {
                var newEmeregncyContact = new TuEmergencyContact
                {
                    UserId = userId,
                    Name = name ?? "",
                    LstName = lastName ?? "",
                    Relationship = relationship ?? "",
                    LanguageId = languageId,
                    Email = email ?? ""
                };

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var phoneParts = phoneNumber.SplitCountryCode();
                    newEmeregncyContact.PhoneCode = phoneParts.CountryCode.Replace("+", "");
                    newEmeregncyContact.PhoneNumber = phoneParts.PhoneNumber;
                }

                await _dbContext.TuEmergencyContacts.AddAsync(newEmeregncyContact);
            }

            await _dbContext.SaveChangesAsync();
            return "SUCCESS";
        }
        catch(Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId });

            _logger.LogError(ex, $"{ex.Message}");
            throw new InfrastructureException(ex.Message);
        }
    }
}
