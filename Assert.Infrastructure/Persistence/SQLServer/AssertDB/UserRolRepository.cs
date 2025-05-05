using Assert.Domain.Common;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class UserRolRepository(IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext) 
    : IUserRolRepository
{
    public async Task<int> Create(int userId, int userTypeId)
    {
        var newUserRol = new TuUserRole
        {
            UserId = userId,
            UserTypeId = userTypeId,
            IsActive = true
        };

        try
        {
            await _dbContext.TuUserRoles.AddAsync(newUserRol);
            await _dbContext.SaveChangesAsync();
            return newUserRol.UserRoleId;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { newUserRol });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<int> CreateGuest(int userId)
    {
        var userTypeGuest = _dbContext.TuUserTypes.Where(x => x.Code == "GE").FirstOrDefault();
        if (userTypeGuest == null)
            throw new InfrastructureException("No existe tipo de usuario: GUEST");

        var userRolId = await Create(userId, userTypeGuest.UserTypeId);
        return userRolId;
    }

    public async Task<TuUserRole> Get(int userRolId)
    {
        try
        {
            var userRol = await _dbContext.TuUserRoles
                .Where(x => x.UserRoleId == userRolId)
                //.Include(x => x.UserType)
                .FirstOrDefaultAsync();
            if (userRol == null)
                throw new NotFoundException($"No existe UserRol: {userRolId}");

            return userRol;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, userRolId.ToString());
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<List<string>> GetUserRoles(int userId)
    {
        try
        {
            var userRolList = await _dbContext.TuUserRoles
                .Where(x => x.UserId == userId)
                .Include(x => x.UserType)
                .Select(x => x.UserType.Name)
                .ToListAsync();

            return userRolList;
        }
        catch(Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, userId.ToString());
            throw new InfrastructureException(ex.Message);
        }
    }
}
