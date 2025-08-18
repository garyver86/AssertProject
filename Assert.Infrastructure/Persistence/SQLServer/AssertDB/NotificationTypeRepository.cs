using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        public Task<TnNotificationType> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
