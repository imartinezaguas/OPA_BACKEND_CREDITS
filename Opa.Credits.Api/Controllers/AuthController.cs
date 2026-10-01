using Microsoft.AspNetCore.Mvc;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Application.Interfaces;

namespace Opa.Credits.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.Authenticate(dto.Username, dto.Password);
        
        if (token == null)
        {
            return Unauthorized(new 
            { 
                success = false, 
                error = new { code = "UNAUTHORIZED", message = "Credenciales incorrectas." } 
            });
        }

        return Ok(new 
        { 
            success = true, 
            data = new { token, message = "Autenticación exitosa" }
        });
    }
}
