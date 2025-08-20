using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface ISocketIoServer
    {
        Task StartAsync();
        Task EmitTo(string connectionId, string eventName, object data);
        Task EmitAsync(string eventName, object data);
        ISocketIoRoom To(string roomId);
        void OnConnection(Action<ISocketIoSocket> handler);
        int GetActiveConnectionsCount();
        int GetRoomConnectionsCount(string roomId);
        DateTime GetLastActivityTime();
    }
    public interface ISocketIoSocket : IDisposable
    {
        string Id { get; }
        DateTime LastActivityTime { get; }
        WebSocketState State { get; }
        Task StartReceivingAsync();
        Task EmitAsync(string eventName, object data);

        void On(string eventName, Action<object> handler);
        void OnDisconnect(Action handler);

        void Join(string roomId);
        void Leave(string roomId);
        TimeSpan GetInactiveTime();

        bool IsInRoom(string roomId);

        Task CloseAsync(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure,
                       string statusDescription = "Closed by server");

    }

    public interface ISocketIoRoom
    {
        Task EmitAsync(string eventName, object data);
    }
}
