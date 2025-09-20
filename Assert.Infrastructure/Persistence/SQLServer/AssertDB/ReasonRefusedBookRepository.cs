using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ReasonRefusedBookRepository : IReasonRefuseBookRepository
    {
        private readonly InfraAssertDbContext _context;

        public ReasonRefusedBookRepository(InfraAssertDbContext context)
        {
            _context = context;
        }

        public async Task<List<TReasonRefusedBook>> GetActives()
        {
            return await _context.TReasonRefusedBooks
                .ToListAsync();
        }
    }
}
