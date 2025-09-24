using Assert.API.Helpers;
using Assert.API.Middleware;
using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Azure.Core;
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
        private readonly RequestMetadata _metadata;
        public HostListingRentController(IAppListingRentService appListingRentService, RequestMetadata metadata)
        {
            _appListingRentService = appListingRentService;
            _metadata = metadata;
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
        ///     {"viewCode": "LV008", "Discounts": [{"dicountTypeId":1, "Price": 200.10},{"dicountTypeId":1, , "Price": 200.50}], "Pricing": 50.50, "WeekendPrice": 70, "CurrencyId": 2 }<br />
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
        /// Servicio que devuelve el paso de registro no finalizado de una propiedad
        /// </summary>
        /// <param name="listinRentId">Identificador de Listing Rent</param>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// Si no existe descuentos enviar null el valor de la lista
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/ProcessData/Continue")]
        public async Task<ReturnModelDTO<ProcessDataResult>> GetLastView(long listinRentId)
        {

            var result = await _appListingRentService.GetLastView(listinRentId, _metadata.UserId);
            return result;
        }
       
        /// <summary>
        /// Servicio que devuelve la lista de propiedades cuyo registro no ha finalizado
        /// </summary>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// Si no existe descuentos enviar null el valor de la lista
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("ProcessData/Unfinished")]
        public async Task<ReturnModelDTO<List<ListingRentDTO>>> GetUnfinished()
        {
            var result = await _appListingRentService.GetUnfinished(_metadata.UserId);
            return result;
        }

        /// <summary>
        /// Servicio que actualiza informacion de precios de renta y descuentos
        /// </summary>
        /// <param name="listinRentId">Identificador de Listing Rent</param>
        /// <param name="request">Precios de renta para: semana - fin de semana y lista de descuentos. Valores posibles para "discountCode" son: "week - month" dentro la lista</param>
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
        /// Servicio que devuelve los listing rents publicados del propietario junto con la información de reservas y dís bloqueados.
        /// </summary>
        /// <returns>Listado de listing rents (propiedades) publicadas masreservas y días bloqueados.</returns>
        /// <response code="200">Si no existió un error al devolver las propiedades.</response>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("CalendarInfo")]
        public async Task<ReturnModelDTO<List<ListingRentCalendarDTO>>> GetCalendar()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ListingRentCalendarDTO>> result = await _appListingRentService.GetCalendarByOwner(requestInfo, true);
            return result;
        }

        /// <summary>
        /// Servicio que devuelve los listing rent publicados del propietario actualmente logueado con informacion
        /// resumida (coordenadas, precios y moneda).
        /// </summary>
        /// <returns>Listado de listing rents (lropiedades).</returns>
        /// <response code="200">Si no existió un error al devolver las propiedades.</response>
        /// <remarks>
        /// No se devolveran los listing rents que se encuentren marcados como eliminados.
        /// </remarks>
        [HttpGet]
        [Authorize(Policy = "GuestOrHost")]
        [Route("Resume")]
        public async Task<ReturnModelDTO<List<ListingRentResumeDTO>>> GetResume()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            ReturnModelDTO<List<ListingRentResumeDTO>> result = await _appListingRentService.GetByOwnerResumed(requestInfo, true);
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

        /// <summary>
        /// Servicio que devuelve el detalle de un Listing Rent
        /// </summary>
        /// <param name="listinRentId">Id del linsting rent a recuperar.</param>
        /// <returns>Detalle de un listing rent.</returns>
        /// <response code="200">Detalle del Listing Rent</response>
        /// <remarks>
        /// Este servicio devuelve el detalle de un listing rent en base a su id, al tratarse de un servicio para Host, valida que el dueño este accediendo al listing y se lo devuelve en cualquier estado.
        /// </remarks>
        [HttpGet("{listinRentId}")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> Get(long listinRentId)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);

            var result = await _appListingRentService.Get(listinRentId, userId, requestInfo, true);
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
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listinRentId}/UpdateBasicData")]
        public async Task<ReturnModelDTO<string>> UpdateBasicData(
            [FromRoute] long listinRentId,
            [FromBody] BasicListingRentData request)
        => await _appListingRentService.UpdateBasicData(listinRentId, request);

        /// <summary>
        /// Servicio que actualiza nombre y descripcion de Listing Rent
        /// </summary>
        /// <param name="listingRentId">Identificador de Listing Rent</param>
        /// <param name="request">Valores: Titulo - Descripcion</param>
        /// <returns></returns>
        /// <response code="200">El valor de Data seria UPDATED</response>
        /// <remarks>
        /// </remarks>
        [HttpPut]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateNameAndDescription")]
        public async Task<ReturnModelDTO<string>> UpdateNameAndDescription(
            [FromRoute] long listingRentId,
            [FromBody] BasicListingRentDataBase request)
        => await _appListingRentService.UpdateBasicData(listingRentId,
                new BasicListingRentData
                {
                    Title = request.Title,
                    Description = request.Description,
                    AspectTypeIdList = null
                });

        /// <summary>
        /// Servicio que actualiza los tipos de propiedad y alojamiento de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID de listing rent</param>
        /// <param name="propertyTypeId">ID de property type</param>
        /// <param name="accomodationTypeId">ID de accommodation type</param>
        /// <returns>UPDATED de un listing rent.</returns>
        /// <response code="200">Detalle del Listing Rent</response>
        /// <remarks>
        /// Como prerequisito ya deberia haber sido creado el listing rent y se deberian haber definido los tipos de propiedad y alojamiento.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdatePropertyAndAccomodationTypes")]
        public async Task<ReturnModelDTO> UpdatePropertyAndAccomodationTypes([FromRoute] long listingRentId,
            int propertyTypeId, int accomodationTypeId)
        => await _appListingRentService.UpdatePropertyAndAccomodationTypes(listingRentId, propertyTypeId, accomodationTypeId);

        /// <summary>
        /// Servicio que actualiza la capacidades de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID de listing rent</param>
        /// <param name="beds">Número de camas</param>
        /// <param name="bedrooms">Número de habitaciones</param>
        /// <param name="bathrooms">Número de baños</param>
        /// <param name="privateBathroom">Cantidad de baños privados disponibles</param>
        /// <param name="privateBathroomLodging">Cantidad de baños privados exclusivos del alojamiento</param>
        /// <param name="sharedBathroom">Cantidad de baños compartidos</param>
        /// <param name="maxGuests">Máximo número de huéspedes</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent</response>
        /// <remarks>
        /// Como prerequisito ya debería haber sido creado el listing rent.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateCapacity")]
        public async Task<ReturnModelDTO> UpdateCapacity([FromRoute] long listingRentId,
            int beds, int bedrooms, int bathrooms, int maxGuests,
            int privateBathroom, int privateBathroomLodging, int sharedBathroom)
        => await _appListingRentService.UpdateCapacity(listingRentId, beds, bedrooms,
            bathrooms, privateBathroom, privateBathroomLodging, sharedBathroom, maxGuests);

        /// <summary>
        /// Servicio que actualiza la ubicación y dirección de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="cityId">ID de la ciudad</param>
        /// <param name="countyId">ID del condado</param>
        /// <param name="stateId">ID del estado/provincia</param>
        /// <param name="latitude">Coordenada de latitud</param>
        /// <param name="longitude">Coordenada de longitud</param>
        /// <param name="address1">Dirección principal (calle y número)</param>
        /// <param name="address2">Dirección secundaria (departamento, piso, etc.)</param>
        /// <param name="zipCode">Código postal</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent actualizado</response>
        /// <remarks>
        /// Como prerequisito ya debería haber sido creado el listing rent.
        /// en caso de no tener algun dato solo enviar 0 en caso de entero y vacio en caso de string.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdatePropertyLocation")]
        public async Task<ReturnModelDTO> UpdatePropertyLocation([FromRoute] long listingRentId,
            int cityId, int countyId, int stateId, double Latitude, double Longitude,
            string address1, string address2, string zipCode, string Country, string State, string County, string City, string Street)
        => await _appListingRentService.UpdatePropertyLocation(listingRentId,
            cityId, countyId, stateId, Latitude, Longitude, address1, address2, zipCode, Country, State, County, City, Street);

        /// <summary>
        /// Actualiza las características, amenidades y elementos de seguridad de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID de listing rent</param>
        /// <param name="request">DTO con los datos para actualizar las características</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Retorna el listing con las características actualizadas exitosamente</response>
        /// <remarks>
        /// <para><strong>Requisitos:</strong></para>
        /// <para>- El listing rent debe existir previamente en el sistema</para>
        /// <para>- Los IDs proporcionados deben existir en la base de datos</para>
        /// <para><strong>Comportamiento:</strong></para>
        /// <para>- Solo se actualizan las listas que contengan valores (las listas vacías se ignoran)</para>
        /// <para>- Las listas se inicializan vacías por defecto para evitar valores nulos</para>
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateCharacteristics")]
        public async Task<ReturnModelDTO> UpdateCharacteristics([FromRoute] long listingRentId,
            [FromBody] UpdateCharasteristicsRequestDTO request)
        {
            var requestInfo = HttpContext.GetRequestInfo();
            return await _appListingRentService.UpdateCharasteristics(listingRentId,
                requestInfo, request.FeaturedAmenities, request.FeatureAspects, request.SecurityItems);
        }

        /// <summary>
        /// Servicio que actualiza la política de cancelación de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="cancellationPolicyId">ID de la política de cancelación a aplicar</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent con la política de cancelación actualizada</response>
        /// <remarks>
        /// Requisitos:
        /// - El listing rent debe existir previamente
        /// - La política de cancelación debe existir en el sistema
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateCancellationPolicy")]
        public async Task<ReturnModelDTO> UpdateCancellationPolicy([FromRoute] long listingRentId,
            int cancellationPolicyId)
        => await _appListingRentService.UpdateCancellationPolicy(listingRentId, cancellationPolicyId);

        /// <summary>
        /// Servicio que actualiza la configuración de reservas de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del listing rent</param>
        /// <param name="approvalPolicyTypeId">ID del tipo de política de aprobación</param>
        /// <param name="minimunNoticeDays">Días mínimos de anticipación requeridos para reservar</param>
        /// <param name="preparationDays">Días de preparación requeridos entre reservas</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent con la configuración de reservas actualizada</response>
        /// <remarks>
        /// Requisitos:
        /// - El listing rent debe existir previamente
        /// - Los valores numéricos deben ser enteros no negativos
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateReservation")]
        public async Task<ReturnModelDTO> UpdateReservation([FromRoute] long listingRentId,
            int approvalPolicyTypeId, int minimunNoticeDays, int preparationDays)
        => await _appListingRentService.UpdateReservation(listingRentId,
            approvalPolicyTypeId, minimunNoticeDays, preparationDays);

        /// <summary>
        /// Servicio que actualiza la configuración de precios y descuentos de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del Listing Rent al que se aplican los cambios</param>
        /// <param name="request">Datos de precios y descuentos que se desean actualizar</param>
        /// <returns>UPDATE</returns>
        /// <response code="200">Detalle del Listing Rent con precios y descuentos actualizados</response>
        /// <remarks>
        /// Requisitos:
        /// - El Listing Rent debe existir previamente.
        /// - Los valores numéricos deben ser válidos según las reglas del dominio.
        /// - Las fechas, si están presentes, deben ser consistentes y no superponerse.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdatePricingAndDiscounts")]
        public async Task<ReturnModelDTO> UpdatePricingAndDiscounts([FromRoute] long listingRentId,
            [FromBody] PricesAndDiscountRequest request)
        => await _appListingRentService.UpdatePricesAndDiscounts(listingRentId, request);

        /// <summary>
        /// Servicio que actualiza los horarios de check-in/check-out y las reglas de estadía para un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del Listing Rent que se desea actualizar</param>
        /// <param name="request">Datos con la nueva configuración de horarios, instrucciones y reglas</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent con la configuración de check-in/out y reglas actualizadas</response>
        /// <remarks>
        /// Requisitos:
        /// - El Listing Rent debe existir previamente.
        /// - Los horarios deben estar en formato válido (por ejemplo, HH:mm) y respetar la lógica de entrada/salida.
        /// - Las instrucciones pueden ser vacio.
        /// - Los IDs de reglas deben referenciar reglas existentes en el sistema, pero en caso de no tener reglas se puede enviar null o lista vacia.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateCheckInOutAndRules")]
        public async Task<ReturnModelDTO> UpdateCheckInOutAndRules([FromRoute] long listingRentId,
            [FromBody] CheckInOutAndRulesRequestDTO request)
        => await _appListingRentService.UpdateCheckInOutAndRules(listingRentId, 
            request.CheckInTime, request.CheckOutTime, request.MaxCheckInTime, 
            request.Instructions, request.RuleIds);

        /// <summary>
        /// Servicio que actualiza las reglas de estadía asociadas a un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del Listing Rent cuyo conjunto de reglas se desea modificar</param>
        /// <param name="ruleIds">Lista de IDs de las reglas que se deben aplicar. Puede ser null o vacía si no se desea asignar reglas</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent con las reglas actualizadas</response>
        /// <remarks>
        /// Requisitos:
        /// - El Listing Rent debe existir previamente.
        /// - Los IDs deben referenciar reglas válidas definidas en el sistema.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdateRules")]
        public async Task<ReturnModelDTO> UpdateRules([FromRoute] long listingRentId,
            List<int> ruleIds)
        => await _appListingRentService.UpdateRules(listingRentId,
            ruleIds);

        /// <summary>
        /// Servicio que actualiza la posición de una foto en el orden de presentación dentro de un Listing Rent.
        /// </summary>
        /// <param name="listingRentId">ID del Listing Rent al que pertenece la foto</param>
        /// <param name="listingPhotoId">ID de la foto cuya posición se desea modificar</param>
        /// <param name="newPostition">Nueva posición que se desea asignar a la foto en el listado</param>
        /// <returns>UPDATED</returns>
        /// <response code="200">Detalle del Listing Rent con la nueva posición de la foto aplicada</response>
        /// <remarks>
        /// Requisitos:
        /// - El Listing Rent y la foto deben existir previamente.
        /// - La nueva posición debe ser un número entero positivo dentro del rango permitido.
        /// - La operación debe mantener la integridad del orden de las demás fotos.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("{listingRentId}/UpdatePhotoPosition")]
        public async Task<ReturnModelDTO> UpdatePhotoPosition(long listingRentId, 
            long listingPhotoId, int newPostition)
        => await _appListingRentService.UpdatePhotoPosition(listingRentId,
            listingPhotoId, newPostition);

        /// <summary>
        /// Servicio que reorganiza el orden de las fotos asociadas a un Listing Rent.
        /// </summary>
        /// <returns>Modelo con los detalles actualizados del Listing Rent y el nuevo orden aplicado</returns>
        /// <response code="200">UPDATED</response>
        /// <remarks>
        /// Requisitos:
        /// - La identificación del Listing Rent debe estar contenida en el contexto interno o ser deducida por el servicio.
        /// - La nueva secuencia de fotos debe ser provista en el cuerpo de la petición (por ejemplo, lista ordenada de IDs).
        /// - Todas las fotos deben pertenecer al Listing Rent involucrado.
        /// - La operación debe validar integridad del orden y evitar colisiones.
        /// </remarks>
        [HttpPut()]
        [AllowAnonymous]
        [Route("SortListingRentPhotos")]
        public async Task<ReturnModelDTO> SortListingRentPhotos()
        => await _appListingRentService.SortListingRentPhotos();

        /// <summary>
        /// Actualiza los valores de estadía mínima y/o máxima para un Listing Rent específico.
        /// </summary>
        /// <param name="request">
        /// Objeto que contiene los parámetros necesarios para la operación:
        /// - <see cref="UpsertMaxMinStayRequestDTO.ListingRentId"/>: Identificador del Listing Rent.
        /// - <see cref="UpsertMaxMinStayRequestDTO.SetMinStay"/> y <see cref="UpsertMaxMinStayRequestDTO.MinStay"/>: Indican si se debe actualizar la estadía mínima y su nuevo valor.
        /// - <see cref="UpsertMaxMinStayRequestDTO.SetMaxStay"/> y <see cref="UpsertMaxMinStayRequestDTO.MaxStay"/>: Indican si se debe actualizar la estadía máxima y su nuevo valor.
        /// </param>
        /// <returns>
        /// Un modelo con los detalles actualizados del Listing Rent, incluyendo los valores aplicados.
        /// </returns>
        /// <response code="200">UPDATED: La estadía mínima y/o máxima fueron actualizadas exitosamente.</response>
        /// <response code="404">NOT FOUND: No se encontró el Listing Rent con el ID especificado.</response>
        /// <response code="400">BAD REQUEST: Los valores proporcionados no cumplen con las reglas de negocio (por ejemplo, valores por debajo del mínimo permitido).</response>
        /// <response code="500">INTERNAL SERVER ERROR: Error inesperado durante la operación.</response>
        /// <remarks>
        /// Requisitos y validaciones:
        /// - El <c>ListingRentId</c> debe ser valido y existir en el sistema.
        /// - Si <c>SetMinStay</c> o <c>SetMaxStay</c> son <c>true</c>, se validarán los valores contra los mínimos definidos por configuración.
        /// - La operación es idempotente: si no se solicita ningún cambio, se retorna "NO CHANGES".
        /// - Se registra cualquier excepción con contexto completo para trazabilidad.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("UpsertMaxMinStay")]
        public async Task<ReturnModelDTO> UpsertMaxMinStay(UpsertMaxMinStayRequestDTO request)
        => await _appListingRentService.UpsertMaxMinStay(request);

        /// <summary>
        /// Actualiza los valores de preaviso requerido para realizar una reserva en un Listing Rent.
        /// </summary>
        /// <param name="request">
        /// Objeto que contiene los parámetros necesarios para la operación:
        /// - <see cref="UpsertMinimumNoticeRequestDTO.ListingRentId"/>: Identificador del Listing Rent.
        /// - <see cref="UpsertMinimumNoticeRequestDTO.MinimumNoticeDay"/>: Días de preaviso requeridos. Puede ser 0 en caso de ser el mismo dia.
        /// - <see cref="UpsertMinimumNoticeRequestDTO.MinimumNoticeHours"/>: Horas mínimas de preaviso requeridas (En caso de ser el mismo dia).
        /// </param>
        /// <returns>
        /// UPDATED
        /// </returns>
        /// <response code="200">UPDATED: El preaviso fue actualizado exitosamente.</response>
        /// <response code="404">NOT FOUND: No se encontró el Listing Rent con el ID especificado.</response>
        /// <response code="400">BAD REQUEST: Los valores proporcionados no cumplen con las reglas de negocio.</response>
        /// <response code="500">INTERNAL SERVER ERROR: Error inesperado durante la operación.</response>
        /// <remarks>
        /// Requisitos y validaciones:
        /// - El <c>ListingRentId</c> debe ser válido y existir en el sistema.
        /// - El valor de <c>MinimumNoticeDay</c> debe ser mayor o igual a 0. En caso de 0 significa que es el mismo día.
        /// - Si <c>MinimumNoticeDay</c> es igual a cero, se puede especificar <c>MinimumNoticeHours</c> para complementar.
        /// - Si no se especifica <c>MinimumNoticeHours</c>, se aplicará un valor nulo o por defecto según configuración.
        /// - Se registra cualquier excepción con contexto completo para trazabilidad y auditoría.
        /// </remarks>
        [HttpPut()]
        [Authorize(Policy = "GuestOrHost")]
        [Route("UpsertReservationNotice")]
        public async Task<ReturnModelDTO> UpsertReservationNotice(UpsertMinimumNoticeRequestDTO request)
        => await _appListingRentService.UpsertReservationNotice(request);
    }
}
