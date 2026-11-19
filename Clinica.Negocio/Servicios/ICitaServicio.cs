using Clinica.Entidades.Dtos;

namespace Clinica.Negocio.Servicios;

public interface ICitaServicio
{
    Task<List<CitaDto>> ObtenerTodasAsync(DateTime? fecha, string? estado, int? medicoId);
    Task<List<CitaDto>?> ObtenerPorMedicoAsync(int medicoId);
    Task<CitaDto?> ObtenerPorIdAsync(int id);
    Task<CitaDto> CrearAsync(CitaCrearDto dto);
    Task<bool> ActualizarAsync(int id, CitaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}