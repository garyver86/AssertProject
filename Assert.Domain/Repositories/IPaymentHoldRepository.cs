using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IPaymentHoldRepository
    {
        Task<bool> HasActiveHoldAsync(long hostId);
        Task AddAsync(PayPaymentHold hold);
        Task ReleaseHoldAsync(long holdId);
        Task<IEnumerable<PayPaymentHold>> GetActiveHoldsAsync();
    }
}
