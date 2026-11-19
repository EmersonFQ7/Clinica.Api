using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clinica.Datos.Repositorios;
using Clinica.Entidades;
using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Entidades.Models;
using Clinica.Negocio.Configuracion;
using Microsoft.IdentityModel.Tokens;

namespace Clinica.Negocio.Servicios;

public class AuthServicio : IAuthServicio
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly JwtConfig _jwt;

    public AuthServicio(IUsuarioRepositorio repositorio, JwtConfig jwt)
    {
        _repositorio = repositorio;
        _jwt = jwt;
    }

    public async Task<PerfilDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        // Regla: correo único
        if (await _repositorio.ObtenerPorCorreoAsync(correo) is not null)
            throw new ReglaNegocioException($"El correo '{correo}' ya está registrado.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolesClinica.Recepcionista   // el rol lo decide el servidor
        };
        await _repositorio.AgregarAsync(usuario);
        return new PerfilDto { Correo = usuario.Correo, Rol = usuario.Rol };
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _repositorio.ObtenerPorCorreoAsync(dto.Correo.Trim().ToLowerInvariant());
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        var expira = DateTime.UtcNow.AddMinutes(_jwt.Minutos);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("role", usuario.Rol)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expira
        };
    }
}