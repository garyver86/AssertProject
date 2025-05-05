using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class UserRepository(IExceptionLoggerService _exceptionLoggerService,
        RequestMetadata _metadata, InfraAssertDbContext _dbContext) : IUserRepository
    {

        public async Task<ReturnModel> Login(string username, string password)
        {
            try
            {
                string pass = UtilsMgr.GetHash512(password);

                string query = $"EXECUTE dbo.SPA_Loguin @username, @password, @ip";

                var param = new List<SqlParameter>
                    {
                        new SqlParameter("@username", username),
                        new SqlParameter("@password", pass),
                        new SqlParameter("@ip", _metadata.IpAddress)
                    };

                var result_login = _dbContext.Database.SqlQueryRaw<LoginModelResult>(
                    query, param.ToArray()).ToList().FirstOrDefault();
                if (result_login.CodeResult == 1)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.OK,
                        //Data = result_login.MessageResult,
                        Data = new ProviderUser
                        {
                            UserId = result_login.Identifier.ToString(),
                            Email = result_login.userName,
                            Name = result_login.givenName,
                            LastName = result_login.lastName
                        },
                        ResultError = new ErrorCommon
                        {
                            Message = result_login.Identifier.ToString(),
                            Code = result_login.Roles
                        }
                    };
                }
                else
                    throw new UnauthorizedAccessException(result_login.MessageResult);
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { username });

                throw new InfrastructureException(ex.Message);
            }
        }

        public ReturnModel ChangeStatusUser(int userId, int id, string status)
        {
            TuUser? user = _dbContext.TuUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (user != null)
            {
                user.Status = status == "AC" ? "IN" : "AC";

                _dbContext.SaveChanges();
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK
                };

            }
            else
            {

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon
                    {
                        Message = "El registro no ha sido encontrado."
                    }
                };
            }
        }

        public async Task<ReturnModel> ValidateUserName(string userName, bool validateStatusActive)
        {
            try
            {
                var user = await _dbContext.TuUsers
                    .Where(x => x.UserName!.ToUpper() == userName.ToUpper())
                    .Include(x => x.TuAccounts)
                    .OrderByDescending(x => x.UserId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = new ErrorCommon { Message = "El usuario no existe en registros." }
                    };
                }

                if (!validateStatusActive)
                {
                    return new ReturnModel
                    {
                        Data = user,
                        StatusCode = ResultStatusCode.OK
                    };
                }

                var lastAccount = user.TuAccounts.OrderByDescending(a => a.AccountId).FirstOrDefault();
                if (lastAccount == null)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.NoContent,
                        ResultError = new ErrorCommon { Message = "No se encontró cuenta asociada al usuario." },
                        Data = user.UserId
                    };
                }

                if (lastAccount.Status != "AC")
                    throw new UnauthorizedException("El usuario no se encuentra activo.");

                if (lastAccount.IsBlocked == true)
                    throw new UnauthorizedException("El usuario se encuentra bloqueado.");

                return new ReturnModel
                {
                    Data = user,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<ReturnModel<TuUser>> Get(int id)
        {
            TuUser user = _dbContext.TuUsers.Where(x => x.UserId == id).ToList().LastOrDefault();
            if (user != null)
            {
                return await Task.Run(() => new ReturnModel<TuUser>
                {
                    Data = user,
                    StatusCode = ResultStatusCode.OK
                });
            }
            else
            {
                return new ReturnModel<TuUser>
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon { Message = "El usuario no fue encontrado." }
                };
            }
        }

        public async Task<List<TuUser>> GetAll()
        {
            List<TuUser> users;

            users = _dbContext.TuUsers.ToList();
            return users;
        }

        public async Task<TuUser> GetById(int id)
        {
            TuUser? user = _dbContext.TuUsers.FirstOrDefault(item => item.UserId == id);
            return user;
        }

        public async Task<int> Create(string userName, Platform platform,
            string name, string lastName, int genderTypeId,
            DateTime? dateOfBirth, string photoLink,
            int accountTypeId, string socialId, int? timeZoneId)
        {
            string plataformStr = platform.ToString().ToLower();
            var platformId = await _dbContext.TuPlatforms.Where(x => x.Code.ToLower() == plataformStr)
                .Select(x => x.PlatformId).FirstOrDefaultAsync();

            if (genderTypeId == 0)
                genderTypeId = await _dbContext.TuGenderTypes.Where(x => x.Code == "nr")
                    .Select(x => x.GenderTypeId).FirstOrDefaultAsync();

            var currentDate = DateTime.UtcNow;

            TuUser newUser = new TuUser
            {
                UserName = userName,
                PlatformId = platformId,
                Name = name,
                LastName = lastName,
                GenderTypeId = genderTypeId,
                DateOfBirth = dateOfBirth.HasValue ? DateOnly.FromDateTime(dateOfBirth.Value)
                    : null,
                TitleTypeId = null,
                UserStatusTypeId = 1,
                RegisterDate = currentDate,
                UpdateDate = currentDate,
                PhotoLink = photoLink,
                Status = "AC",
                AccountType = accountTypeId,
                SocialId = socialId,
                TimeZoneId = timeZoneId
            };

            try
            {
                await _dbContext.TuUsers.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
                return newUser.UserId;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { newUser });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<int> Update(TuUser user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<ReturnModel<List<TuUserRole>>> GetRoles(int userId, bool getOnlyActives)
        {
            List<TuUserRole> roles = null;
            List<int> roles_enableds = null;
            roles_enableds = _dbContext.TuUserRoles.Where(x => x.UserId == userId && (!getOnlyActives || (x.IsActive ?? false))).Select(x => x.UserRoleId).ToList();
            if (roles_enableds?.Count > 0)
            {
                roles = _dbContext.TuUserRoles.Where(x => roles_enableds.Contains(x.UserRoleId) && (!getOnlyActives || (x.IsActive ?? false))).ToList();
            }
            else
            {
                return new ReturnModel<List<TuUserRole>> { StatusCode = ResultStatusCode.NotFound };
            }
            if (roles?.Count > 0)
            {
                return new ReturnModel<List<TuUserRole>> { Data = roles, StatusCode = ResultStatusCode.OK };
            }
            else
            {
                return new ReturnModel<List<TuUserRole>> { StatusCode = ResultStatusCode.NotFound };
            }
        }
    }
}
