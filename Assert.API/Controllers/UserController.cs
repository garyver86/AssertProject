using Assert.API.Helpers;
using Assert.Application.DTOs;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IAppUserService _userService;
        public UserController(IAppUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Servicio que habilita el rol de HOST al usuario actual.
        /// </summary>
        /// <returns>Confirmación de la habilitación.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Una vez habilitado el rol, se debe crear un nuevo JWT, ya que estos son generados con información del rol del usuario.
        /// </remarks>
        [HttpGet("EnableHostRole")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> EnableHostRole()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var result = await _userService.EnableHostRole(userId, requestInfo, true);

            return result;
        }
        /// <summary>
        /// Servicio que deshabilita el rol de HOST al usuario actual.
        /// </summary>
        /// <returns>Confirmación de la deshabilitación.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Una vez deshabilitado el rol, se debe crear un nuevo JWT, ya que estos son generados con información del rol del usuario.
        /// </remarks>
        [HttpGet("DisableHostRole")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> DisableHostRole()
        {
            var requestInfo = HttpContext.GetRequestInfo();
            int userId = 0;
            int.TryParse(User.FindFirst("identifier")?.Value, out userId);
            var result = await _userService.DisableHostRole(userId, requestInfo, true);

            return result;
        }
    }
}
