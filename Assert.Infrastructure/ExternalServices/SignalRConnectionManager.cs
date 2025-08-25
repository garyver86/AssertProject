using Assert.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.ExternalServices
{
    public class SignalRConnectionManager : IConnectionManager
    {
        private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> _userConnections = new();
        private static readonly ConcurrentDictionary<string, string> _connectionUsers = new();

        public void AddConnection(string userId, string connectionId)
        {
            try
            {
                // Usar ConcurrentBag en lugar de HashSet para mejor manejo de concurrencia
                var connections = _userConnections.GetOrAdd(userId, key => new ConcurrentBag<string>());

                // ConcurrentBag es thread-safe por diseño, no necesita lock
                if (!connections.Contains(connectionId))
                {
                    connections.Add(connectionId);
                }

                _connectionUsers[connectionId] = userId;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RemoveConnection(string connectionId)
        {
            try
            {
                if (_connectionUsers.TryRemove(connectionId, out var userId))
                {
                    if (_userConnections.TryGetValue(userId, out var connections))
                    {
                        // Para ConcurrentBag, necesitamos recrear la lista sin la conexión
                        var newConnections = new ConcurrentBag<string>(
                            connections.Where(c => c != connectionId));

                        if (newConnections.IsEmpty)
                        {
                            _userConnections.TryRemove(userId, out _);
                        }
                        else
                        {
                            _userConnections[userId] = newConnections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public HashSet<string> GetConnections(string userId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                return new HashSet<string>(connections);
            }
            return new HashSet<string>();
        }

        public string GetUserId(string connectionId)
        {
            return _connectionUsers.TryGetValue(connectionId, out var userId) ? userId : null;
        }

        public int GetConnectedUsersCount()
        {
            return _userConnections.Count;
        }
    }
}
