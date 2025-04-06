using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IUserRoleRepository
    {
        Task<bool> EnableHostPermission(long userId);
        Task<bool> DisableHostPermission(long userId);
    }
}
