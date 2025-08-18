using Assert.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class ConnectionManager : IConnectionManager
    {
        private static readonly ConcurrentDictionary<string, List<string>> _connections =
        new ConcurrentDictionary<string, List<string>>();

        public void AddConnection(string userId, string connectionId)
        {
            _connections.AddOrUpdate(userId,
                new List<string> { connectionId },
                (key, existingList) =>
                {
                    if (!existingList.Contains(connectionId))
                        existingList.Add(connectionId);
                    return existingList;
                });
        }

        public void RemoveConnection(string userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (!connections.Any())
                    _connections.TryRemove(userId, out _);
            }
        }

        public List<string> GetConnections(string userId)
        {
            return _connections.TryGetValue(userId, out var connections) ?
                connections :new List<string>();
        }
    }
}
