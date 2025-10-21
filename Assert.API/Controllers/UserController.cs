using Assert.API.Helpers;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers;

public class UserController : Controller
{
    private readonly IAppUserService _userService;
    private readonly RequestMetadata _metadata;
    public UserController(IAppUserService userService, RequestMetadata metadata)
    {
        _userService = userService;
        _metadata = metadata;
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
    /// Endpoint para restablecer la contraseña de un usuario local mediante código OTP.
    /// </summary>
    /// <param name="pwd">Modelo con credenciales y código OTP.</param>
    /// <returns>Objeto <see cref="ReturnModelDTO"/> con el resultado de la operación.</returns>
    /// <response code="200">La contraseña fue actualizada correctamente.</response>
    /// <response code="400">Solicitud inválida o código OTP incorrecto.</response>
    /// <response code="404">Usuario no encontrado.</response>
    /// <remarks>
    /// Este servicio solo aplica a usuarios locales registrados en ASSERT.
    /// No está disponible para cuentas federadas como Google, Facebook o Apple.
    /// Requiere un código OTP válido previamente generado y enviado por correo electronico.
    /// </remarks>

    [HttpPost("ForgotPassword")]
    public async Task<ReturnModelDTO> ForgotPasswordToLocalUser([FromBody] ForgotPasswordRequest pwd)
    => await _userService.ForgotPassword(pwd);

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
    => await _userService.UpsertAdditionalProfile(data);

    /// <summary>
    /// Servicio actualiza foto de perfil.
    /// </summary>
    /// <returns>Confirmación de la actualizacion: retorna la url de la foto subida</returns>
    /// <response code="200">Si se proceso correctamente.</response>
    /// <remarks>
    /// Esto funciona para todos los tipos de cuenta: local, google, facebook, apple.
    /// </remarks>
    [HttpPost("UpdateProfilePhoto")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> UpdateProfilePhoto(ProfilePhotoRequestDTO profilePhotoRequest)
    => await _userService.UpdateProfilePhoto(profilePhotoRequest.ProfilePhoto);

    /// <summary>
    /// Servicio para bloquear un usuario como host.
    /// </summary>
    /// <param name="id">Id del usuario a bloquear</param>
    /// <returns>Confirmación de la actualizacion</returns>
    /// <response code="200">Si se proceso correctamente.</response>
    /// <remarks>
    /// Esta acción bloquea a un usuario para que no pueda realizar tareas de host, por lo que sus propiedades
    /// no podran aparecer en busquedas, en listas de sugerencias, tampoco se podrán realizar cotizaciones (Solo permitirá crear una reserva
    /// si el pago se realizó antes del bloqueo, para evitar que un usuario pague y no obtenga el servicio y evitar malas experiencias).
    /// </remarks>
    [HttpPut("{id}/BlockAsHost")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> BlockAsHost(int id)
    => await _userService.BlockAsHost(id, _metadata.UserId, HttpContext.GetRequestInfo(), true);

    /// <summary>
    /// Servicio para desbloquear un usuario como host.
    /// </summary>
    /// <param name="id">Id del usuario a desbloquear</param>
    /// <returns>Confirmación de la actualizacion</returns>
    /// <response code="200">Si se proceso correctamente.</response>
    [HttpPut("{id}/UnblockAsHost")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> UnblockAsHost(int id)
    => await _userService.UnblockAsHost(id, _metadata.UserId, HttpContext.GetRequestInfo(), true);


    /// <summary>
    /// Servicio que obtiene las opciones de selección disponibles para cerrar una cuenta de usuario.
    /// </summary>
    /// <returns>Lista de opciones de cierre de cuenta.</returns>
    /// <response code="200">Si se procesó correctamente.</response>
    /// <remarks>
    /// Retorna todas las opciones activas que el usuario puede seleccionar al momento de cerrar su cuenta.
    /// Estas opciones pueden estar agrupadas por categorías según el tipo de grupo de usuario.
    /// </remarks>
    [HttpGet("GetUserSelectionOptions")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> GetUserSelectionOptions()
    => await _userService.GetUserSelectionOptions();

    /// <summary>
    /// Servicio que cierra la cuenta del usuario logeado.
    /// </summary>
    /// <param name="userAccountClosedId">ID del registro de cuenta cerrada (0 para crear nuevo).</param>
    /// <param name="userSelectionOptionsId">ID de la opción seleccionada por el usuario.</param>
    /// <returns>Confirmación del cierre: SAVED.</returns>
    /// <response code="200">Si se procesó correctamente.</response>
    /// <remarks>
    /// Cierra la cuenta del usuario y registra la razón seleccionada.
    /// El usuario no podrá acceder a la plataforma hasta que la cuenta sea restaurada.
    /// UserAccountClosedId debe ser 0 para crear un nuevo registro, o el ID existente para actualizar.
    /// </remarks>
    [HttpPost("CloseUserAccount")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> CloseUserAccount(int userAccountClosedId, int userSelectionOptionsId)
    => await _userService.CloseUserAccount(userAccountClosedId, userSelectionOptionsId);

    /// <summary>
    /// Servicio que restaura una cuenta de usuario previamente cerrada.
    /// </summary>
    /// <param name="userAccountClosedId">ID del registro de cuenta cerrada.</param>
    /// <returns>Confirmación de la restauración: RESTORED.</returns>
    /// <response code="200">Si se procesó correctamente.</response>
    /// <remarks>
    /// Restaura el acceso a la cuenta del usuario que fue previamente cerrada.
    /// El usuario podrá volver a iniciar sesión y usar normalmente la plataforma.
    /// </remarks>
    [HttpPut("RestoreUserAccount/{userAccountClosedId}")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> RestoreUserAccount(int userAccountClosedId)
    => await _userService.RestoreUserAccount(userAccountClosedId);

    /// <summary>
    /// Servicio que obtiene información sobre el cierre de cuenta del usuario logeado.
    /// </summary>
    /// <returns>Información del cierre de cuenta incluyendo fecha y razón.</returns>
    /// <response code="200">Si se procesó correctamente.</response>
    /// <response code="404">Si no existe información de cierre de cuenta para el usuario.</response>
    /// <remarks>
    /// Retorna los detalles del último cierre de cuenta del usuario actual,
    /// incluyendo la opción seleccionada, fechas de cierre y apertura (si aplica).
    /// </remarks>
    [HttpGet("GetUserAccountClosed")]
    [Authorize(Policy = "GuestOrHostOrAdmin")]
    public async Task<ReturnModelDTO> GetUserAccountClosed()
    => await _userService.GetUserAccountClosed();
}
