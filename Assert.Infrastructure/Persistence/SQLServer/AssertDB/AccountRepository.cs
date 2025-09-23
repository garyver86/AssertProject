using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Models;
using Assert.Domain.Notifications;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class AccountRepository
    (IExceptionLoggerService _exceptionLoggerService, 
    IEmailNotification _emailNotifications,
    InfraAssertDbContext _dbContext,
    RequestMetadata _metadata)
    : IAccountRepository
{
    public async Task<string> GenerateOtpToCreateUser(string email, 
        string name, string lastName)
    {
        try 
        {
            #region otp code process
            string newOtp = UtilsMgr.GetOTPCode();
            string body = UtilsMgr.LoadOtpTemplate(
                $"{name}", newOtp);
            await _emailNotifications.SendAsync(new EmailNotification
            {
                Body = body,
                Subject = "Codigo OTP - Recuperacion de contrasena",
                To = new List<string> { email }
            });

            await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE assertdb.dbo.TU_UserOtpCreate
                    SET assertdb.dbo.TU_UserOtpCreate.Status = 'IN'
                    WHERE assertdb.dbo.TU_UserOtpCreate.Email = {email} 
                    AND assertdb.dbo.TU_UserOtpCreate.Status = 'AC'");

            var newUserOtpCreate = new TuUserOtpCreate
            {
                Email = email,
                Name = name,
                LastName = lastName,
                OtpCode = newOtp,
                Status = "AC"
            };
            await _dbContext.TuUserOtpCreates.AddAsync(newUserOtpCreate);
            await _dbContext.SaveChangesAsync();
            #endregion

            return "OTP_SENT";
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<string> ValidateOtpToCreateUser(
        string email, string otpCode)
    {
        try
        {
            var latestOtpForCreateUser = await _dbContext.TuUserOtpCreates
                    .Where(x => x.Email == email && x.Status == "AC")
                    .FirstOrDefaultAsync();

            if (latestOtpForCreateUser is null)
                throw new NotFoundException($"No existe codigo OTP generado para crear nuevo usuaerio: {email}");

            if (latestOtpForCreateUser.OtpCode != otpCode)
                throw new UnauthorizedException("El codigo OTP no es correcto para creacion de usuario. Verifique e intente nuevamente.");

            latestOtpForCreateUser.Status = "IN";
            await _dbContext.SaveChangesAsync();

            return "VALIDATED";
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<string> ForgotPassword(string userName, string otpCode, string newPassword)
    {
        try
        {
            var userTypeLocal = await _dbContext.TuPlatforms
            .FirstOrDefaultAsync(x => x.Code!.ToLower() == "local");

            if (userTypeLocal is null) throw new NotFoundException("No existe tipo de cuenta LOCAL");

            var currentUser = await _dbContext.TuUsers
                .Include(u => u.TuAccounts)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            if (currentUser is null) throw new NotFoundException("El usuario no existe en registro de la plataforma");

            if (currentUser.PlatformId != userTypeLocal.PlatformId)
                throw new UnauthorizedAccessException("El usuario no puede recuperar contrasena. Asocio cuenta desde una plataforma externa.");

            var currentAccount = currentUser.TuAccounts.FirstOrDefault();

            if (currentAccount is null) throw new NotFoundException("No existe usuario con sesion iniciada");
            if (currentUser.Status != "AC") throw new UnauthorizedException("El usuaerio no esta autorizado, tiene la cuenta desactivada");
            if(currentAccount.IsBlocked ?? false) throw new UnauthorizedException("El usuario no esta autorizado, tiene la cuenta bloqueada");

            if (string.IsNullOrWhiteSpace(otpCode))
            {
                #region otp code process
                string newOtp = UtilsMgr.GetOTPCode();
                string body = UtilsMgr.LoadOtpTemplate(
                    $"{currentUser.Name}", newOtp);
                await _emailNotifications.SendAsync(new EmailNotification
                {
                    Body = body,
                    Subject = "Codigo OTP - Recuperacion de contrasena",
                    To = new List<string> { currentUser.UserName! }
                });

                await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE assertdb.dbo.TU_UserOtp
                    SET assertdb.dbo.TU_UserOtp.Status = 'IN'
                    WHERE assertdb.dbo.TU_UserOtp.UserId = {_metadata.UserId} 
                    AND assertdb.dbo.TU_UserOtp.Status = 'AC'");

                var newUserOtp = new TuUserOtp
                {
                    UserId = currentUser.UserId,
                    OtpCode = newOtp,
                    DateLastGen = DateTime.Now,
                    Status = "AC",
                    UseOldPassword = "",
                    UseNewPassword = ""
                };
                await _dbContext.TuUserOtps.AddAsync(newUserOtp);
                await _dbContext.SaveChangesAsync();
                #endregion

                return "OTP_SENT";
            }
            else //with otpCode
            {
                var latestOtpForUser = await _dbContext.TuUserOtps
                    .Where(x => x.UserId == currentUser.UserId && x.Status == "AC")
                    .OrderByDescending(x => x.DateLastGen)
                    .FirstOrDefaultAsync();

                if (latestOtpForUser is null)
                    throw new NotFoundException("No existe codigo OTP generado para el cambio de contrasena");

                if (latestOtpForUser.OtpCode != otpCode)
                    throw new UnauthorizedException("El codigo OTP no es correcto para modificacion de contrasena. Verifique e intente nuevamente");

                var newPwd = UtilsMgr.GetHash512(newPassword);
                currentAccount.Password = newPwd;

                latestOtpForUser.UseNewPassword = newPwd;
                latestOtpForUser.Status = "IN";

                await _dbContext.SaveChangesAsync();

                return "UPDATED";
            }
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<string> ChangePassword(string oldPassword, string newPassword, string otpCode)
    {
        try
        {
            var userTypeLocal = await _dbContext.TuPlatforms
            .FirstOrDefaultAsync(x => x.Code!.ToLower() == "local");

            if (userTypeLocal is null) throw new NotFoundException("No existe tipo de cuenta LOCAL");

            var currentUser = await _dbContext.TuUsers
                .Include(u => u.TuAccounts)
                .FirstOrDefaultAsync(x => x.UserId == _metadata.UserId);

            if (currentUser is null) throw new NotFoundException("El usuario no existe en registro de la plataforma");

            if (currentUser.PlatformId != userTypeLocal.PlatformId)
                throw new UnauthorizedAccessException("El usuario no puede modificar su contrasena. Asocio cuenta desde una plataforma externa.");

            var currentAccount = currentUser.TuAccounts.FirstOrDefault();

            if(currentAccount is null) throw new NotFoundException("No existe usuario con sesion iniciada");
            if (currentUser.Status != "AC") throw new UnauthorizedException("El usuaerio no esta autorizado, tiene la cuenta desactivada");

            if (string.IsNullOrWhiteSpace(otpCode))
            {
                string oldPwd = UtilsMgr.GetHash512(oldPassword);
                if (currentAccount.Password != oldPwd)
                    throw new UnauthorizedException("La contrasena actual no coincide. Verifique e intente nuevamente");

                #region otp code process
                string newOtp = UtilsMgr.GetOTPCode();
                string body = UtilsMgr.LoadOtpTemplate(
                    $"{currentUser.Name}", newOtp);
                await _emailNotifications.SendAsync(new EmailNotification
                {
                    Body = body,
                    Subject = "Codigo OTP - Cambio de contrasena",
                    To = new List<string> { currentUser.UserName! }
                });

                await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE assertdb.dbo.TU_UserOtp
                    SET assertdb.dbo.TU_UserOtp.Status = 'IN'
                    WHERE assertdb.dbo.TU_UserOtp.UserId = {_metadata.UserId} 
                    AND assertdb.dbo.TU_UserOtp.Status = 'AC'");

                var newUserOtp = new TuUserOtp
                {
                    UserId = _metadata.UserId,
                    OtpCode = newOtp,
                    DateLastGen = DateTime.Now,
                    Status = "AC",
                    UseOldPassword = oldPwd,
                    UseNewPassword = ""
                };
                await _dbContext.TuUserOtps.AddAsync(newUserOtp);
                await _dbContext.SaveChangesAsync();
                #endregion

                return "OTP_SENT";
            }
            else //with otp
            {
                var latestOtpForUser = await _dbContext.TuUserOtps
                    .Where(x => x.UserId == _metadata.UserId && x.Status == "AC")
                    .OrderByDescending(x => x.DateLastGen)
                    .FirstOrDefaultAsync();

                if(latestOtpForUser is null)
                    throw new NotFoundException("No existe codigo OTP generado para el cambio de contrasena");

                if(latestOtpForUser.OtpCode != otpCode)
                    throw new UnauthorizedException("El codigo OTP no es correcto para modificacion de contrasena. Verifique e intente nuevamente");

                var newPwd = UtilsMgr.GetHash512(newPassword);
                currentAccount.Password = newPwd;

                latestOtpForUser.UseNewPassword = newPwd;
                latestOtpForUser.Status = "IN";

                await _dbContext.SaveChangesAsync();

                return "UPDATED";
            }
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<int> Create(int userId, string password)
    {
        string pass = UtilsMgr.GetHash512(password);
        TuAccount account = new TuAccount()
        {
            UserId = userId,
            Password = pass,
            IncorrectAccess = 0,
            IsBlocked = false,
            LastBlockDate = null,
            DateLastLogin = null,
            IpLastLogin = null,
            Status = "AC",
            TemporaryBlockTo = null,
            ForceChange = false
        };

        try
        {
            await _dbContext.TuAccounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
            return Convert.ToInt32(account.AccountId);
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { account });
            throw new InfrastructureException(ex.Message);
        }
    }

    public Task<ReturnModel<TuAccount>> Get(int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnModel<TuAccount>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateLastSessionInfo()
    {
        try
        {
            await _dbContext.TuAccounts
                .Where(a => a.AccountId == _metadata.AccountId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.DateLastLogin, DateTime.UtcNow)
                    .SetProperty(a => a.IpLastLogin, _metadata.IpAddress));
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className,
                new { _metadata });
            throw new InfrastructureException(ex.Message);
        }
    }
}
