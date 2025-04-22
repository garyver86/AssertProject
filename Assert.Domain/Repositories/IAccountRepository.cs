using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories;

public interface IAccountRepository
{
    Task<ReturnModel<TuAccount>> Get(int accountId);
    Task<ReturnModel<TuAccount>> GetByUserId(int userId);
    Task<int> Create(int userId, string password);
    Task UpdateLastSessionInfo();
}
