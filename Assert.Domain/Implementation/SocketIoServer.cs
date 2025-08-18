using Assert.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class SocketIoServer : ISocketIoServer
    {
        private readonly int _port;
        private readonly ConcurrentDictionary<string, ISocketIoSocket> _sockets = new();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ISocketIoSocket>> _rooms = new();
        private Action<ISocketIoSocket> _connectionHandler;

        public SocketIoServer(int port)
        {
            _port = port;
        }

        private DateTime _lastServerActivity = DateTime.UtcNow;

        public DateTime GetLastActivityTime()
        {
            return _sockets.IsEmpty
                ? _lastServerActivity
                : _sockets.Values.Max(s => s.LastActivityTime);
        }

        public DateTime GetConnectionLastActivity(string connectionId)
        {
            return _sockets.TryGetValue(connectionId, out var socket)
                ? socket.LastActivityTime
                : DateTime.MinValue;
        }

        internal void UpdateServerActivity()
        {
            _lastServerActivity = DateTime.UtcNow;
        }

        public async Task EmitTo(string connectionId, string eventName, object data)
        {
            if (_sockets.TryGetValue(connectionId, out var socket))
            {
                await socket.EmitAsync(eventName, data);
                UpdateServerActivity();
            }
        }

        public async Task StartAsync()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{_port}/");
            listener.Start();

            while (true)
            {
                var context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    var webSocket = await context.AcceptWebSocketAsync(null);
                    var socket = new SocketIoSocket(Guid.NewGuid().ToString(), webSocket.WebSocket, this);
                    _sockets.TryAdd(socket.Id, socket);
                    _connectionHandler?.Invoke(socket);
                    _ = socket.StartReceivingAsync();
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        public void OnConnection(Action<ISocketIoSocket> handler)
        {
            _connectionHandler = handler;
        }


        public async Task EmitAsync(string eventName, object data)
        {
            foreach (var socket in _sockets.Values)
            {
                await socket.EmitAsync(eventName, data);
            }
        }

        public ISocketIoRoom To(string roomId)
        {
            return new SocketIoRoom(roomId, this);
        }

        internal void RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out _);
        }

        internal void AddToRoom(string roomId, ISocketIoSocket socket)
        {
            var room = _rooms.GetOrAdd(roomId, _ => new ConcurrentDictionary<string, ISocketIoSocket>());
            room.TryAdd(socket.Id, socket);
        }

        internal void RemoveFromRoom(string roomId, ISocketIoSocket socket)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                room.TryRemove(socket.Id, out _);
            }
        }

        internal async Task EmitToRoomAsync(string roomId, string eventName, object data)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                foreach (var socket in room.Values)
                {
                    await socket.EmitAsync(eventName, data);
                }
            }
        }

        public int GetActiveConnectionsCount()
        {
            return _sockets.Count;
        }

        public int GetRoomConnectionsCount(string roomId)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                return room.Count;
            }
            return 0;
        }


    }
}
