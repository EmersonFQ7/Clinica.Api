using Clinica.Entidades.Models;

namespace Clinica.Datos.Repositorios;

public interface ICitaRepositorio
{
    Task<List<Cita>> ObtenerTodasAsync(DateTime? fecha, string? estado, int? medicoId);
    Task<List<Cita>> ObtenerPorMedicoAsync(int medicoId);
    Task<Cita?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteCitaEnHorarioAsync(int medicoId, DateTime fechaHora, int? excluirId = null);
    Task AgregarAsync(Cita cita);
    Task ActualizarAsync(Cita cita);
    Task EliminarAsync(Cita cita);
}