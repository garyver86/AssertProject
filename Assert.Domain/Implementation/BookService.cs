using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public BookService(
            IListingRentRepository listingRentRepository,
            IListingCalendarRepository listingCalendarRepository,
            IListingPricingRepository listingPricingRepository,
            IListingDiscountRepository listingDiscountRepository,
            IErrorHandler errorHandler,
            IPayPriceCalculationRepository payPriceCalculationRepository,
            IListingDiscountForRateRepository listingDiscountForRateRepository)
        {
            _listingRentRepository = listingRentRepository;
            _listingCalendarRepository = listingCalendarRepository;
            _listingPricingRepository = listingPricingRepository;
            _listingDiscountRepository = listingDiscountRepository;
            _errorHandler = errorHandler;
            _payPriceCalculationRepository = payPriceCalculationRepository;
            _listingDiscountForRateRepository = listingDiscountForRateRepository;
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
            var listing = await _listingRentRepository.Get(listingRentId, onlyActive: true);
            if (listing == null)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.NotFound;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.NotFound, "La propiedad no existe o no está activa.", useTechnicalMessages);
                return result;
            }
            else if (listing.ListingStatusId != 3)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La propiedad no está disponible para reservas.", useTechnicalMessages);
                return result;
            }

            // 2. Validar fechas
            if (startDate >= endDate)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La fecha de inicio debe ser menor a la fecha de fin.", useTechnicalMessages);
                return result;
            }
            if (startDate < DateTime.Today)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "No se pueden reservar fechas en el pasado.", useTechnicalMessages);
                return result;
            }

            // 3. Validar conflicto con reservas o bloqueos
            var (calendarDays, _) = await _listingCalendarRepository.GetCalendarDaysAsync(
                (int)listingRentId, startDate, endDate, 1, (int)(endDate - startDate).TotalDays + 1);

            if (calendarDays.Any(d => d.BlockType != null))
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.Conflict;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.Conflict, "Las fechas seleccionadas no están disponibles.", useTechnicalMessages);
                return result;
            }

            // 4. Validar mínimo y máximo de noches
            int nights = (endDate - startDate).Days;
            if (listing.MinimunStay.HasValue && nights < listing.MinimunStay.Value)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La estancia mínima es de {listing.MinimunStay.Value} noches.", useTechnicalMessages);
                return result;
            }
            if (listing.MaximumStay.HasValue && nights > listing.MaximumStay.Value)
            {
                result.HasError = true;
                result.StatusCode = ResultStatusCode.BadRequest;
                result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La estancia máxima es de {listing.MaximumStay.Value} noches.", useTechnicalMessages);
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
            };

            var resultCalculation = await _payPriceCalculationRepository.Create(payPriceCalculation);

            result.Data = payPriceCalculation;
            result.StatusCode = ResultStatusCode.OK;
            result.HasError = false;
            return result;
        }
        //public async Task<ReturnModel<PayPriceCalculation>> CalculatePrice(
        //    long listingRentId,
        //    DateTime startDate,
        //    DateTime endDate,
        //    int guestId,
        //    Dictionary<string, string> clientData,
        //    bool useTechnicalMessages)
        //{
        //    var result = new ReturnModel<PayPriceCalculation>();

        //    // 1. Validar existencia y estado de la propiedad
        //    var listing = await _listingRentRepository.Get(listingRentId, onlyActive: true);
        //    if (listing == null)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.NotFound;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.NotFound, "La propiedad no existe o no está activa.", useTechnicalMessages);
        //        return result;
        //    }
        //    else if (listing.ListingStatusId != 3)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La propiedad no está disponible para reservas.", useTechnicalMessages);
        //        return result;
        //    }

        //    // 2. Validar fechas
        //    if (startDate >= endDate)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La fecha de inicio debe ser menor a la fecha de fin.", useTechnicalMessages);
        //        return result;
        //    }
        //    if (startDate < DateTime.Today)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "No se pueden reservar fechas en el pasado.", useTechnicalMessages);
        //        return result;
        //    }

        //    // 3. Validar conflicto con reservas o bloqueos
        //    var (calendarDays, _) = await _listingCalendarRepository.GetCalendarDaysAsync(
        //        (int)listingRentId, startDate, endDate, 1, (int)(endDate - startDate).TotalDays + 1);

        //    if (calendarDays.Any(d => d.BlockType != null))
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.Conflict;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.Conflict, "Las fechas seleccionadas no están disponibles.", useTechnicalMessages);
        //        return result;
        //    }

        //    // 4. Validar mínimo y máximo de noches
        //    int nights = (endDate - startDate).Days;
        //    if (listing.MinimunStay.HasValue && nights < listing.MinimunStay.Value)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La estancia mínima es de {listing.MinimunStay.Value} noches.", useTechnicalMessages);
        //        return result;
        //    }
        //    if (listing.MaximumStay.HasValue && nights > listing.MaximumStay.Value)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La estancia máxima es de {listing.MaximumStay.Value} noches.", useTechnicalMessages);
        //        return result;
        //    }

        //    // 5. Calcular precio base
        //    var priceInfo = await _listingPricingRepository.GetByListingRent(listingRentId);
        //    if (priceInfo == null || !priceInfo.PriceNightly.HasValue)
        //    {
        //        result.HasError = true;
        //        result.StatusCode = ResultStatusCode.BadRequest;
        //        result.ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "No se encontró información de precios para la propiedad.", useTechnicalMessages);
        //        return result;
        //    }

        //    decimal total = 0;
        //    for (var date = startDate; date < endDate; date = date.AddDays(1))
        //    {
        //        // Si es fin de semana y hay precio especial, usarlo
        //        bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        //        if (isWeekend && priceInfo.WeekendNightlyPrice.HasValue)
        //        {
        //            total += priceInfo.WeekendNightlyPrice.Value;
        //        }
        //        else
        //        {
        //            total += priceInfo.PriceNightly.Value;
        //        }
        //    }

        //    // 6. Aplicar descuentos (si existen)
        //    // Aquí podrías consultar los descuentos y aplicarlos según la lógica de negocio

        //    // 7. Sumar tarifas adicionales (ejemplo: limpieza, servicio, etc.)
        //    // Aquí podrías sumar tarifas adicionales si existen

        //    // 8. Construir el modelo de resultado
        //    var payPriceCalculation = new PayPriceCalculation
        //    {
        //        Amount = total,
        //        CurrencyCode = priceInfo.Currency?.Code ?? "USD",
        //        CalculationDetails = $"Reserva de {nights} noches del {startDate:yyyy-MM-dd} al {endDate:yyyy-MM-dd}",
        //        CreationDate = DateTime.UtcNow,
        //        ExpirationDate = DateTime.UtcNow.AddMinutes(30),
        //        CalculationStatue = "PENDING",
        //        CalculationCode = Guid.NewGuid(),
        //        IpAddress = clientData != null && clientData.ContainsKey("ip") ? clientData["ip"] : null,
        //        UserAgent = clientData != null && clientData.ContainsKey("userAgent") ? clientData["userAgent"] : null,
        //    };

        //    var resultCalculation = await _payPriceCalculationRepository.Create(payPriceCalculation);

        //    result.Data = payPriceCalculation;
        //    result.StatusCode = ResultStatusCode.OK;
        //    result.HasError = false;
        //    return result;
        //}
    }
}
