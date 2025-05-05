using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories;

public interface IUserRolRepository
{
    Task<int> CreateGuest(int userId);
    Task<int> Create(int userId, int userTypeId);
    Task<TuUserRole> Get(int userRolId);
    Task<List<string>> GetUserRoles(int userId);
}
