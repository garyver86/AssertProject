using Assert.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Interfaces.Notifications
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent);
    }
}
