using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface INotificationDispatcher
    {
        Task SendNotificationAsync(int userId, TnNotification notification, string notificationType);
        Task UpdateUnreadCount(int userId, int count);
    }
}
