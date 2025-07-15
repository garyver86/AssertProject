using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        /// Servicio de inicio de sesion de usuarios locales + validacion de token y enrollamiento de usuarios de redes sociales
        /// </summary>
        /// <param name="loginRequest">Platform: local-google-meta-apple
        ///  ; Usuarios local: UserName - Password 
        ///  ; Usuaerios redes sociales: UserName - Token</param>
        /// <returns code="200">Si proceso se ejcuto con exito</returns>
        /// <remarks>
        /// En caso de usuarios de redes sociales si el token es valido y no existe en BD Assert, estos seran registrados con rol Guest por defecto
        /// </remarks>
        [HttpPost("Login")]
        //[EnableCors("AllowedOriginsPolicy")]
        public async Task<ReturnModelDTO> Login([FromBody] LoginRequest loginRequest)
        {
            return await _userService.LoginAndEnrollment(loginRequest.Platform, loginRequest.Token,
                loginRequest.UserName, loginRequest.Password);
        }

        /// <summary>
        /// Servicio de enrolamiento de usuario local (Assert)
        /// </summary>
        /// <param name="userRequest">Name - LastName - Email - Password - CountryId - PhoneNumber</param>
        /// <returns code="200">Enrola usuaerio y retorna token de login realizado</returns>
        /// <remarks>
        /// En caso de existir usuario en cualquier otra plataforma lanza error
        /// </remarks>
        [HttpPost("EnrollmentLocalUserAndLogin")]
        //[EnableCors("AllowedOriginsPolicy")]
        public async Task<ReturnModelDTO> EnrollmentLocalUserAndLogin([FromBody] LocalUserRequest userRequest)
        {
            return await _userService.LocalUserEnrollment(userRequest);
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

        /// <summary>
        /// Servicio para renovar un token JWT expirado.
        /// </summary>
        /// <param name="expiredToken">El token JWT expirado que se va a renovar (Permite el acceso sin autenticación).</param>
        /// <returns>Un nuevo token JWT.</returns>
        /// <response code="200">Si el token se renovó correctamente.</response>
        /// <response code="400">Si el token proporcionado no es válido.</response>
        /// <response code="401">Si el usuario asociado al token no se encuentra o está inactivo.</response>
        [HttpPost("RenewToken")]
        [AllowAnonymous] // Permite el acceso sin autenticación
        public async Task<ReturnModelDTO> RenewToken([FromBody] string expiredToken)
        {
            return await _userService.RenewJwtToken(expiredToken);
            //try
            //{
            //    return await _userService.RenewJwtToken(expiredToken);
            //}
            //catch (Exception ex)
            //{
            //    return new ReturnModelDTO
            //    {
            //        StatusCode = ResultStatusCode.InternalError,
            //        HasError = true,
            //        ResultError = new ErrorCommonDTO { Message = $"Error al renovar el token: {ex.Message}" }
            //    };
            //}
        }

        /// <summary>
        /// Servicio que actualiza la información personal del usuario.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: SUCCESS.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Los valores de campos que no se requiere modificar enviarlos vacios en caso de string y 0 en caso de int. 
        /// El numero de telefono es tipo string, tiene el siguiente formato: +591-72724711
        /// </remarks>
        [HttpPost("UpdatePersonalInformation")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UpdatePersonalInformation([FromBody] UpdatePersonalInformationRequest userRequest)
        => await _userService.UpdatePersonalInformation(userRequest);

        /// <summary>
        /// Servicio que retorna el contacto de emergencia del usuario logeado.
        /// </summary>
        /// <returns>Confirmación modelo de contacto de emergencia.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// No requiere enviar ningun Id, ya que el contacto de emergencia se asocia al usuario logeado.
        /// </remarks>
        [HttpGet("GetEmergencyContactByUser")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> GetEmergencyContactByUser()
        => await _userService.GetEmergencyContact();

        /// <summary>
        /// Servicio que inserta/actualiza la información de contacto de emergencia del usuario logeado.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: SUCCESS.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Los valores de campos que no se requiere modificar enviarlos vacios en caso de string y 0 en caso de int.
        /// El numero de telefono es tipo string, tiene el siguiente formato: +591-72724711
        /// </remarks>
        [HttpPost("UpsertEmergencyContact")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UpsertEmergencyContact([FromBody] EmergencyContactRequest emergencyContactRequest)
        => await _userService.UpsertEmergencyContact(emergencyContactRequest);

        /// <summary>
        /// Servicio que retorna informacion personal de usuario logeado.
        /// </summary>
        /// <returns>Confirmación modelo de contacto de emergencia.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// No requiere enviar ningun Id, ya que el contacto de emergencia se asocia al usuario logeado.
        /// El codigo de celular no viene con el simbolo +, por ejemplo: PhoneCode = 591
        /// </remarks>
        [HttpGet("GetPersonalInformation")]
        [Authorize(Policy = "GuestOrHost")]
        public async Task<ReturnModelDTO> GetPersonalInformation()
        => await _userService.GetPersonalInformation();

        /// <summary>
        /// Servicio que modifica la contrasena del usuario.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: UPDATED.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// Solo aplica a usuarios locales (Assert), no aplica a usuarios de: google - facebook - apple.
        /// </remarks>
        [HttpPost("ChangePassword")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> ChangePasswordToLocalUser([FromBody] ChangePasswordRequest pwd)
        => await _userService.ChangePassword(pwd);

        /// <summary>
        /// Servicio que retorna el perfil del usuario logeado contemplando resenas de huespedes y anfitriones.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: UPDATED.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPost("GetProfile")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetProfile()
        => await _userService.GetUserProfile();

        /// <summary>
        /// Servicio que inserta/actualiza datos adicionales de perfil del usuario logeado.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: UPDATED.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPost("GetAdditionalProfileData")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> GetAdditionalProfileData()
        => await _userService.GetAdditionalProfile();

        /// <summary>
        /// Servicio que inserta/actualiza datos adicionales de perfil del usuario logeado.
        /// </summary>
        /// <returns>Confirmación de la actualizacion: UPDATED.</returns>
        /// <response code="200">Si se procesó correctamente.</response>
        /// <remarks>
        /// En caso de insert retorna el nuevo Id de datos adicionales de perfil, en caso de update retorna el Id existente.
        /// </remarks>
        [HttpPost("UpsertAdditionalProfileData")]
        [Authorize(Policy = "GuestOrHostOrAdmin")]
        public async Task<ReturnModelDTO> UpsertAdditionalProfileData([FromBody] AdditionalProfileDataDTO data)
        =>  await _userService.UpsertAdditionalProfile(data);
    }
}
