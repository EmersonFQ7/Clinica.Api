using Clinica.Entidades.Dtos;

namespace Clinica.Negocio.Servicios;

public interface IAuthServicio
{
    Task<PerfilDto> RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}