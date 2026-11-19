using Clinica.Entidades.Models;

namespace Clinica.Datos.Repositorios;

public interface IMedicoRepositorio
{
    Task<List<Medico>> ObtenerTodosAsync(string? especialidad, bool? activo);
    Task<Medico?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteEmailAsync(string email, int? excluirId = null);
    Task<bool> TieneCitasAsync(int medicoId);
    Task AgregarAsync(Medico medico);
    Task ActualizarAsync(Medico medico);
    Task EliminarAsync(Medico medico);
}