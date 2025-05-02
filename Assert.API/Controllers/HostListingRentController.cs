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
        /// El proceso de alta de un Listing Rent consta de 3 pasos, los cuales agrupan 16 vistas:<br />
        /// Paso 1: 6 vistas (Subtype, Accomodation Type, Address, Hosts-Bedrooms-Beds, Bathrooms-Stay, Precense)<br />
        /// Paso 2: 5 vistas (Amenities-Security Items, Photos, Title, FeaturedAspects, Description)<br />
        /// Paso 3: 5 vistas (Approval Policy, Pricing, Discounts, Security Confirmation, Review and confirmation)<br />
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/ProcessData")]
        public async Task<ReturnModelDTO<ProcessDataResult>> ProcessData(long? listinRentId, [FromBody] ProcessDataRequest request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<ProcessDataResult> result = await _appListingRentService.ProcessListingData(listinRentId ?? 0, request, requestInfo, request.UseTechnicalMessages);
            return result;
        }


        /// <summary>
        /// Servicio que realiza la subida de las imagenes asociadas a los listing rent.
        /// </summary>
        /// <param name="images">Lista de imágenes .</param>
        /// <returns>Lista de resultados de la subida de los archivos.</returns>
        /// <response code="200">Si se procesó la información de la vista de forma correcta.</response>
        /// <remarks>
        /// En caso de que el resultado de la subbida de los archivos sea exitosa, devuelve el nombre del archivo. 
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
