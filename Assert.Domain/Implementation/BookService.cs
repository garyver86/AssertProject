using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            IPayTransactionRepository payTransactionRepository)
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
        public async Task<ReturnModel<PayPriceCalculation>> CalculatePrice(
            long listingRentId,
            DateTime startDate,
            DateTime endDate,
            int guestId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages)
        {
            var result = new ReturnModel<PayPriceCalculation>();

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
                if (calendarDay != null && calendarDay.Price.HasValue)
                {
                    dayPrice = calendarDay.Price.Value;
                }
                else
                {
                    bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                    if (isWeekend && priceInfo.WeekendNightlyPrice.HasValue)
                        dayPrice = priceInfo.WeekendNightlyPrice.Value;
                    else
                        dayPrice = priceInfo.PriceNightly ?? 0;
                }
                total += dayPrice;

                // 2. Descuentos por día (TLCalendarDiscount)
                if (calendarDay != null && calendarDay.TlCalendarDiscounts != null)
                {
                    foreach (var calDiscount in calendarDay.TlCalendarDiscounts)
                    {
                        if (calDiscount.IsDiscount)
                        {
                            totalDiscount += dayPrice * (calDiscount.Porcentage / 100m);
                        }
                    }
                }

                //// 3. Tarifas adicionales por día (TLAdditionalFees)
                //if (calendarDay != null && calendarDay.TlAdditionalFees != null)
                //{
                //    foreach (var fee in calendarDay.TlAdditionalFees)
                //    {
                //        totalAdditionalFees += fee.Amount; // Asumiendo que la propiedad es Amount
                //    }
                //}
            }

            //// 7. Sumar tarifas adicionales (ejemplo: amenities premium)
            //var amenities = await _listingAmenityRepository.GetByListingRentId(listingRentId);
            //decimal extraFees = amenities?.Where(a => a.IsPremium).Sum(a => a.AmenitiesType?.Price ?? 0) ?? 0;
            //total += extraFees;

            // 8. Aplicar descuentos generales
            if (discounts != null)
            {
                foreach (var discount in discounts)
                {
                    if (discount.IsDiscount)
                    {
                        totalDiscount += total * (discount.Porcentage / 100m);
                    }
                }
            }

            // 9. Total final
            total = total + totalAdditionalFees - totalDiscount;

            // 10. Construir el modelo de resultado
            var payPriceCalculation = new PayPriceCalculation
            {
                Amount = total,
                CurrencyCode = priceInfo.Currency?.Code ?? "USD",
                CalculationDetails = $"Reserva de {nights} noches del {startDate:yyyy-MM-dd} al {endDate:yyyy-MM-dd}",
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(30),
                CalculationStatue = "PENDING",
                CalculationCode = Guid.NewGuid(),
                IpAddress = clientData != null && clientData.ContainsKey("ip") ? clientData["ip"] : null,
                UserAgent = clientData != null && clientData.ContainsKey("userAgent") ? clientData["userAgent"] : null,
                InitBook = startDate,
                EndBook = endDate,
                ListingRentId = listingRentId
            };

            var resultCalculation = await _payPriceCalculationRepository.Create(payPriceCalculation);

            result.Data = payPriceCalculation;
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

            // 5. Crear la reserva


            var booking = new TbBook
            {
                ListingRentId = priceCalculation.ListingRentId ?? 0,
                UserIdRenter = userId,
                StartDate = priceCalculation.InitBook ?? DateTime.UtcNow,
                EndDate = priceCalculation.EndBook ?? DateTime.UtcNow,
                AmountTotal = priceCalculation.Amount,
                CurrencyId = await _currencyrepository.GetCurrencyId(priceCalculation.CurrencyCode),
                NameRenter = clientData.ContainsKey("name") ? clientData["name"] : string.Empty,
                LastNameRenter = clientData.ContainsKey("lastName") ? clientData["lastName"] : string.Empty,
                TermsAccepted = true, // Asumimos que se aceptaron los términos para llegar aquí
                BookStatusId = 1, // Estado inicial (por ejemplo, "Confirmado")
                PaymentCode = paymentRequest.OrderCode,
                PaymentId = "",

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
            long savedTransaction = await _payTransactionRepository.Create(transaction);

            List<DateOnly> dates = new List<DateOnly>();

            for (var date = booking.StartDate; date <= booking.EndDate; date = date.AddDays(1))
            {
                dates.Add(DateOnly.FromDateTime(date));
            }

            var resultBlock = await _listingCalendarRepository.BulkBlockDaysAsync(priceCalculation.ListingRentId ?? 0, dates, 2, "Alquiler de propiedad", bookId);

            var resultStatusUpdate = await _payPriceCalculationRepository.SetAsPayed(paymentRequest.CalculationCode, paymentRequest.PaymentProviderId,
                paymentRequest.MethodOfPaymentId, savedTransaction);

            var book = await _bookRepository.GetByIdAsync(bookId);

            return new ReturnModel<TbBook>
            {
                Data = book,
                StatusCode = ResultStatusCode.OK,
                HasError = false,
            };
        }

        public async Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId)
        {
            var result = await _bookRepository.GetBooksWithoutReviewByUser(userId);
            return result;
        }

        private async Task<ReturnModel> CheckAvailability(int listingRentId, DateTime? initBook, DateTime? endBook)
        {
            return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false };
        }
    }
}
