using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IListingRentRepository _listingRentRepository;
        private readonly IBookRepository _bookingRepository;
        private readonly INotificationDispatcher _dispatcher;
        private readonly IPayPriceCalculationRepository _priceCalculationRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IListingRentRepository propertyRepository,
            IBookRepository bookingRepository,
            INotificationDispatcher dispatcher,
            INotificationTypeRepository notificationTypeRepository,
            IPayPriceCalculationRepository payPriceCalculationRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _listingRentRepository = propertyRepository;
            _bookingRepository = bookingRepository;
            _dispatcher = dispatcher;
            _notificationTypeRepository = notificationTypeRepository;
            _priceCalculationRepository = payPriceCalculationRepository;
        }

        public async Task<TnNotification> GetNotificationAsync(int notificationId, int userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);

            if (notification == null || notification.UserId != userId)
                throw new Exception("Notification not found");

            // Marcar como leída al recuperarla
            if (!notification.IsRead)
            {
                await _notificationRepository.MarkAsReadAsync(notificationId);
                var count = await GetUnreadCountAsync(userId);
                await _dispatcher.UpdateUnreadCount(userId, count);
            }

            return notification;
        }

        public async Task<(List<TnNotification>, PaginationMetadata)> GetUserNotificationsAsync(int userId, int page, int pageSize)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, page, pageSize);
            var totalCount = await _notificationRepository.GetUnreadCountAsync(userId);

            return (notifications, new PaginationMetadata
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItemCount = totalCount,
                TotalPageCount = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                throw new Exception("Notification not found");

            await _notificationRepository.MarkAsReadAsync(notificationId);
            var count = await GetUnreadCountAsync(userId);
            await _dispatcher.UpdateUnreadCount(userId, count);
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _notificationRepository.MarkAllAsReadAsync(userId);
            var count = await GetUnreadCountAsync(userId);
            await _dispatcher.UpdateUnreadCount(userId, count);
        }

        public async Task<(List<TnNotification>, PaginationMetadata)> GetHistoricalNotificationsAsync(
            int userId,
            NotificationHistoryFilter filter)
        {
            var notifications = await _notificationRepository.GetHistoricalNotificationsAsync(
                userId,
                filter.FromDate,
                filter.ToDate,
                filter.TypeFilter,
                filter.Page,
                filter.PageSize);

            var totalCount = await _notificationRepository.GetHistoricalNotificationsCountAsync(
                userId,
                filter.FromDate,
                filter.ToDate,
                filter.TypeFilter);

            return (notifications, new PaginationMetadata
            {
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalItemCount = totalCount,
                TotalPageCount = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            });
        }

        //Solicitud de reserva de propiedad (Booking con aprobación.)
        public async Task SendBookingRequestNotificationAsync(int hostId, int listingRentId, int bookingId)
        {
            var property = await _listingRentRepository.Get(listingRentId, hostId);
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            var renter = await _userRepository.Get(hostId);
            var notificationType = await _notificationTypeRepository.GetByNameAsync("BookingRequest");
            var notification = new TnNotification
            {
                UserId = hostId,
                TypeId = notificationType.TypeId,
                ListingRentId = listingRentId,
                BookingId = bookingId,
                Title = "Nueva solicitud de reserva",
                MessageBody = $"{renter.Data?.Name} ha solicitado reservar tu propiedad {property.Name}",
                IsRead = false
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Añadir acciones
            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "approve",
                ActionUrl = $"/bookings/{bookingId}/approve",
                ActionLabel = "Aprobar",
                IsPrimary = true
            });

            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "reject",
                ActionUrl = $"/bookings/{bookingId}/reject",
                ActionLabel = "Rechazar",
                IsPrimary = false
            });

            // Enviar notificación en tiempo real
            await _dispatcher.SendNotificationAsync(hostId, createdNotification);
        }

        //Notificación por pago de reserva
        public async Task SendBookingPaymentNotificationAsync(long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);
            var renter = await _userRepository.Get(booking.UserIdRenter);
            var notificationType = await _notificationTypeRepository.GetByNameAsync("PaymentConfirmation");
            var notification = new TnNotification
            {
                UserId = booking.ListingRent.OwnerUserId,
                TypeId = notificationType.TypeId,
                ListingRentId = booking.ListingRentId,
                BookingId = booking.BookId,
                Title = "Pago de reserva",
                MessageBody = $"{renter.Data?.Name} ha realizado el pago de la reserva de la propiedad {booking.ListingRent.Name}",
                IsRead = false
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Añadir acciones
            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/details",
                ActionLabel = "Aprobar",
                IsPrimary = true
            });

            // Enviar notificación en tiempo real
            await _dispatcher.SendNotificationAsync(booking.ListingRent.OwnerUserId, createdNotification);
        }
        //Notificación por pago rchazado de reserva
        public async Task SendBookingPaymentRejectedNotificationAsync(long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);
            var renter = await _userRepository.Get(booking.UserIdRenter);
            var notificationType = await _notificationTypeRepository.GetByNameAsync("PaymentRejected");
            var notification = new TnNotification
            {
                UserId = booking.UserIdRenter,
                TypeId = notificationType.TypeId,
                ListingRentId = booking.ListingRentId,
                BookingId = booking.BookId,
                Title = "Rechazo de pago de reserva",
                MessageBody = $"Debe realizar el pago de la reserva de la propiedad {booking.ListingRent.Name} para que la reserva sea confirmada.",
                IsRead = false
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Añadir acciones
            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/payment",
                ActionLabel = "Aprobar",
                IsPrimary = true
            });

            // Enviar notificación en tiempo real
            await _dispatcher.SendNotificationAsync(booking.ListingRent.OwnerUserId, createdNotification);
        }
        //Notificación por pago pendiente
        public async Task SendBookingPaymentPendingNotificationAsync(int userId, long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);
            var renter = await _userRepository.Get(userId);
            var notificationType = await _notificationTypeRepository.GetByNameAsync("PaymentPending");
            var notification = new TnNotification
            {
                UserId = booking.ListingRent.OwnerUserId,
                TypeId = notificationType.TypeId,
                ListingRentId = booking.ListingRentId,
                BookingId = booking.BookId,
                Title = "Pago pendiente para confirmar reserva.",
                MessageBody = $"{renter.Data?.Name} ha realizado el pago de la reserva de la propiedad {booking.ListingRent.Name}, el cual ha sido rechazado",
                IsRead = false
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Añadir acciones
            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/details",
                ActionLabel = "Aprobar",
                IsPrimary = true
            });

            // Enviar notificación en tiempo real
            await _dispatcher.SendNotificationAsync(booking.ListingRent.OwnerUserId, createdNotification);
        }

        //Notificación por mensaje recibido
        public async Task SendNewMessageNotificationAsync(int userIdOrigin, int userIdTo, long? bookId)
        {
            var userOrigin = await _userRepository.Get(userIdOrigin);
            //var userTo = await _userRepository.Get(userIdTo);
            var notificationType = await _notificationTypeRepository.GetByNameAsync("UserToUserMessage");
            var notification = new TnNotification
            {
                UserId = userIdTo,
                TypeId = notificationType.TypeId,
                BookingId = bookId,
                Title = "Nuevo mensaje recibido.",
                MessageBody = $"Ha recibido un nuevo mensaje de {userOrigin.Data.Name}",
                IsRead = false
            };

            var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

            // Añadir acciones
            await _notificationRepository.AddNotificationActionAsync(new TnNotificationAction
            {
                NotificationId = createdNotification.NotificationId,
                ActionType = "review",
                ActionUrl = $"/bookings/{bookId}/message",
                ActionLabel = "Ver Mensaje",
                IsPrimary = true
            });

            // Enviar notificación en tiempo real
            await _dispatcher.SendNotificationAsync(userIdTo, createdNotification);
        }

        /*
         * Lista de Notificaciones sugeridas:
         * 
         * 1. Reservas y Reservaciones
            ✅ Solicitud de reserva enviada (Inquilino → Anfitrión) ************************
            ✅ Reserva aprobada (Anfitrión → Inquilino)
            ✅ Reserva rechazada (Anfitrión → Inquilino)
            ✅ Reserva cancelada por el inquilino (Inquilino → Anfitrión)
            ✅ Reserva cancelada por el anfitrión (Anfitrión → Inquilino)
            ✅ Pago de reserva confirmado (Sistema → Inquilino)******************************
            ✅ Pago fallido/rechazado (Sistema → Inquilino)******************************
            ✅ Recordatorio de pago pendiente (Sistema → Inquilino) *************************

            2. Check-in/Check-out y Estadía
            ✅ Check-in confirmado (Sistema → Anfitrión e Inquilino)
            ✅ Check-out confirmado (Sistema → Anfitrión e Inquilino)
            ✅ Recordatorio de check-out próximo (Sistema → Inquilino)
            ✅ Instrucciones de acceso enviadas (Automático/Sistema → Inquilino)

            3. Mensajería y Comunicación
            ✅ Nuevo mensaje recibido (Usuario → Usuario) ***********************************
            ✅ Mensaje no leído después de X tiempo (Sistema → Usuario)

            4. Reseñas y Calificaciones
            ✅ Solicitud de reseña post-estadía (Sistema → Inquilino)
            ✅ Solicitud de reseña post-estadía (Sistema → Anfitrión)
            ✅ Nueva reseña recibida (Sistema → Usuario evaluado)
            ✅ Respuesta a tu reseña (Sistema → Usuario original)

            5. Alertas de Precios y Disponibilidad
            ✅ Precio reducido en propiedad favorita (Sistema → Usuario)
            ✅ Propiedad disponible en fechas deseadas (Sistema → Usuario)

            6. Alertas de Seguridad y Verificación
            ✅ Verificación de identidad requerida (Sistema → Usuario)
            ✅ Verificación de identidad aprobada/rechazada (Sistema → Usuario)
            ✅ Actividad sospechosa detectada (Sistema → Usuario/Admin)

            7. Notificaciones del Anfitrión
            ✅ Nueva regla/actualización de propiedad (Anfitrión → Inquilinos futuros)
            ✅ Mantenimiento programado (Anfitrión → Inquilino actual)
            ✅ Cambio en políticas de cancelación (Sistema → Usuarios afectados)

            8. Notificaciones del Sistema
            ✅ Actualizaciones importantes de la plataforma (Sistema → Todos usuarios)
            ✅ Cambios en términos y condiciones (Sistema → Todos usuarios)
            ✅ Ofertas especiales/promociones (Sistema → Usuarios seleccionados)

            9. Alertas de Emergencia
            ⚠️ Problema reportado en propiedad (Inquilino → Anfitrión)
            ⚠️ Emergencia en propiedad (Sistema → Anfitrión e Inquilino)
            ⚠️ Clima extremo que afecta la reserva (Sistema → Usuarios afectados)

            10. Notificaciones de Soporte
            ✅ Ticket de soporte creado (Sistema → Usuario)
            ✅ Respuesta a ticket de soporte (Soporte → Usuario)
            ✅ Encuesta de satisfacción de soporte (Sistema → Usuario)

            Notas importantes:
            Cada notificación debe ser configurable (el usuario puede elegir qué notificaciones recibir)
            Priorizar por urgencia (emergencias vs. informativas)

            Incluir acciones rápidas cuando sea posible (ej: "Aprobar reserva" directamente desde la notificación)
         * */

        public Task SendBookingApprovedNotificationAsync(int renterId, int propertyId, int bookingId)
        {
            throw new NotImplementedException();
        }

        public Task SendReviewReminderNotificationAsync(int renterId, int propertyId, int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
