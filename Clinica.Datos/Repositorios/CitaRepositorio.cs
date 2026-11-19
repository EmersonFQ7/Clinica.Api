using Clinica.Datos.Models;
using Clinica.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Datos.Repositorios;

public class CitaRepositorio : ICitaRepositorio
{
    private readonly ClinicaContext _context;

    public CitaRepositorio(ClinicaContext context)
    {
        _context = context;
    }

    public async Task<List<Cita>> ObtenerTodasAsync(DateTime? fecha, string? estado, int? medicoId)
    {
        var consulta = _context.Citas
            .AsNoTracking()
            .Include(c => c.Medico)
            .AsQueryable();

        if (fecha.HasValue)
        {
            var inicio = fecha.Value.Date;
            var fin = inicio.AddDays(1);
            consulta = consulta.Where(c => c.FechaHora >= inicio && c.FechaHora < fin);
        }

        if (!string.IsNullOrWhiteSpace(estado))
            consulta = consulta.Where(c => c.Estado == estado);

        if (medicoId.HasValue)
            consulta = consulta.Where(c => c.MedicoId == medicoId.Value);

        return await consulta.OrderBy(c => c.FechaHora).ToListAsync();
    }

    public async Task<List<Cita>> ObtenerPorMedicoAsync(int medicoId)
    {
        return await _context.Citas
            .AsNoTracking()
            .Include(c => c.Medico)
            .Where(c => c.MedicoId == medicoId)
            .OrderBy(c => c.FechaHora)
            .ToListAsync();
    }

    public async Task<Cita?> ObtenerPorIdAsync(int id)
    {
        return await _context.Citas
            .Include(c => c.Medico)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExisteCitaEnHorarioAsync(int medicoId, DateTime fechaHora, int? excluirId = null)
    {
        return await _context.Citas.AnyAsync(c =>
            c.MedicoId == medicoId &&
            c.FechaHora == fechaHora &&
            c.Estado != "Cancelada" &&
            (excluirId == null || c.Id != excluirId));
    }

    public async Task AgregarAsync(Cita cita)
    {
        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Cita cita)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Cita cita)
    {
        _context.Citas.Remove(cita);
        await _context.SaveChangesAsync();
    }
}