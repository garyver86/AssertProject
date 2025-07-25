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
    public class PayTransactionRepository(
        InfraAssertDbContext _context
        , IServiceProvider serviceProvider) : IPayTransactionRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<long> Create(PayTransaction transaction)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                context.PayTransactions.Add(transaction);
                await context.SaveChangesAsync();
                return transaction.PayTransactionId;
            }
        }
    }
}
