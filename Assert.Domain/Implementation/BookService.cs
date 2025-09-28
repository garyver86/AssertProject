using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using static Azure.Core.HttpHeader;

namespace Assert.Domain.Implementation
{
    public class BookService : IBookService
    {
        private readonly IListingRentRepository _listingRentRepository;
        private readonly IListingCalendarRepository _listingCalendarRepository;
        private readonly IListingPricingRepository _listingPricingRepository;
        private readonly IListingDiscountRepository _listingDiscountRepository;
        private readonly IPayPriceCalculationRepository _payPriceCalculationRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly IListingDiscountForRateRepository _listingDiscountForRateRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICurrencyRespository _currencyrepository;
        private readonly IPayTransactionRepository _payTransactionRepository;
        private readonly IListingAdditionalFeeRepository _listingAdditionalFee;
        private readonly IAssertFeeRepository _assertFeeRepository;
        private readonly INotificationService _notificationService;

        public BookService(
            IListingRentRepository listingRentRepository,
            IListingCalendarRepository listingCalendarRepository,
            IListingPricingRepository listingPricingRepository,
            IListingDiscountRepository listingDiscountRepository,
            IErrorHandler errorHandler,
            IPayPriceCalculationRepository payPriceCalculationRepository,
            IListingDiscountForRateRepository listingDiscountForRateRepository,
            IBookRepository bookRepository,
            ICurrencyRespository currencyrepository,
            IPayTransactionRepository payTransactionRepository,
            IListingAdditionalFeeRepository listingAdditionalFee,
            IAssertFeeRepository assertFeeRepository,
            INotificationService notificationService)
        {
            _listingRentRepository = listingRentRepository;
            _listingCalendarRepository = listingCalendarRepository;
            _listingPricingRepository = listingPricingRepository;
            _listingDiscountRepository = listingDiscountRepository;
            _errorHandler = errorHandler;
            _payPriceCalculationRepository = payPriceCalculationRepository;
            _listingDiscountForRateRepository = listingDiscountForRateRepository;
            _bookRepository = bookRepository;
            _currencyrepository = currencyrepository;
            _payTransactionRepository = payTransactionRepository;
            _listingAdditionalFee = listingAdditionalFee;
            _assertFeeRepository = assertFeeRepository;
            _notificationService = notificationService;
        }



        /// <summary>
        /// Si el propietario cambia el precio del alquiler durante esos 30 minutos, la cotización NO se actualiza automáticamente:
        /// •	El usuario podrá reservar al precio que se le cotizó, aunque el propietario haya cambiado el precio después.
        /// •	El monto, moneda y detalles de la cotización quedan “congelados” hasta la expiración(o hasta que el usuario reserve).
        /// ¿Por qué?
        /// Esto es una práctica común para evitar confusión y dar certeza al usuario que está reservando.
        /// Solo después de que la cotización expira, si el usuario solicita una nueva, se calculará con el nuevo precio.
        /// </summary>
        /// <param name="listingRentId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="guestId"></param>
        /// <param name="clientData"></param>
        /// <param name="useTechnicalMessages"></param>
        /// <returns></returns>
        public async Task<ReturnModel<(PayPriceCalculation, List<PriceBreakdownItem>)>> CalculatePrice(
            long listingRentId,
            DateTime startDate,
            DateTime endDate,
            int guestId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages)
        {
            var result = new ReturnModel<(PayPriceCalculation, List<PriceBreakdownItem>)>();
            var breakdown = new List<PriceBreakdownItem>();

            // 1. Validar existencia y estado de la propiedad
            var listing = await _listingRentRepository.Get(listingRentId, 0, onlyActive: true);
            if (listing == null)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.NotFound;
                result.ResultError = new ErrorCommon { Message = "La propiedad no existe o no está activa." };
                return result;
            }
            else if (listing.ListingStatusId != 3)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = "La propiedad no está disponible para reservas." };
                return result;
            }
            else if (listing.OwnerUser.BlockAsHost == true)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = "La propiedad no está disponible para reservas (bl)." };
                return result;
            }
            else if (listing.OwnerUserId == guestId)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = "No se puede reservar una propiedad propia." };
                return result;
            }
            // 2. Validar fechas
            if (startDate >= endDate)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = "La fecha de inicio debe ser menor a la fecha de fin." };
                return result;
            }
            if (startDate < DateTime.Today)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = "No se pueden reservar fechas en el pasado." };
                return result;
            }

            // 3. Validar conflicto con reservas o bloqueos
            var (calendarDays, _) = await _listingCalendarRepository.GetCalendarDaysAsync(
                (int)listingRentId, startDate, endDate, 1, (int)(endDate - startDate).TotalDays + 1);

            if (calendarDays.Any(d => d.BlockType != null))
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.Conflict;
                result.ResultError = new ErrorCommon { Message = "Las fechas seleccionadas no están disponibles." };
                return result;
            }

            // 4. Validar mínimo y máximo de noches
            int nights = (endDate - startDate).Days;
            if (listing.MinimunStay.HasValue && nights < listing.MinimunStay.Value)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = $"La estancia mínima es de {listing.MinimunStay.Value} noches." };
                return result;
            }
            if (listing.MaximumStay.HasValue && nights > listing.MaximumStay.Value)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = $"La estancia máxima es de {listing.MaximumStay.Value} noches." };
                return result;
            }

            // 5. Obtener precios y descuentos
            var priceInfo = await _listingPricingRepository.GetByListingRent(listingRentId);
            var discounts = await _listingDiscountForRateRepository.GetByListingRentId(listingRentId);


            if (priceInfo == null)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = new ErrorCommon { Message = $"La propiedad no cuenta con precios." };
                return result;
            }

            // 6. Calcular precio por día considerando tarifas especiales
            decimal total = 0;
            decimal totalDiscount = 0;
            decimal totalAdditionalFees = 0;
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                var calendarDay = calendarDays.FirstOrDefault(d => d.Date == DateOnly.FromDateTime(date));

                // Si el día está bloqueado, lo ignoramos (ya validaste antes, pero por seguridad)
                if (calendarDay != null && calendarDay.BlockType != null)
                    continue;

                // 1. Precio base del día
                decimal dayPrice;
                string priceType = "STANDARD";

                if (calendarDay != null && calendarDay.Price.HasValue)
                {
                    dayPrice = calendarDay.Price.Value;
                    priceType = "CUSTOM";
                }
                else
                {
                    bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                    if (isWeekend && priceInfo.WeekendNightlyPrice.HasValue)
                    {
                        dayPrice = priceInfo.WeekendNightlyPrice.Value;
                        priceType = "WEEKEND";
                    }
                    else
                    {
                        dayPrice = priceInfo.PriceNightly ?? 0;
                    }
                }
                total += dayPrice;
                breakdown.Add(new PriceBreakdownItem
                {
                    Type = "BASE_PRICE",
                    Description = $"Precio por noche ({priceType})",
                    Amount = dayPrice,
                    Date = date
                });

                // 2. Descuentos por día (TLCalendarDiscount)
                if (calendarDay != null && calendarDay.TlCalendarDiscounts != null)
                {
                    foreach (var calDiscount in calendarDay.TlCalendarDiscounts)
                    {
                        if (calDiscount.IsDiscount)
                        {
                            //totalDiscount += dayPrice * (calDiscount.Porcentage / 100m);
                            decimal discountAmount = dayPrice * (calDiscount.Porcentage / 100m);
                            totalDiscount += discountAmount;

                            breakdown.Add(new PriceBreakdownItem
                            {
                                Type = "DAILY_DISCOUNT",
                                Description = $"Descuento diario ({calDiscount.Porcentage}%)",
                                Amount = -discountAmount,
                                Percentage = calDiscount.Porcentage,
                                Date = date
                            });
                        }
                    }
                }

                //// 3. Tarifas adicionales por día (TLAdditionalFees)
                //if (calendarDay != null && calendarDay.TlListingCalendarAdditionalFees != null)
                //{
                //    foreach (var fee in calendarDay.TlListingCalendarAdditionalFees)
                //    {
                //        decimal feeAmount = fee.IsPercent ? (total * fee.AmountFee / 100) : fee.AmountFee;
                //        totalAdditionalFees += feeAmount;
                //        breakdown.Add(new PriceBreakdownItem
                //        {
                //            Type = fee.AdditionalFee.FeeCode ?? "ADDITIONAL_FEE",
                //            Description = fee.AdditionalFee.DeeDescription,
                //            Amount = feeAmount,
                //            Percentage = fee.IsPercent ? fee.AmountFee : 0,
                //        });
                //    }
                //}
                //TODO: En este caso, preguntar si se debe aplicar la tarifa adicional por día o por reserva completa. EN caso de tener
                // tarifas adicionales por día, se debe aplicar el total de tarifas adicionales al final del cálculo?.
            }

            // 7. Sumar tarifas adicionales
            var additionalFees = await _listingAdditionalFee.GetByListingRentId(listingRentId, listing.OwnerUserId);
            if (additionalFees != null && additionalFees.Count > 0)
            {
                foreach (var fee in additionalFees)
                {
                    if (fee.AdditionalFee != null)
                    {
                        decimal feeAmount = fee.IsPercent ? (total * fee.AmountFee / 100) : fee.AmountFee;
                        totalAdditionalFees += feeAmount;
                        breakdown.Add(new PriceBreakdownItem
                        {
                            Type = fee.AdditionalFee.FeeCode ?? "ADDITIONAL_FEE",
                            Description = fee.AdditionalFee.DeeDescription,
                            Amount = feeAmount,
                            Percentage = fee.IsPercent ? fee.AmountFee : 0,
                        });
                    }
                }
            }

            // 8. Aplicar descuentos generales
            if (discounts != null)
            {
                foreach (var discount in discounts)
                {
                    if (discount.IsDiscount)
                    {
                        //totalDiscount += total * (discount.Porcentage / 100m);
                        decimal discountAmount = total * (discount.Porcentage / 100m);
                        totalDiscount += discountAmount;

                        breakdown.Add(new PriceBreakdownItem
                        {
                            Type = "GENERAL_DISCOUNT",
                            Description = $"Descuento general ({discount.Porcentage}%)",
                            Amount = -discountAmount,
                            Percentage = discount.Porcentage
                        });
                    }
                }
            }

            // 9. Total final
            total = total + totalAdditionalFees - totalDiscount;

            // 9. Aplicar fee de uso de plataforma
            var assertFee = await _assertFeeRepository.GetAssertFee(listingRentId);
            if (assertFee != null)
            {
                decimal assertFeeAmount = assertFee.FeeBase ?? 0;
                decimal asserFeePercent = assertFee.FeePercent ?? 0;
                if (asserFeePercent > 0)
                {
                    assertFeeAmount = total * (asserFeePercent / 100);
                }
                if (assertFeeAmount > 0)
                {
                    total += assertFeeAmount;
                    breakdown.Add(new PriceBreakdownItem
                    {
                        Type = "PLATFORM_FEE",
                        Description = "Tarifa de uso de plataforma",
                        Amount = assertFeeAmount,
                        Percentage = asserFeePercent > 0 ? asserFeePercent : null,
                    });
                }
            }


            // 10. Construir el modelo de resultado
            var payPriceCalculation = new PayPriceCalculation
            {
                Amount = total,
                CurrencyCode = priceInfo.Currency?.Code ?? "BOB",
                CalculationDetails = $"Reserva de {nights} noches del {startDate:yyyy-MM-dd} al {endDate:yyyy-MM-dd}",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(30),
                CalculationStatue = "PENDING",
                CalculationCode = Guid.NewGuid(),
                IpAddress = clientData != null && clientData.ContainsKey("ip") ? clientData["ip"] : null,
                UserAgent = clientData != null && clientData.ContainsKey("userAgent") ? clientData["userAgent"] : null,
                InitBook = startDate,
                EndBook = endDate,
                ListingRentId = listingRentId,
                BreakdownInfo = JsonConvert.SerializeObject(breakdown),
                CalculationStatusId = 1,
                UserId = guestId
            };

            var resultCalculation = await _payPriceCalculationRepository.Create(payPriceCalculation);

            result.Data = (payPriceCalculation, breakdown);
            result.StatusCode = ResultStatusCode.OK;
            result.HasError = false;
            return result;
        }


        public async Task<ReturnModel<TbBook>> RegisterPaymentAndCreateBooking(
            PaymentRequest paymentRequest,
            int userId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages)
        {

            PayPriceCalculation priceCalculation = await _payPriceCalculationRepository.GetByCode(paymentRequest.CalculationCode);

            if (priceCalculation == null)
            {
                return new ReturnModel<TbBook>
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El código de cotización ingresado no puede ser encontrado.", useTechnicalMessages)
                };
            }
            if (priceCalculation.UserId != userId)
            {
                return new ReturnModel<TbBook>
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El código de cotización ingresado no pertenece al usuario autenticado.", useTechnicalMessages)
                };
            }

            var result = new ReturnModel<TbBook>();

            // 1. Validar que la cotización aún sea válida (no haya expirado)
            if (priceCalculation.ExpirationDate < DateTime.UtcNow)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.BadRequest,
                    "La cotización ha expirado. Por favor, solicite una nueva cotización.",
                    useTechnicalMessages);
                return result;
            }

            // 2. Validar que el monto del pago coincida con la cotización
            if (paymentRequest.Amount != priceCalculation.Amount ||
                paymentRequest.CurrencyCode != priceCalculation.CurrencyCode)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.BadRequest,
                    "El monto o moneda del pago no coincide con la cotización.",
                    useTechnicalMessages);
                return result;
            }

            // 3. Validar disponibilidad nuevamente (en caso de que haya cambiado desde la cotización)
            ReturnModel avaResult = await CheckAvailability(
                (int)priceCalculation.ListingRentId,
                priceCalculation.InitBook,
                priceCalculation.EndBook);

            if (avaResult.StatusCode != ResultStatusCode.OK)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.Conflict;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.Conflict,
                    "Las fechas seleccionadas ya no están disponibles.",
                    useTechnicalMessages);
                return result;
            }

            // 4. Crear transacción de pago
            var transaction = new PayTransaction
            {
                OrderCode = paymentRequest.OrderCode,
                Stan = paymentRequest.Stan,
                Amount = paymentRequest.Amount,
                CurrencyCode = paymentRequest.CurrencyCode,
                MethodOfPaymentId = paymentRequest.MethodOfPaymentId,
                PaymentProviderId = paymentRequest.PaymentProviderId,
                CountryId = paymentRequest.CountryId,
                TransactionStatusCode = "APP", // Aprobado
                TransactionStatus = "AUTHORISED",
                PaymentData = paymentRequest.PaymentData,
                TransactionData = paymentRequest.TransactionData,
                CreatedAt = DateTime.UtcNow
            };

            TbBook booking = new TbBook();
            if (priceCalculation.BookId == null)
            {
                // 5. Crear la reserva

                CheckinValuesModel checInOutValues = CalculateCheckInOutValues(priceCalculation);
                CheckinValuesModel CancellationValues = CalculateCancellationValues(priceCalculation, (DateTime)checInOutValues.CheckIn);

                booking = new TbBook
                {
                    ListingRentId = priceCalculation.ListingRentId ?? 0,
                    UserIdRenter = userId,
                    StartDate = (DateTime)priceCalculation.InitBook?.Date,
                    EndDate = (DateTime)priceCalculation.EndBook?.Date,
                    AmountTotal = priceCalculation.Amount,
                    CurrencyId = await _currencyrepository.GetCurrencyId(priceCalculation.CurrencyCode),
                    NameRenter = clientData.ContainsKey("name") ? clientData["name"] : string.Empty,
                    LastNameRenter = clientData.ContainsKey("lastName") ? clientData["lastName"] : string.Empty,
                    TermsAccepted = true, // Asumimos que se aceptaron los términos para llegar aquí
                    BookStatusId = 3, // Estado inicial (por ejemplo, "Confirmado")
                    PaymentCode = paymentRequest.OrderCode,
                    Checkin = checInOutValues.CheckIn,
                    Checkout = checInOutValues.CheckOut,
                    MaxCheckin = checInOutValues.MaxCheckIn,
                    PaymentId = "",
                    CancellationStart = checInOutValues.CancellationStart,
                    CancellationEnd = checInOutValues.CancellationEnd

                };

                long bookId = await _bookRepository.UpsertBookAsync(booking);

                if (bookId <= 0)
                {
                    result.HasError = true;
                    result.StatusCode = ResultStatusCode.Conflict;
                    result.ResultError = _errorHandler.GetError(
                        ResultStatusCode.Conflict,
                        "La reserva no ha podido ser creada.",
                        useTechnicalMessages);
                    return result;
                }
                transaction.BookingId = bookId;

                booking = await _bookRepository.GetByIdAsync(bookId);
            }
            else
            {
                transaction.BookingId = priceCalculation.BookId;
                booking = await _bookRepository.GetByIdAsync(transaction.BookingId ?? 0);
                booking.BookStatusId = 3; // Confirmado
                booking.PaymentCode = paymentRequest.OrderCode;
                await _bookRepository.UpsertBookAsync(booking);
            }
            long savedTransaction = await _payTransactionRepository.Create(transaction);

            List<DateOnly> dates = new List<DateOnly>();

            for (var date = booking.StartDate; date <= booking.EndDate; date = date.AddDays(1))
            {
                dates.Add(DateOnly.FromDateTime(date));
            }

            var resultBlock = await _listingCalendarRepository.BulkBlockDaysAsync(priceCalculation.ListingRentId ?? 0, dates, 2, "Alquiler de propiedad", priceCalculation.BookId, null);

            if (priceCalculation.ListingRent?.PreparationDays > 0)
            {
                DateTime initPreparation = booking.EndDate.AddDays(1);
                DateTime endPreparation = booking.EndDate.AddDays(priceCalculation.ListingRent?.PreparationDays ?? 1);
                List<DateOnly> preparationDates = new List<DateOnly>();

                for (var date = initPreparation; date <= endPreparation; date = date.AddDays(1))
                {
                    preparationDates.Add(DateOnly.FromDateTime(date));
                }
                var resultBlockPreparation = await _listingCalendarRepository.BulkBlockDaysAsync(priceCalculation.ListingRentId ?? 0, preparationDates, 3, "Preparación", transaction.BookingId, null);
            }
            var resultStatusUpdate = await _payPriceCalculationRepository.SetAsPayed(paymentRequest.CalculationCode, paymentRequest.PaymentProviderId,
                paymentRequest.MethodOfPaymentId, savedTransaction);

            return new ReturnModel<TbBook>
            {
                Data = booking,
                StatusCode = ResultStatusCode.OK,
                HasError = false,
            };
        }

        private CheckinValuesModel CalculateCheckInOutValues(PayPriceCalculation priceCalculation)
        {
            if (priceCalculation.ListingRent?.TlCheckInOutPolicies == null || priceCalculation.ListingRent?.TlCheckInOutPolicies.Count == 0)
            {
                return new CheckinValuesModel
                {
                    CheckIn = null,
                    CheckOut = null,
                    MaxCheckIn = null
                };
            }
            var policy = priceCalculation.ListingRent?.TlCheckInOutPolicies.FirstOrDefault();
            if (policy == null)
            {
                return new CheckinValuesModel
                {
                    CheckIn = null,
                    CheckOut = null,
                    MaxCheckIn = null
                };
            }
            else
            {
                DateTime? _checkIn = null;
                if (priceCalculation.InitBook.HasValue && policy.CheckInTime.HasValue)
                {
                    _checkIn = ((DateTime)priceCalculation.InitBook?.Date) + ((TimeOnly)policy.CheckInTime).ToTimeSpan();
                }
                DateTime? _checkOut = null;
                if (priceCalculation.EndBook.HasValue && policy.CheckOutTime.HasValue)
                {
                    _checkOut = ((DateTime)priceCalculation.EndBook?.Date) + ((TimeOnly)policy.CheckOutTime).ToTimeSpan();
                }
                DateTime? _maxCheckIn = null;
                if (_checkIn.HasValue && policy.MaxCheckInTime.HasValue)
                {
                    _maxCheckIn = ((DateTime)priceCalculation.InitBook?.Date) + ((TimeOnly)policy.MaxCheckInTime).ToTimeSpan();
                }
                return new CheckinValuesModel
                {
                    CheckIn = _checkIn,
                    CheckOut = _checkOut,
                    MaxCheckIn = _maxCheckIn
                };
            }
        }

        private CheckinValuesModel CalculateCancellationValues(PayPriceCalculation priceCalculation, DateTime checkIn)
        {
            if (priceCalculation.ListingRent?.CancelationPolicyType == null)
            {
                return new CheckinValuesModel
                {
                    CheckIn = null,
                    CheckOut = null,
                    MaxCheckIn = null
                };
            }
            var policy = priceCalculation.ListingRent?.CancelationPolicyType;
            if (policy == null)
            {
                return new CheckinValuesModel
                {
                    CheckIn = null,
                    CheckOut = null,
                    MaxCheckIn = null
                };
            }
            else
            {
                DateTime? _cancellationEnd = null;
                if (priceCalculation.InitBook.HasValue && policy.HoursBeforeCheckIn != null)
                {
                    _cancellationEnd = (checkIn).AddHours(((double)policy.HoursBeforeCheckIn) * -1);
                }
                DateTime? _cancellationStart = null;
                if (priceCalculation.InitBook.HasValue && policy.HoursAfetrBooking != null)
                {
                    _cancellationStart = ((DateTime)priceCalculation.InitBook?.Date).AddHours(((double)policy.HoursAfetrBooking) * -1);
                }

                if (_cancellationStart.HasValue && _cancellationEnd.HasValue && _cancellationStart > _cancellationEnd)
                {
                    _cancellationStart = null;
                }

                return new CheckinValuesModel
                {
                    CancellationEnd = _cancellationEnd,
                    CancellationStart = _cancellationStart
                };
            }
        }

        public async Task<ReturnModel<TbBook>> BookingRequestApproval(
            Guid CalculationCode,
            int userId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages)
        {

            PayPriceCalculation priceCalculation = await _payPriceCalculationRepository.GetByCode(CalculationCode);

            if (priceCalculation == null)
            {
                return new ReturnModel<TbBook>
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El código de cotización ingresado no puede ser encontrado.", useTechnicalMessages)
                };
            }

            var result = new ReturnModel<TbBook>();

            // 1. Validar que la cotización aún sea válida (no haya expirado)
            if (priceCalculation.ExpirationDate < DateTime.UtcNow)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.BadRequest,
                    "La cotización ha expirado. Por favor, solicite una nueva cotización.",
                    useTechnicalMessages);
                return result;
            }

            // 3. Validar disponibilidad nuevamente (en caso de que haya cambiado desde la cotización)
            ReturnModel avaResult = await CheckAvailability(
                (int)priceCalculation.ListingRentId,
                priceCalculation.InitBook,
                priceCalculation.EndBook);

            if (avaResult.StatusCode != ResultStatusCode.OK)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.Conflict;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.Conflict,
                    "Las fechas seleccionadas ya no están disponibles.",
                    useTechnicalMessages);
                return result;
            }

            var listing = _listingRentRepository.Get((int)priceCalculation.ListingRentId, 0, onlyActive: true).Result;
            if (listing == null)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.NotFound;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.NotFound,
                    "La propiedad no existe o no está activa.",
                    useTechnicalMessages);
                return result;
            }

            if (listing.ApprovalPolicyTypeId == 2)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.BadRequest,
                    "La propiedad no está configurada para reservas con aprobación del anfitrión.",
                    useTechnicalMessages);
                return result;
            }

            if (listing.ApprovalRequestDays > 0)
            {
                if (priceCalculation.InitBook.HasValue && priceCalculation.InitBook.Value <= DateTime.UtcNow.AddDays(listing.ApprovalRequestDays.Value))
                {
                    result.HasError = true;
                    result.StatusCode = ResultStatusCode.BadRequest;
                    result.ResultError = _errorHandler.GetError(
                        ResultStatusCode.BadRequest,
                        $"El anfitrión requiere al menos {listing.ApprovalRequestDays.Value} días de anticipación para aprobar la reserva.",
                        useTechnicalMessages);
                    return result;
                }
            }

            // 5. Crear la reserva
            CheckinValuesModel checInOutValues = CalculateCheckInOutValues(priceCalculation);
            CheckinValuesModel CancellationValues = CalculateCancellationValues(priceCalculation, (DateTime)checInOutValues.CheckIn);
            TbBook booking = new TbBook
            {
                ListingRentId = priceCalculation.ListingRentId ?? 0,
                UserIdRenter = userId,
                StartDate = (DateTime)priceCalculation.InitBook?.Date,
                EndDate = (DateTime)priceCalculation.EndBook?.Date,
                AmountTotal = priceCalculation.Amount,
                CurrencyId = await _currencyrepository.GetCurrencyId(priceCalculation.CurrencyCode),
                NameRenter = clientData.ContainsKey("name") ? clientData["name"] : string.Empty,
                LastNameRenter = clientData.ContainsKey("lastName") ? clientData["lastName"] : string.Empty,
                TermsAccepted = true, // Asumimos que se aceptaron los términos para llegar aquí
                BookStatusId = 1, // Estado inicial (Prebook)
                Checkin = checInOutValues.CheckIn,
                Checkout = checInOutValues.CheckOut,
                MaxCheckin = checInOutValues.MaxCheckIn,
                CancellationStart = checInOutValues.CancellationStart,
                CancellationEnd = checInOutValues.CancellationEnd,
                RequestDateTime = DateTime.UtcNow
            };

            long bookId = await _bookRepository.UpsertBookAsync(booking);

            if (bookId <= 0)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.Conflict;
                result.ResultError = _errorHandler.GetError(
                    ResultStatusCode.Conflict,
                    "La reserva no ha podido ser creada.",
                    useTechnicalMessages);
                return result;
            }

            booking = await _bookRepository.GetByIdAsync(bookId);

            //Envío de solicitud de aprobación al anfitrión
            _notificationService.SendBookingRequestNotificationAsync(
                listing.OwnerUserId,
                (long)priceCalculation.ListingRentId,
                booking.BookId
            ).Wait();


            return new ReturnModel<TbBook>
            {
                Data = booking,
                StatusCode = ResultStatusCode.OK,
                HasError = false,
            };
        }

        public async Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId)
        {
            var result = await _bookRepository.GetBooksWithoutReviewByUser(userId);
            return result;
        }

        public async Task<TbBook> Cancel(int userId, long bookId)
        {
            var result = await _bookRepository.Cancel(userId, bookId);
            return result;
        }

        public async Task<List<TbBook>> GetPendingAcceptance(int userId)
        {
            var result = await _bookRepository.GetPendingAcceptance(userId);
            return result;
        }

        public async Task<List<TbBook>> GetPendingAcceptanceForRenter(int userId)
        {
            var result = await _bookRepository.GetPendingAcceptanceForRenter(userId);
            return result;
        }

        public async Task<List<TbBook>> GetCancelablesBookings(int userId)
        {
            var result = await _bookRepository.GetCancelablesBookings(userId);
            return result;
        }

        public async Task<List<TbBook>> GetApprovedsWOInit(int userId)
        {
            var result = await _bookRepository.GetApprovedsWOInit(userId);
            return result;
        }

        public async Task<TbBook> AuthorizationResponse(int userId, long bookId, bool isApproval, int? reasonRefused)
        {
            var result = await _bookRepository.AuthorizationResponse(userId, bookId, isApproval, reasonRefused);
            return result;
        }

        public async Task<PayPriceCalculation> ConsultingResponse(int userId, long priceCalculationId, bool isApproval, int? reasonRefused)
        {
            var result = await _payPriceCalculationRepository.ConsultingResponse(userId, priceCalculationId, isApproval, reasonRefused);
            return result;
        }

        private async Task<ReturnModel> CheckAvailability(int listingRentId, DateTime? initBook, DateTime? endBook)
        {
            return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false };
        }

        public async Task CheckAndExpireReservation(DateTime expirationThreshold)
        {
            try
            {
                await _bookRepository.CheckAndExpireReservation(expirationThreshold);

            }
            catch (DbUpdateException dbEx)
            {
                //Guardar en log
                throw;
            }
            catch (Exception ex)
            {
                //Guardar en log
                throw;
            }
        }

        public async Task CheckAndFinishReservation(DateTime expirationThreshold)
        {
            try
            {
                await _bookRepository.CheckAndFinishReservation(expirationThreshold);

            }
            catch (DbUpdateException dbEx)
            {
                //Guardar en log
                throw;
            }
            catch (Exception ex)
            {
                //Guardar en log
                throw;
            }
        }
    }
}
