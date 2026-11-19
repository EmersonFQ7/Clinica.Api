using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServicio _servicio;
    public AuthController(IAuthServicio servicio) => _servicio = servicio;

    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PerfilDto>> Registro(RegistroDto dto)
    {
        try
        {
            var perfil = await _servicio.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, perfil);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _servicio.LoginAsync(dto);
        if (token is null) return Unauthorized(new { mensaje = "Credenciales inválidas." });
        return Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<PerfilDto> Perfil()
    {
        return Ok(new PerfilDto
        {
            Correo = User.FindFirst("email")?.Value ?? "",
            Rol = User.FindFirst("role")?.Value ?? ""
        });
    }
}