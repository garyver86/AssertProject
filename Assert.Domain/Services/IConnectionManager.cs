using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IConnectionManager
    {
        void AddConnection(string userId, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string userId);
        string GetUserId(string connectionId);
        int GetConnectedUsersCount();
    }
}
