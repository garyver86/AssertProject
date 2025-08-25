using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Events
{
    public class UserConnectedEvent : DomainEvent
    {
        public string ConnectionId { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UserConnectedEvent(string connectionId, string userId, string userName)
        {
            ConnectionId = connectionId;
            UserId = userId;
            UserName = userName;
        }
    }
    public class UserDisconnectedEvent : DomainEvent
    {
        public string ConnectionId { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UserDisconnectedEvent(string connectionId, string userId, string userName)
        {
            ConnectionId = connectionId;
            UserId = userId;
            UserName = userName;
        }
    }
}
