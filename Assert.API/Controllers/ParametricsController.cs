using Assert.API.Helpers;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ParametricsController : Controller
    {
        private readonly IAppSearchService _searchService;
        private readonly IAppParametricService _parametricService;
        private readonly IAppReviewService _reviewService;
        public ParametricsController(IAppSearchService searchService, IAppParametricService parametricService, IAppReviewService reviewService)
        {
            _searchService = searchService;
            _parametricService = parametricService;
            _reviewService = reviewService;
        }

        /// <summary>
        /// Servicio que permite la busqueda de ciudades.
        /// </summary>
        /// <param name="filter">Texto que permite filtrar las ciudades (Mínimo 3 caracteres).</param>
        /// <returns>Listado de ciudades que coinciden con la búsqueda.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran las ciudades que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("City/{filter}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<CountryDTO>>> Search(string filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var cities = await _searchService.SearchCities(filter, 4, requestInfo, true);

            return cities;
        }

        /// <summary>
        /// Servicio que permite la busqueda de Localizaciones geograficas en base a parametros.
        /// </summary>
        ///  /// <param name="typeFilter">Texto que permite definir el tipo de localización que se quiere buscar("city,county,state,country").</param>
        /// <param name="filter">Texto que permite filtrar las localizaciones (Mínimo 3 caracteres).</param>
        /// <returns>Listado de localizaciones que coinciden con la búsqueda.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [HttpGet("Locations/{typeFilter}/{filter}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<CountryDTO>>> SearchLocation(string typeFilter, string filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int _filterType = 0;
            switch (typeFilter)
            {
                case "city":
                    _filterType = 4;
                    break;
                case "county":
                    _filterType = 3;
                    break;
                case "state":
                    _filterType = 2;
                    break;
                case "country":
                    _filterType = 1;
                    break;
                case "all":
                    _filterType = 0;
                    break;
                default:
                    return new ReturnModelDTO<List<CountryDTO>>()
                    {
                        HasError = true,
                        StatusCode = "400",
                        ResultError = new ErrorCommonDTO
                        {
                            Code = "400",
                            Message = "El tipo de filtro no es válido."
                        }
                    };
            }
            var cities = await _searchService.SearchCities(filter, _filterType, requestInfo, true);

            return cities;
        }

        /// <summary>
        /// Servicio que devuelve una lista de sugerencias de locations en base a un filtro ingresado.
        /// </summary>
        /// <param name="filter">Texto que permite filtrar las localizaciones (Mínimo 3 caracteres).</param>
        /// <returns>Listado de localizaciones que coinciden con la búsqueda.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// </remarks>
        [HttpGet("Locations/Suggest/{filter}")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<LocationSuggestion>>> SuggestLocation(string filter)
        {
            var requestInfo = HttpContext.GetRequestInfo();

            var cities = await _searchService.SuggestLocation(filter, requestInfo, true);

            return cities;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de alojamientos activos.
        /// </summary>
        /// <returns>Listado de tipos de alojamientos.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de alojamiento que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("AccomodationTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<AccomodationTypeDTO>>> AccomodationTypes()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var cities = await _parametricService.GetAccomodationTypes(requestInfo, true);

            return cities;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de amenities o servicios activos.
        /// </summary>
        /// <returns>Listado de tipos de amenities.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de amenities que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("AmenityTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<AmenityDTO>>> AmenityTypes()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<AmenityDTO>> amenities = await _parametricService.GetAmenityTypes(requestInfo, true);

            return amenities;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de reservación.
        /// </summary>
        /// <returns>Listado de tipos de amenities.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de amenities que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("ReservationTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<ApprovalPolicyDTO>>> ReservationTypes()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ApprovalPolicyDTO>> amenities = await _parametricService.GetReservationTypes(requestInfo, true);

            return amenities;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de alojamientos activos.
        /// </summary>
        /// <returns>Listado de tipos de alojamientos.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de alojamiento que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("PropertyTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<PropertyTypeDTO>>> PropertyTypes()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var propertyTypes = await _parametricService.GetPropertyTypes(requestInfo, true);

            return propertyTypes;
        }

        /// <summary>
        /// Servicio que devuelve la lista de aspectos destacados.
        /// </summary>
        /// <returns>Listado de aspectos destacados.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos aspectos destacados que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("FeaturedAspects")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<FeaturedAspectDTO>>> FeaturedAspects()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var featuredAspectssult = await _parametricService.GetFeaturedAspects(requestInfo, true);

            return featuredAspectssult;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de descuento.
        /// </summary>
        /// <returns>Listado de tipos de descuentos.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos aspectos destacados que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("DiscountTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<DiscountDTO>>> DiscountTypes()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var discountTypesResult = await _parametricService.GetDiscountTypes(requestInfo, true);

            return discountTypesResult;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de espacios en una propiedad.
        /// </summary>
        /// <returns>Listado de tipos de espacios.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de espacios que no se encuentren deshabilitadas.
        /// </remarks>
        [HttpGet("SpaceType")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<DiscountDTO>>> SpaceType()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var discountTypesResult = await _parametricService.GetDiscountTypes(requestInfo, true);

            return discountTypesResult;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de lenguajes.
        /// </summary>
        /// <returns>Listado de tipos de lenguajes.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los tipos de lenguajes habilitados.
        /// </remarks>
        [HttpGet("GetLanguageTypes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<LanguageDTO>>> GetLanguageTypes()
        => await _parametricService.GetLanguageTypes();

        /// <summary>
        /// Servicio que devuelve la lista tipos de monedas disponibles.
        /// </summary>
        /// <returns>Listado de tipso de monedas</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran todos los tipos de monedas registradas
        /// </remarks>
        [HttpGet("GetCurrencyTipes")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<CurrencyDTO>>> GetCurrencyTipes()
        => await _parametricService.GetCurrencies();

        /// <summary>
        /// Servicio que devuelve la lista de políticas de cancelación.
        /// </summary>
        /// <returns>Listado de políticas de cancelación.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran las políticas habilitadas.
        /// </remarks>
        [HttpGet("CancellationPolicies")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<CancellationPolicyDTO>>> GetCancellationPolicies()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var discountTypesResult = await _parametricService.GetCancellationPolicies(requestInfo, true);

            return discountTypesResult;
        }


        /// <summary>
        /// Servicio que devuelve la lista de items de seguridad.
        /// </summary>
        /// <returns>Listado de items de seguridad.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran los items de seguridad activos.
        /// </remarks>
        [HttpGet("SecurityItems")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<SecurityItemDTO>>> SecurityItems()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var SecurityItemDTO = await _parametricService.GetSecurityItems(requestInfo, true);

            return SecurityItemDTO;
        }

        /// <summary>
        /// Servicio que devuelve la lista de tipos de reglas.
        /// </summary>
        /// <returns>Listado de reglas aplicables al alquiler de una propiedad.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo se consideraran las reglas habilitadas.
        /// </remarks>
        [HttpGet("RentRules")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<RentRuleDTO>>> GetRentRules()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var discountTypesResult = await _parametricService.GetRentRules(requestInfo, true);

            return discountTypesResult;
        }

        /// <summary>
        /// Servicio que devuelve la lista de preguntas para realizar un review.
        /// </summary>
        /// <returns>Listado de preguntas para realizar reviews a una reserva.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        [HttpGet("ReviewQuestions")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO<List<ReviewQuestionDTO>>> GetReviewQuestions()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var discountTypesResult = await _reviewService.GetReviewQuestions(requestInfo);

            return discountTypesResult;
        }
    }
}
