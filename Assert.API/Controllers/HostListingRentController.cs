using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
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
        ///         {"viewCode": "LV003",  "address": { "zipCode": "0000", "address1": "Av. Heroinas y Belzu", "cityId": 28896, "countyId": 1913,"stateId": 173}}. State: Departamento, County: Provincia <br />
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
        ///     Vista 8 (LV008): Set Set Pricing and Discounts. Se definen los precios y descuentos de la propiedad (Pricing (Pricing, WeekendPrice), Discounts). Los posibles valores de DiscountTypes son retornados en la respuesta exitosa del paso anterior. No es obligatorio registrar descuentos.
        ///     {"viewCode": "LV008", "Discounts": [{"dicountTypeId":1},{"dicountTypeId":1}], "Pricing": 50.50, "WeekendPrice": 70, "CurrencyId": 2 }<br />
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
        /// Servicio que actualiza informacion basica de un Listing Rent
        /// </summary>
        /// <param name="listinRentId">Identificador de Listing Rent</param>
        /// <param name="request">Valores: Titulo - Descripcion - 2 Tipos de aspectos (Id)</param>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/UpdateBasicData")]
        public async Task<ReturnModelDTO<string>> UpdateBasicData(long listinRentId, 
            [FromBody] BasicListingRentData request)
        {
            var result = await _appListingRentService.UpdateBasicData(listinRentId, request);
            return result;
        }

        /// <summary>
        /// Servicio que actualiza informacion de precios de renta y descuentos
        /// </summary>
        /// <param name="listinRentId">Identificador de Listing Rent</param>
        /// <param name="request">Precios de renta para: semana - fin de semana y lista de descuentos</param>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// Si no existe descuentos enviar null el valor de la lista
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/UpdatePricesAndDiscounts")]
        public async Task<ReturnModelDTO<string>> UpdatePricesAndDiscounts(long listinRentId, 
            [FromBody] PricesAndDiscountRequest request)
        {
            var result = await _appListingRentService.UpdatePricesAndDiscounts(listinRentId, request);
            return result;
        }

        ///// <summary>
        ///// Servicio que realiza la subida de las imagenes asociadas a los listing rent.
        ///// </summary>
        ///// <param name="images">Lista de imágenes .</param>
        ///// <returns>Lista de resultados de la subida de los archivos.</returns>
        ///// <response code="200">Si se procesó la información de la vista de forma correcta.</response>
        ///// <remarks>
        ///// En caso de que el resultado de la subida de los archivos sea exitosa, devuelve el nombre del archivo. 
        ///// Los nombres de todos los archivos deben ser enviados al procesar los datos de la vista <br>LV007</br>.
        ///// Se implementó la subida de las imágenes en dos pasos para mejorar la eficiencia de la subida de archivos,
        ///// facilitar las pruebas, mejorar la experiencia de los usuarios, entre otros.
        ///// </remarks>
        //[HttpPost]
        //[Authorize(Policy = "GuestOrHost")]
        //[Route("UploadListingRentImages")]
        //public async Task<List<ReturnModelDTO>> UploadListingRentImages([FromForm] List<IFormFile> images)
        //{
        //    var clientData = HttpContext.GetRequestInfo();
        //    List<ReturnModelDTO> result = await _appListingRentService.UploadImages(images, clientData);
        //    return result;
        //}

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

        /// <summary>
        /// Servicio que realiza la subida del archivo asociada a la foto de un listing rent.
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="images">Lista de imágenes con su sector correspondiente</param>
        /// <returns>Lista de resultados de la subida de los archivos con sus IDs generados</returns>
        /// <response code="200">Si se procesó la información correctamente</response>
        /// <remarks>
        /// Este servicio devuelve una lista con los nombres de los archivos subidos, posteriormente debe  usarse el servicio "{listingRentId}/Photos"
        /// para subir el detalle de las fotos con los respectivos nombres.
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("UploadPhotoImage")]
        public async Task<List<ReturnModelDTO>> UploadPhotosImages(long listingRentId, [FromForm] List<IFormFile> ImageFiles)
        {
            var clientData = HttpContext.GetRequestInfo();
            var result = await _appListingRentService.UploadImages(ImageFiles, clientData);
            return result;
        }

        /// <summary>
        /// Servicio que realiza la subida de una imagen asociada a un listing rent.
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="images">Lista de imágenes con su sector correspondiente</param>
        /// <returns>Lista de resultados de la subida de los archivos con sus IDs generados</returns>
        /// <response code="200">Si se procesó la información correctamente</response>
        [HttpPost]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/Photos")]
        public async Task<List<ReturnModelDTO>> UploadImagesWithSector(long listingRentId, [FromBody] List<UploadImageRequest> imagesDescription)
        {
            var clientData = HttpContext.GetRequestInfo();
            var result = await _appListingRentService.UploadImagesDescription(listingRentId, imagesDescription, clientData);
            return result;
        }

        /// <summary>
        /// Servicio que elimina una foto de un listing rent
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="photoId">ID de la foto a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Si se eliminó correctamente</response>
        /// <response code="404">Si la foto no existe o no pertenece al listing</response>
        [HttpDelete]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/Photos/{photoId}")]
        public async Task<ReturnModelDTO> DeletePhoto(long listingRentId, int photoId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appListingRentService.DeletePhoto(listingRentId, photoId, requestInfo);
            return result;
        }

        /// <summary>
        /// Servicio que actualiza la información de una foto
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="photoId">ID de la foto a actualizar</param>
        /// <param name="request">Nueva información de la foto</param>
        /// <returns>Foto actualizada</returns>
        /// <response code="200">Si se actualizó correctamente</response>
        /// <response code="404">Si la foto no existe o no pertenece al listing</response>
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/Photos/{photoId}")]
        public async Task<ReturnModelDTO<PhotoDTO>> UpdatePhotoSpaceId(
            long listingRentId,
            int photoId,
            [FromBody] UploadImageListingRent request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            var result = await _appListingRentService.UpdatePhoto(listingRentId, photoId, request, requestInfo);
            return result;
        }
    }
}
