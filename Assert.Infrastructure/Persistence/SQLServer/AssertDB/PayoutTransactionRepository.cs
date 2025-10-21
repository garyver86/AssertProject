using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PayoutTransactionRepository(
        InfraAssertDbContext _context,
        IServiceProvider serviceProvider) : IPayoutTransactionRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<IEnumerable<PayPayoutTransaction>> GetDuePayoutsAsync(DateTime currentTime)
        {
            return await _context.PayPayoutTransactions
                .Include(p => p.PriceCalculation)
                .Include(p => p.HostId)
                .Include(p => p.PayoutAccount)
                .Where(p => p.PayoutStatus == "scheduled" &&
                           p.ScheduledPayoutDate <= currentTime &&
                           !_context.PayPaymentHolds.Any(h => h.HostId == p.HostId && h.PaymentHoldStatus == "active"))
                .OrderBy(p => p.ScheduledPayoutDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayPayoutTransaction>> GetFailedPayoutsAsync(DateTime since)
        {
            return await _context.PayPayoutTransactions
                .Where(p => p.PayoutStatus == "failed" &&
                           p.PayoutDate >= since &&
                           p.AttemptCount < 3) // Retry limit
                .ToListAsync();
        }

        public async Task AddAsync(PayPayoutTransaction payout)
        {
            using (var _context = new InfraAssertDbContext(dbOptions))
            {
                await _context.PayPayoutTransactions.AddAsync(payout);
                await _context.SaveChangesAsync();
            }
        }

        public Task<PayPayoutTransaction> GetByIdAsync(long payoutId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayPayoutTransaction>> GetPayoutsByHostAsync(long hostId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PayPayoutTransaction payout)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(long payoutId)
        {
            throw new NotImplementedException();
        }
    }
}
