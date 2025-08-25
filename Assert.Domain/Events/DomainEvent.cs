using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Events
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
        public Guid EventId { get; protected set; } = Guid.NewGuid();
    }
}
