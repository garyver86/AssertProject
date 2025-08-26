class NotificationClient {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.userId = null;
        this.authToken = null;

        this.initializeEventListeners();
    }

    initializeEventListeners() {
        // Botones de control - Usando addEventListener en lugar de onclick
        document.getElementById('connect-btn').addEventListener('click', () => this.connect());
        document.getElementById('disconnect-btn').addEventListener('click', () => this.disconnect());
        //document.getElementById('test-notification-btn').addEventListener('click', () => this.testNotification());

        // Manejar cierre de página
        window.addEventListener('beforeunload', () => this.disconnect());

        // Manejar visibilidad de la página (reconectar cuando vuelve a estar visible)
        document.addEventListener('visibilitychange', () => {
            if (document.visibilityState === 'visible' && !this.isConnected) {
                this.connect();
            }
        });
    }

    async connect() {
        try {
            this.updateConnectionStatus('🟡 Conectando...');

            // Obtener token de autenticación
            this.authToken = await this.getAuthToken();
            if (!this.authToken) {
                alert('Usuario no autenticado. Por favor inicia sesión.');
                this.updateConnectionStatus('🔴 No autenticado');
                return;
            }

            // Obtener ID de usuario desde el token
            this.userId = this.getUserIdFromToken(this.authToken);
            document.getElementById('username').textContent = `Usuario: ${this.userId}`;

            // Configurar conexión SignalR
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl('https://localhost:44317/notifications-hub', { // Cambia por tu URL
                    accessTokenFactory: () => this.authToken,
                    skipNegotiation: true,
                    transport: signalR.HttpTransportType.WebSockets
                })
                .withAutomaticReconnect({
                    nextRetryDelayInMilliseconds: (retryContext) => {
                        return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 32000);
                    }
                })
                .configureLogging(signalR.LogLevel.Warning)
                .build();

            // Configurar manejadores de eventos
            this.setupEventHandlers();

            // Iniciar conexión
            await this.connection.start();
            this.isConnected = true;

            this.updateConnectionStatus('🟢 Conectado');
            this.updateButtonStates();

            console.log('Conectado al hub de notificaciones');

            // Solicitar conteo inicial de notificaciones no leídas
            await this.connection.invoke('RequestUnreadCount');

        } catch (error) {
            console.error('Error conectando al hub:', error);
            this.updateConnectionStatus('🔴 Error de conexión');
            this.updateButtonStates();
            alert('Error conectando al servidor de notificaciones: ' + error.message);
        }
    }

    async disconnect() {
        if (this.connection) {
            try {
                this.updateConnectionStatus('🟡 Desconectando...');
                await this.connection.stop();
                this.isConnected = false;
                this.updateConnectionStatus('🔴 Desconectado');
                this.updateButtonStates();
                console.log('Desconectado del hub de notificaciones');
                document.getElementById('username').textContent = `Usuario no autenticado`;

            } catch (error) {
                console.error('Error desconectando:', error);
                this.updateConnectionStatus('🔴 Error al desconectar');
            }
        }
    }

    setupEventHandlers() {
        // 1. Notificaciones de nuevas reservas
        this.connection.on('ReceiveNotification', (notification) => {
            console.log('Nueva notificación recibida:', notification);
            this.displayNotification(notification);
            this.incrementUnreadCount();
        });

        // 2. Actualización de contador de no leídos
        this.connection.on('UpdateUnreadCount', (count) => {
            console.log('Contador actualizado:', count);
            this.updateUnreadCount(count);
        });

        // 3. Confirmación de notificación marcada como leída
        this.connection.on('NotificationMarkedAsRead', (notificationId) => {
            console.log('Notificación marcada como leída:', notificationId);
            this.markNotificationAsReadUI(notificationId);
        });

        // 4. Eventos de conexión/reconexión
        this.connection.onreconnecting((error) => {
            console.log('Reconectando...', error);
            this.updateConnectionStatus('🟡 Reconectando...');
        });

        this.connection.onreconnected((connectionId) => {
            console.log('Reconectado. Nueva connectionId:', connectionId);
            this.updateConnectionStatus('🟢 Conectado');
            // Resincronizar estado después de reconexión
            this.connection.invoke('RequestUnreadCount');
        });

        this.connection.onclose((error) => {
            console.log('Conexión cerrada', error);
            this.updateConnectionStatus('🔴 Desconectado');
            this.isConnected = false;
            this.updateButtonStates();
        });
    }

    // Método para marcar notificación como leída
    async markNotificationAsRead(notificationId) {
        if (this.isConnected) {
            try {
                await this.connection.invoke('MarkNotificationAsRead', notificationId);
            } catch (error) {
                console.error('Error marcando notificación como leída:', error);
            }
        }
    }

    // Método para probar notificación (solo desarrollo)
    async testNotification() {
        if (this.isConnected) {
            try {
                // Simular diferentes tipos de notificaciones
                const testNotifications = [
                    {
                        notificationId: Date.now(),
                        type: 'BookingRequest',
                        title: '📋 Nueva solicitud de reserva',
                        messageBody: 'Juan Pérez ha solicitado reservar tu propiedad "Casa en la Playa" para el 15-20 de Enero',
                        createdAt: new Date().toISOString(),
                        listingRentId: 789,
                        bookingId: 456,
                        actions: [
                            {
                                actionType: 'approve',
                                actionLabel: '✅ Aprobar',
                                actionUrl: '/bookings/456/approve'
                            },
                            {
                                actionType: 'reject',
                                actionLabel: '❌ Rechazar',
                                actionUrl: '/bookings/456/reject'
                            }
                        ]
                    },
                    {
                        notificationId: Date.now() + 1,
                        type: 'UserToUserMessage',
                        title: '💬 Nuevo mensaje',
                        messageBody: 'María García: "Hola, ¿la propiedad tiene wifi?"',
                        createdAt: new Date().toISOString(),
                        bookingId: 123,
                        actions: [
                            {
                                actionType: 'view',
                                actionLabel: '📩 Responder',
                                actionUrl: '/messages/123'
                            }
                        ]
                    },
                    {
                        notificationId: Date.now() + 2,
                        type: 'PaymentReceived',
                        title: '💰 Pago recibido',
                        messageBody: 'Se ha recibido el pago de $350 por la reserva #789',
                        createdAt: new Date().toISOString(),
                        bookingId: 789,
                        actions: [
                            {
                                actionType: 'view',
                                actionLabel: '📊 Ver detalles',
                                actionUrl: '/bookings/789/payment'
                            }
                        ]
                    }
                ];

                const randomNotification = testNotifications[Math.floor(Math.random() * testNotifications.length)];
                this.displayNotification(randomNotification);
                this.incrementUnreadCount();

            } catch (error) {
                console.error('Error en prueba:', error);
            }
        }
    }

    // UI Helper Methods
    displayNotification(notification) {
        const notificationsList = document.getElementById('notifications-list');

        // Limpiar mensaje inicial si existe
        if (notificationsList.innerHTML.includes('Conéctate')) {
            notificationsList.innerHTML = '';
        }

        const notificationElement = document.createElement('div');
        notificationElement.className = 'notification-item notification-new';
        notificationElement.id = `notification-${notification.notificationId}`;

        notificationElement.innerHTML = `
            <h3>${this.getNotificationIcon(notification.type)} ${notification.title}</h3>
            <p>${notification.messageBody}</p>
            <div class="notification-meta">
                <small>${new Date(notification.createdAt).toLocaleString()}</small>
                <small>Tipo: ${notification.type}</small>
            </div>
            <div class="notification-actions">
                ${notification.actions ? notification.actions.map(action =>
            `<button class="btn-action" onclick="window.notificationClient.handleAction('${action.actionType}', '${action.actionUrl}', ${notification.notificationId})">
                        ${action.actionLabel}
                    </button>`
        ).join('') : ''}
                <button class="btn-action" onclick="window.notificationClient.markNotificationAsRead(${notification.notificationId})">
                    ✔️ Marcar como leído
                </button>
            </div>
        `;

        notificationsList.prepend(notificationElement);
    }

    getNotificationIcon(type) {
        const icons = {
            'BookingRequest': '📋',
            'UserToUserMessage': '💬',
            'PaymentReceived': '💰',
            'PaymentRejected': '❌',
            'BookingApproved': '✅',
            'BookingRejected': '🚫',
            'ReviewReminder': '⭐'
        };
        return icons[type] || '🔔';
    }

    handleAction(actionType, actionUrl, notificationId) {
        console.log(`Acción: ${actionType}, URL: ${actionUrl}`);

        // Marcar como leída cuando se toma una acción
        this.markNotificationAsRead(notificationId);

        // Simular navegación (en producción, usaría tu router)
        alert(`Acción: ${actionType}\nURL: ${actionUrl}\n\n(En producción navegaría a esta URL)`);

        // Para producción real:
        // if (actionUrl && actionUrl !== '#') {
        //     window.location.href = actionUrl;
        // }
    }

    updateUnreadCount(count) {
        const badge = document.getElementById('unread-count');
        badge.textContent = count;
        badge.style.display = count > 0 ? 'inline-block' : 'none';
    }

    incrementUnreadCount() {
        const badge = document.getElementById('unread-count');
        const currentCount = parseInt(badge.textContent) || 0;
        this.updateUnreadCount(currentCount + 1);
    }

    markNotificationAsReadUI(notificationId) {
        const notificationElement = document.getElementById(`notification-${notificationId}`);
        if (notificationElement) {
            notificationElement.classList.remove('notification-new');
        }

        const badge = document.getElementById('unread-count');
        const currentCount = parseInt(badge.textContent) || 0;
        this.updateUnreadCount(Math.max(0, currentCount - 1));
    }

    updateConnectionStatus(status) {
        document.getElementById('connection-status').innerHTML = `<span>${status}</span>`;
    }

    updateButtonStates() {
        document.getElementById('connect-btn').disabled = this.isConnected;
        document.getElementById('disconnect-btn').disabled = !this.isConnected;
        //document.getElementById('test-notification-btn').disabled = !this.isConnected;
    }

    // Métodos de autenticación (simulados para demo)
    async getAuthToken() {
        // Para demo, simular un token. En producción, obtendrías esto de tu auth system
        
        const demoToken = document.getElementById('jwt-token').value;
        // En producción real:
        // return localStorage.getItem('authToken') || sessionStorage.getItem('authToken');

        return demoToken;
    }

    getUserIdFromToken(token) {
        try {
            const parts = token.split('.');
            if (parts.length !== 3) {
                showError('El token JWT debe tener 3 partes separadas por puntos.');
                return;
            }

            // Decodificar header y payload
            const header = JSON.parse(base64UrlDecode(parts[0]));
            const payload = JSON.parse(base64UrlDecode(parts[1]));

            //// Mostrar los resultados formateados
            //headerResult.textContent = JSON.stringify(header, null, 2);
            //payloadResult.textContent = JSON.stringify(payload, null, 2);

            //console.log(headerResult.textContent);
            //console.log(payloadResult.textContent);

            return payload["sub"]; // ID fijo para demo
        } catch (ex){
            return 'unknown';
        }
    }

}

    function base64UrlDecode(str) {
        // Convertir base64url a base64
        let base64 = str.replace(/-/g, '+').replace(/_/g, '/');

        // Añadir padding si es necesario
        while (base64.length % 4) {
            base64 += '=';
        }

        // Decodificar
        return decodeURIComponent(escape(atob(base64)));
    }
// Inicializar el cliente cuando se carga la página
document.addEventListener('DOMContentLoaded', () => {
    window.notificationClient = new NotificationClient();

    // Opcional: Conectar automáticamente después de 1 segundo
    setTimeout(() => {
        // notificationClient.connect(); // Descomenta para auto-conexión
    }, 1000);
});