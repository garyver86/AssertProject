using Assert.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Assert.Domain.Implementation
{
    public class SocketIoSocket : ISocketIoSocket, IDisposable
    {
        public string Id { get; }
        public DateTime LastActivityTime { get; private set; } = DateTime.UtcNow;
        public WebSocketState State => _webSocket?.State ?? WebSocketState.None;


        private readonly ConcurrentDictionary<string, bool> _joinedRooms = new();

        private readonly WebSocket _webSocket;
        private readonly SocketIoServer _server;
        //private readonly ActivityTracker _activityTracker;
        private readonly ConcurrentDictionary<string, Action<object>> _handlers = new();
        private Action _disconnectHandler;
        private readonly CancellationTokenSource _cts = new();
        private bool _disposed = false;

        //public SocketIoSocket(string id, WebSocket webSocket, SocketIoServer server, ActivityTracker activityTracker)
        public SocketIoSocket(string id, WebSocket webSocket, SocketIoServer server)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
            _server = server ?? throw new ArgumentNullException(nameof(server));
            //_activityTracker = activityTracker ?? throw new ArgumentNullException(nameof(activityTracker));
        }

        public async Task StartReceivingAsync()
        {
            var buffer = new byte[1024 * 4]; // Buffer de 4KB

            try
            {
                while (_webSocket.State == WebSocketState.Open && !_cts.IsCancellationRequested)
                {
                    var result = await _webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        _cts.Token);

                    UpdateActivityTime("receive");

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        ProcessMessage(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await HandleDisconnection();
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Cancelación normal
            }
            catch (WebSocketException wsEx) when (wsEx.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                // Conexión cerrada abruptamente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en conexión {Id}: {ex.Message}");
            }
            finally
            {
                await HandleDisconnection();
            }
        }

        private async Task HandleDisconnection()
        {
            if (_disposed) return;

            try
            {
                if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.CloseReceived)
                {
                    await _webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None);
                }
            }
            finally
            {
                _disconnectHandler?.Invoke();
                _server.RemoveSocket(Id);
                Dispose();
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                var socketMessage = JsonSerializer.Deserialize<SocketIoMessage>(message);
                if (socketMessage != null && _handlers.TryGetValue(socketMessage.EventName, out var handler))
                {
                    handler(socketMessage.Data);
                    UpdateActivityTime($"handle:{socketMessage.EventName}");
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Error deserializando mensaje: {jsonEx.Message}");
            }
        }

        public async Task EmitAsync(string eventName, object data)
        {
            if (_webSocket.State != WebSocketState.Open || _disposed)
                return;

            try
            {
                var message = new SocketIoMessage { EventName = eventName, Data = data };
                var json = JsonSerializer.Serialize(message);
                var bytes = Encoding.UTF8.GetBytes(json);

                await _webSocket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    _cts.Token);

                UpdateActivityTime($"emit:{eventName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando mensaje: {ex.Message}");
                await HandleDisconnection();
            }
        }

        public void On(string eventName, Action<object> handler)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentException("Event name cannot be empty", nameof(eventName));

            _handlers[eventName] = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public void OnDisconnect(Action handler)
        {
            _disconnectHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        private void UpdateActivityTime(string eventType = "activity")
        {
            LastActivityTime = DateTime.UtcNow;
            //_activityTracker?.LogActivity(Id, eventType);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _cts.Cancel();
            _cts.Dispose();
            _webSocket.Dispose();

            GC.SuppressFinalize(this);
        }

        public TimeSpan GetInactiveTime()
        {
            return DateTime.UtcNow - LastActivityTime; ;
        }

        public bool IsInRoom(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentException("Room ID cannot be null or empty", nameof(roomId));

            return _joinedRooms.ContainsKey(roomId);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure, string statusDescription = "Closed by server")
        {
            if (_disposed)
                return;

            try
            {
                if (_webSocket.State == WebSocketState.Open ||
                    _webSocket.State == WebSocketState.CloseReceived)
                {
                    await _webSocket.CloseAsync(closeStatus, statusDescription, _cts.Token);
                    //_activityTracker?.LogActivity(Id, $"closed:{closeStatus}");
                }
            }
            catch (OperationCanceledException)
            {
                // Cancelación esperada
            }
            catch (WebSocketException wsEx)
            {
                Console.WriteLine($"WebSocket error during close: {wsEx.WebSocketErrorCode}");
            }
            finally
            {
                await HandleDisconnection();
            }
        }

        public void Join(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentException("Room ID cannot be null or empty", nameof(roomId));

            if (_joinedRooms.TryAdd(roomId, true))
            {
                _server.AddToRoom(roomId, this);
                UpdateActivityTime($"join:{roomId}");
            }
        }

        public void Leave(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentException("Room ID cannot be null or empty", nameof(roomId));

            if (_joinedRooms.TryRemove(roomId, out _))
            {
                _server.RemoveFromRoom(roomId, this);
                UpdateActivityTime($"leave:{roomId}");
            }
        }

        ~SocketIoSocket()
        {
            Dispose();
        }
    }
    public class SocketIoRoom : ISocketIoRoom
    {
        private readonly string _roomId;
        private readonly SocketIoServer _server;
        private readonly ConcurrentDictionary<string, ISocketIoSocket> _sockets = new();
        private readonly ILogger<SocketIoRoom> _logger;

        public string RoomId => _roomId;
        public int ConnectionCount => _sockets.Count;
        public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

        public SocketIoRoom(string roomId, SocketIoServer server)
        {
            _roomId = roomId ?? throw new ArgumentNullException(nameof(roomId));
            _server = server ?? throw new ArgumentNullException(nameof(server));
        }

        /// <summary>
        /// Agrega un socket a esta sala
        /// </summary>
        public bool AddSocket(ISocketIoSocket socket)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            var added = _sockets.TryAdd(socket.Id, socket);
            if (added)
            {
                UpdateActivity();
                _logger.LogInformation($"Socket {socket.Id} joined room {_roomId}");
            }
            return added;
        }

        /// <summary>
        /// Remueve un socket de esta sala
        /// </summary>
        public bool RemoveSocket(string socketId)
        {
            var removed = _sockets.TryRemove(socketId, out _);
            if (removed)
            {
                UpdateActivity();
                _logger.LogInformation($"Socket {socketId} left room {_roomId}");
            }
            return removed;
        }

        /// <summary>
        /// Envía un mensaje a todos los sockets en esta sala
        /// </summary>
        public async Task EmitAsync(string eventName, object data)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentException("Event name cannot be null or empty", nameof(eventName));

            UpdateActivity();
            var tasks = new List<Task>();

            foreach (var socket in _sockets.Values.Where(s => s.State == WebSocketState.Open))
            {
                try
                {
                    tasks.Add(socket.EmitAsync(eventName, data));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error emitting to socket {socket.Id} in room {_roomId}");
                }
            }

            await Task.WhenAll(tasks);
            _logger.LogDebug($"Emitted '{eventName}' to {tasks.Count} sockets in room {_roomId}");
        }

        /// <summary>
        /// Obtiene todos los sockets en esta sala
        /// </summary>
        public IEnumerable<ISocketIoSocket> GetSockets()
        {
            return _sockets.Values.ToList();
        }

        /// <summary>
        /// Verifica si un socket específico está en esta sala
        /// </summary>
        public bool HasSocket(string socketId)
        {
            return _sockets.ContainsKey(socketId);
        }

        /// <summary>
        /// Limpia todos los sockets inactivos de la sala
        /// </summary>
        public async Task CleanInactiveSockets(TimeSpan inactivityThreshold)
        {
            var inactiveSockets = _sockets.Values
                .Where(s => s.GetInactiveTime() > inactivityThreshold)
                .ToList();

            foreach (var socket in inactiveSockets)
            {
                _sockets.TryRemove(socket.Id, out _);
                _logger.LogInformation($"Removed inactive socket {socket.Id} from room {_roomId}");

                try
                {
                    await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Inactive connection");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error closing inactive socket {socket.Id}");
                }
            }
        }

        private void UpdateActivity()
        {
            LastActivity = DateTime.UtcNow;
        }
    }
    public class SocketIoMessage
    {
        public string EventName { get; set; }
        public object Data { get; set; }
    }
}
