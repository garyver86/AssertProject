using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private readonly InfraAssertDbContext _context;

        public NotificationTypeRepository(InfraAssertDbContext context)
        {
            _context = context;
        }
        public async Task<TnNotificationType> GetByNameAsync(string name)
        {
            return await _context.TnNotificationTypes
              .FirstOrDefaultAsync(n => n.Nname == name);
        }
    }
}
