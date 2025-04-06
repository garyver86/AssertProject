using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly InfraAssertDbContext _context;
        private readonly IUserTypeRepository _userTypeRepository;
        public UserRoleRepository(InfraAssertDbContext context, IExceptionLogRepository exceptionLogRepository, IUserTypeRepository userTypeRepository)
        {
            _context = context;
            _exceptionLogRepository = exceptionLogRepository;
            _userTypeRepository = userTypeRepository;
        }

        public async Task<bool> DisableHostPermission(long userId)
        {
            var permission = await _context.TuUserRoles
                .Where(x => x.UserId == userId && x.UserType.Code == "HO").FirstOrDefaultAsync();

            if (permission != null)
            {
                if (permission.IsActive == true)
                {
                    permission.IsActive = false;
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<bool> EnableHostPermission(long userId)
        {
            var permission = await _context.TuUserRoles
                 .Where(x => x.UserId == userId && x.UserType.Code == "HO").FirstOrDefaultAsync();

            if (permission != null)
            {
                if (permission.IsActive != true)
                {
                    permission.IsActive = true;
                }
            }
            else
            {
                TuUserType HostRole = await _userTypeRepository.GetByCode("HO");
                var newPermission = new TuUserRole
                {
                    UserId = (int)userId,
                    UserTypeId = HostRole.UserTypeId,
                    IsActive = true
                };
                _context.TuUserRoles.Add(newPermission);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}