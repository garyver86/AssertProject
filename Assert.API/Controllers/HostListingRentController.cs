using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Host/ListingRent")]
    public class HostListingRentController : Controller
    {
        private readonly IAppListingRentService _appListingRentService;
        public HostListingRentController(IAppListingRentService appListingRentService)
        {
            _appListingRentService = appListingRentService;
        }


        /// <summary>
        /// Servicio que realiza el registro/edición de un listing rent.
        /// </summary>
        /// <param name="listinRentId">Id del linsting rent a recuperar.</param>
        /// <param name="request">Información ingresada en la vista correspondiente.</param>
        /// <returns>Datos necesarios para la siguiente vista, si es que corresponde.</returns>
        /// <response code="200">Si se procesó la información de la vista de forma correcta.</response>
        /// <remarks>
        /// El proceso de alta de un Listing Rent consta de 3 pasos, los cuales agrupan 10 vistas:<br />
        /// Paso 1:<br />
        /// 
        ///     Vista 1 (LV001): Set Property Type. Se define el tipo de propiedad (SubtypeId). Los posibles valores de Property Subtype deben ser recuperados del servicio 'Parametrics/PropertyTypes'. En este punto, si se envía el campo "ListingRentId" se actualiza la propiedad, si no se envía se crea una nueva.
        ///         {"viewCode": "LV001",  "subtypeId": 6} <br />
        ///     Vista 2 (LV002): Set Accomodation Type. Se define el tipo de alojamiento (AccomodationId). Los posibles valores de AccomodationType son retornados en la respuesta exitosa del paso anterior.
        ///         {"viewCode": "LV002",  "AccomodationId": 1} <br />
        ///     Vista 3 (LV003): Set Address. Se define la dirección de la propiedad (Address(Address1, CityId, ZipCode)). Los posibles valores de CityId son retornados en la respuesta exitosa del paso anterior.
        ///         {"viewCode": "LV003",  "address": { "zipCode": "0000", "address1": "Av. Heroinas y Belzu", "cityId": 28896}} <br />
        ///     Vista 4 (LV004): Set Location. Se define la ubicación de la propiedad (Address(Latitude, Longitude)).
        ///         {"viewCode": "LV004",  "address": { "latitude": "-17.38981233345421", "longitude": "-66.14371647547648"}} <br />
        ///     Vista 5 (LV005): Set Capacity. Se Define la capicidad de la propiedad (Bedrooms, Beds, Bathrooms, y MaxGuests).
        ///         {"viewCode": "LV005", "maxGuests": 4, "bathrooms": 2, "bedrooms": 3, "beds": 3 } <br />
        /// Paso 2:<br />
        ///     Vista 6 (LV006): Set Amenities. Se definen los amenities de la propiedad (Amenities). Los posibles valores de Amenities son retornados en la respuesta exitosa del paso anterior.
        ///         {"viewCode": "LV006", "Amenities": [1,2,3,4,5] } <br />
        ///     Vista 7 (LV007): Set Attributes. Se definen los atributos de la propiedad (Title, Description, FeaturedAspects). Los posibles valores de FeaturedAspects son retornados en la respuesta exitosa del paso anterior.
        ///         {"viewCode": "LV007", "FeaturedAspects": [1,2,3], "Title": "Casa centrica a pasos del cristo", "Description": "Propiedad en pleno centro de la ciudad." } <br />
        /// Paso 3:<br />
        ///     Vista 8 (LV008): Set Set Pricing and Discounts. Se definen los precios y descuentos de la propiedad (Pricing (Pricing, WeekendPrice), Discounts). Los posibles valores de DiscountTypes son retornados en la respuesta exitosa del paso anterior. No es obligatorio registrar descuentos.<br />
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/ProcessData")]
        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessData(long? listinRentId, [FromBody] ProcessDataRequest request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<ProcessDataResult> result = await _appListingRentService.ProcessListingData(listinRentId ?? 0, request, requestInfo, true);
            return result;
        }


        /// <summary>
        /// Servicio que realiza la subida de las imagenes asociadas a los listing rent.
        /// </summary>
        /// <param name="images">Lista de imágenes .</param>
        /// <returns>Lista de resultados de la subida de los archivos.</returns>
        /// <response code="200">Si se procesó la información de la vista de forma correcta.</response>
        /// <remarks>
        /// En caso de que el resultado de la subida de los archivos sea exitosa, devuelve el nombre del archivo. 
        /// Los nombres de todos los archivos deben ser enviados al procesar los datos de la vista <br>LV007</br>.
        /// Se implementó la subida de las imágenes en dos pasos para mejorar la eficiencia de la subida de archivos,
        /// facilitar las pruebas, mejorar la experiencia de los usuarios, entre otros.
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("UploadListingRentImages")]
        public async Task<List<ReturnModelDTO>> UploadListingRentImages([FromForm] List<IFormFile> images)
        {
            var clientData = HttpContext.GetRequestInfo();
            List<ReturnModelDTO> result = await _appListingRentService.UploadImages(images, clientData);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve los listing rent del propietario actualmente logueado.
        /// </summary>
        /// <returns>Listado de listing rents (lropiedades).</returns>
        /// <response code="200">Si no existió un error al devolver las propiedades.</response>
        /// <remarks>
        /// No se devolveran los listing rents que se encuentren marcados como eliminados.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("")]
        public async Task<ReturnModelDTO<List<ListingRentDTO>>> Get()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ListingRentDTO>> result = await _appListingRentService.GetByOwner(requestInfo, true);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve los listing rent del propietario actualmente logueado.
        /// </summary>
        /// <returns>Listado de listing rents (lropiedades).</returns>
        /// <response code="200">Si no existió un error al devolver las propiedades.</response>
        /// <remarks>
        /// No se devolveran los listing rents que se encuentren marcados como eliminados.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/Photos")]
        public async Task<ReturnModelDTO<List<PhotoDTO>>> GetImages(long listinRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<PhotoDTO>> result = await _appListingRentService.GetPhotoByListigRent(listinRentId, requestInfo, true);
            return result;
        }
    }
}
