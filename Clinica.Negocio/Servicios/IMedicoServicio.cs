using Clinica.Entidades.Dtos;

namespace Clinica.Negocio.Servicios;

public interface IMedicoServicio
{
    Task<List<MedicoDto>> ObtenerTodosAsync(string? especialidad, bool? activo);
    Task<MedicoDto?> ObtenerPorIdAsync(int id);
    Task<MedicoDto> CrearAsync(MedicoCrearDto dto);
    Task<bool> ActualizarAsync(int id, MedicoActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}