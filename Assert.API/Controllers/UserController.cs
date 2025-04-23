using Assert.API.Helpers;
using Assert.API.Models;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Common;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Assert.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IAppUserService _userService;

    public UserController(IAppUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    [EnableCors("AllowedOriginsPolicy")]
    public async Task<ReturnModelDTO> Login([FromBody] LoginRequest loginRequest)
    {
        return await _userService.LoginAndEnrollment(loginRequest.Platform, loginRequest.Token,
            loginRequest.UserName, loginRequest.Password);
    }
}
