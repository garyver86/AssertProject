using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IListingRentRepository propertyRepository,
            IBookRepository bookingRepository,
            INotificationDispatcher dispatcher,
            INotificationTypeRepository notificationTypeRepository,
            IPayPriceCalculationRepository payPriceCalculationRepository,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _listingRentRepository = propertyRepository;
            _bookingRepository = bookingRepository;
            _dispatcher = dispatcher;
            _notificationTypeRepository = notificationTypeRepository;
            _priceCalculationRepository = payPriceCalculationRepository;
            _logger = logger;
        }

        public async Task<TnNotification> GetNotificationAsync(long notificationId, int userId)
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

        public async Task MarkAsReadAsync(long notificationId, int userId)
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

        // Método helper para enviar notificaciones
        private async Task<TnNotification> CreateAndSendNotificationAsync(
            int userId,
            string notificationTypeName,
            string title,
            string messageBody,
            long? listingRentId = null,
            long? bookingId = null,
            List<TnNotificationAction> actions = null)
        {
            try
            {
                var notificationType = await _notificationTypeRepository.GetByNameAsync(notificationTypeName);
                var notification = new TnNotification
                {
                    UserId = userId,
                    TypeId = notificationType.TypeId,
                    ListingRentId = listingRentId,
                    BookingId = bookingId,
                    Title = title,
                    MessageBody = messageBody,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);

                // Añadir acciones si se proporcionan
                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        action.NotificationId = createdNotification.NotificationId;
                        await _notificationRepository.AddNotificationActionAsync(action);
                    }
                }

                // Enviar notificación en tiempo real
                await _dispatcher.SendNotificationAsync(userId, createdNotification, notificationTypeName);

                _logger.LogInformation("Notificación enviada al usuario {UserId}: {Title}", userId, title);

                return createdNotification;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación al usuario {UserId}", userId);
                throw;
            }
        }

        // Solicitud de reserva de propiedad
        public async Task SendBookingRequestNotificationAsync(int hostId, int listingRentId, int bookingId)
        {
            var property = await _listingRentRepository.Get(listingRentId, hostId);
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            var renter = await _userRepository.Get(booking.UserIdRenter);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "approve",
                ActionUrl = $"/bookings/{bookingId}/approve",
                ActionLabel = "Aprobar",
                IsPrimary = true
            },
            new TnNotificationAction
            {
                ActionType = "reject",
                ActionUrl = $"/bookings/{bookingId}/reject",
                ActionLabel = "Rechazar",
                IsPrimary = false
            }
        };

            await CreateAndSendNotificationAsync(
                hostId,
                "BookingRequest",
                "Nueva solicitud de reserva",
                $"{renter.Data?.Name} ha solicitado reservar tu propiedad {property.Name}",
                listingRentId,
                bookingId,
                actions
            );
        }

        // Notificación por pago de reserva
        public async Task SendBookingPaymentNotificationAsync(long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);
            var renter = await _userRepository.Get(booking.UserIdRenter);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/details",
                ActionLabel = "Ver Detalles",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                booking.ListingRent.OwnerUserId,
                "PaymentConfirmation",
                "Pago de reserva confirmado",
                $"{renter.Data?.Name} ha realizado el pago de la reserva de la propiedad {booking.ListingRent.Name}",
                booking.ListingRentId,
                booking.BookId,
                actions
            );
        }

        // Notificación por pago rechazado de reserva
        public async Task SendBookingPaymentRejectedNotificationAsync(long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/payment",
                ActionLabel = "Reintentar Pago",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                booking.UserIdRenter,
                "PaymentRejected",
                "Rechazo de pago de reserva",
                $"Debe realizar el pago de la reserva de la propiedad {booking.ListingRent.Name} para que la reserva sea confirmada.",
                booking.ListingRentId,
                booking.BookId,
                actions
            );
        }

        // Notificación por pago pendiente
        public async Task SendBookingPaymentPendingNotificationAsync(int userId, long priceCalculationId)
        {
            var priceCalc = await _priceCalculationRepository.GetById(priceCalculationId);
            var booking = await _bookingRepository.GetByIdAsync(priceCalc.BookId ?? 0);
            var renter = await _userRepository.Get(userId);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "review",
                ActionUrl = $"/bookings/{booking.BookId}/details",
                ActionLabel = "Ver Detalles",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                booking.ListingRent.OwnerUserId,
                "PaymentPending",
                "Pago pendiente para confirmar reserva",
                $"{renter.Data?.Name} ha realizado el pago de la reserva de la propiedad {booking.ListingRent.Name}, el cual ha sido rechazado",
                booking.ListingRentId,
                booking.BookId,
                actions
            );
        }

        // Notificación por mensaje recibido
        public async Task SendNewMessageNotificationAsync(int userIdOrigin, int userIdTo, long? bookId)
        {
            var userOrigin = await _userRepository.Get(userIdOrigin);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "review",
                ActionUrl = $"/bookings/{bookId}/message",
                ActionLabel = "Ver Mensaje",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                userIdTo,
                "UserToUserMessage",
                "Nuevo mensaje recibido",
                $"Ha recibido un nuevo mensaje de {userOrigin.Data.Name}",
                null,
                bookId,
                actions
            );
        }

        // Notificación manual
        public async Task SendNewMessageNotificationAsync(int userIdTo, string messageBody)
        {
            var actions = new List<TnNotificationAction>
            {
                new TnNotificationAction
                {
                    ActionType = "review",
                    ActionUrl = $"/notifications",
                    ActionLabel = "Ver Notificación",
                    IsPrimary = true
                }
            };

            await CreateAndSendNotificationAsync(
                userIdTo,
                "ManualNotification",
                "Notificación manual",
                messageBody,
                null,
                null,
                actions
            );
        }

        // Implementación de métodos pendientes
        public async Task SendBookingApprovedNotificationAsync(int renterId, int propertyId, int bookingId)
        {
            var property = await _listingRentRepository.Get(propertyId, 0); // 0 porque no necesitamos ownerId para lectura
            var host = await _userRepository.Get((await _bookingRepository.GetByIdAsync(bookingId)).ListingRent.OwnerUserId);

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "view",
                ActionUrl = $"/bookings/{bookingId}/details",
                ActionLabel = "Ver Reserva",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                renterId,
                "BookingApproved",
                "Reserva aprobada",
                $"Tu reserva para {property.Name} ha sido aprobada por {host.Data?.Name}",
                propertyId,
                bookingId,
                actions
            );
        }

        public async Task SendReviewReminderNotificationAsync(int userId, int propertyId, int bookingId, bool isForHost)
        {
            var property = await _listingRentRepository.Get(propertyId, 0);
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            var notificationType = isForHost ? "HostReviewReminder" : "GuestReviewReminder";
            var title = isForHost ? "Califica a tu huésped" : "Califica tu estancia";
            var message = isForHost
                ? $"Por favor califica a tu huésped de {property.Name}"
                : $"Por favor califica tu estancia en {property.Name}";

            var actions = new List<TnNotificationAction>
        {
            new TnNotificationAction
            {
                ActionType = "review",
                ActionUrl = $"/bookings/{bookingId}/review",
                ActionLabel = "Escribir Reseña",
                IsPrimary = true
            }
        };

            await CreateAndSendNotificationAsync(
                userId,
                notificationType,
                title,
                message,
                propertyId,
                bookingId,
                actions
            );
        }

        // Métodos adicionales para completar tu lista de notificaciones
        public async Task SendBookingRejectedNotificationAsync(int renterId, int propertyId, int bookingId, string reason)
        {
            var property = await _listingRentRepository.Get(propertyId, 0);

            await CreateAndSendNotificationAsync(
                renterId,
                "BookingRejected",
                "Reserva rechazada",
                $"Tu reserva para {property.Name} ha sido rechazada. Razón: {reason}",
                propertyId,
                bookingId
            );
        }

        public async Task SendCheckInReminderNotificationAsync(int guestId, int propertyId, int bookingId, DateTime checkInDate)
        {
            var property = await _listingRentRepository.Get(propertyId, 0);

            var actions = new List<TnNotificationAction>
            {
                new TnNotificationAction
                {
                    ActionType = "view",
                    ActionUrl = $"/bookings/{bookingId}/checkin",
                    ActionLabel = "Ver Instrucciones",
                    IsPrimary = true
                }
            };

            await CreateAndSendNotificationAsync(
                guestId,
                "CheckInReminder",
                "Recordatorio de check-in",
                $"Tu check-in para {property.Name} es el {checkInDate:dd/MM/yyyy}. ¡Prepárate para tu viaje!",
                propertyId,
                bookingId,
                actions
            );
        }
    }
}
