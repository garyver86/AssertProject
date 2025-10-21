using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IPayoutTransactionRepository
    {
        Task<PayPayoutTransaction> GetByIdAsync(long payoutId);
        Task<IEnumerable<PayPayoutTransaction>> GetDuePayoutsAsync(DateTime currentTime);
        Task<IEnumerable<PayPayoutTransaction>> GetFailedPayoutsAsync(DateTime since);
        Task<IEnumerable<PayPayoutTransaction>> GetPayoutsByHostAsync(long hostId);
        Task AddAsync(PayPayoutTransaction payout);
        Task UpdateAsync(PayPayoutTransaction payout);
        Task<bool> ExistsAsync(long payoutId);
    }
}
