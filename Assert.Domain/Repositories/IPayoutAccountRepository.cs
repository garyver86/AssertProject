using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IPayoutAccountRepository
    {
        Task<PayPayoutAccount> GetDefaultByHostAsync(int hostId);
        Task<IEnumerable<PayPayoutAccount>> GetByHostAsync(int hostId);
        Task AddAsync(PayPayoutAccount account);
        Task UpdateAsync(PayPayoutAccount account);
        Task<bool> VerifyMethodAsync(long accountId);
    }
}
