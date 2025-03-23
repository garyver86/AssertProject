using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class UserRepository : IUserRepository
    {
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly InfraAssertDbContext _context;
        public UserRepository(IExceptionLogRepository exceptionLogRepository, InfraAssertDbContext infraAssertDbContext)
        {
            _exceptionLogRepository = exceptionLogRepository;
            _context = infraAssertDbContext;
        }
        public async Task<ReturnModel> Login(string username, string password, string ip, string browseInfo)
        {
            try
            {
                string pass = UtilsMgr.GetHash512(password);

                string query = $"EXECUTE dbo.SPA_Loguin @username, @password, @ip";

                var param = new List<SqlParameter>
                    {
                        new SqlParameter("@username", username),
                        new SqlParameter("@password", pass),
                        new SqlParameter("@ip", ip)
                    };

                var result_login = _context.Database.SqlQueryRaw<LoginModelResult>(
                    query, param.ToArray()).ToList().FirstOrDefault();
                if (result_login.CodeResult == 1)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.OK,
                        Data = result_login.MessageResult,
                        ResultError = new ErrorCommon
                        {
                            Message = result_login.Identifier.ToString(),
                            //Code = UserRole.Admin
                            Code = result_login.Roles
                        }
                    };
                }
                else
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.Unauthorized,
                        ResultError = new ErrorCommon { Message = result_login.MessageResult }
                    };
                }
            }
            catch (Exception ex)
            {
                _exceptionLogRepository.SaveException(ex, "Login", "Infraestructure.UserRepository", new { username }, null, ip, browseInfo);
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.InternalError,
                    ResultError = new ErrorCommon { Message = ex.Message }
                };
            }
        }


        public ReturnModel ChangeStatusUser(int userId, int id, string status)
        {
            TuUser? user = _context.TuUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (user != null)
            {
                user.Status = status == "AC" ? "IN" : "AC";

                _context.SaveChanges();
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
        public ReturnModel ValidateUserName(string userName, bool validateStatusActive)
        {
            TuUser user = _context.TuUsers.Where(x => x.UserName.ToUpper() == userName.ToUpper()).ToList().LastOrDefault();
            if (user != null)
            {
                if (validateStatusActive)
                {
                    var account = user.TuAccounts.ToList().Last();
                    if (account.Status == "AC")
                    {
                        if (!(account.IsBlocked ?? false))
                        {
                            return new ReturnModel
                            {
                                Data = user,
                                StatusCode = ResultStatusCode.OK
                            };
                        }
                        else
                        {
                            return new ReturnModel
                            {
                                StatusCode = ResultStatusCode.Unauthorized,
                                ResultError = new ErrorCommon { Message = "El usuario se encuentra bloqueado." }
                            };
                        }
                    }
                    else
                    {
                        return new ReturnModel
                        {
                            StatusCode = ResultStatusCode.Unauthorized,
                            ResultError = new ErrorCommon { Message = "El usuario no se encuentra activo." }
                        };
                    }
                }
                return new ReturnModel
                {
                    Data = user,
                    StatusCode = ResultStatusCode.OK
                };
            }
            else
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon { Message = "El usuario no fue encontrado." }
                };
            }
        }

        public async Task<ReturnModel<TuUser>> Get(int id)
        {
            TuUser user = _context.TuUsers.Where(x => x.UserId == id).ToList().LastOrDefault();
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

            users = _context.TuUsers.ToList();
            return users;
        }

        public async Task<TuUser> GetById(int id)
        {
            TuUser? user = _context.TuUsers.FirstOrDefault(item => item.UserId == id);
            return user;
        }

        public async Task<int> Create(TuUser user)
        {
            await _context.TuUsers.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(TuUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<ReturnModel<List<TuUserRole>>> GetRoles(int userId, bool getOnlyActives)
        {
            List<TuUserRole> roles = null;
            List<int> roles_enableds = null;
            roles_enableds = _context.TuUserRoles.Where(x => x.UserId == userId && (!getOnlyActives || (x.IsActive ?? false))).Select(x => x.UserRoleId).ToList();
            if (roles_enableds?.Count > 0)
            {
                roles = _context.TuUserRoles.Where(x => roles_enableds.Contains(x.UserRoleId) && (!getOnlyActives || (x.IsActive ?? false))).ToList();
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
