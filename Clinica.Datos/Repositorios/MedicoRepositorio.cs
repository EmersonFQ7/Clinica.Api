using Clinica.Datos.Models;
using Clinica.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Datos.Repositorios;

public class MedicoRepositorio : IMedicoRepositorio
{
    private readonly ClinicaContext _context;

    public MedicoRepositorio(ClinicaContext context)
    {
        _context = context;
    }

    public async Task<List<Medico>> ObtenerTodosAsync(string? especialidad, bool? activo)
    {
        var consulta = _context.Medicos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(especialidad))
            consulta = consulta.Where(m => m.Especialidad == especialidad);

        if (activo.HasValue)
            consulta = consulta.Where(m => m.Activo == activo.Value);

        return await consulta
            .OrderBy(m => m.Apellido)
            .ThenBy(m => m.Nombre)
            .ToListAsync();
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        return await _context.Medicos.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? excluirId = null)
    {
        return await _context.Medicos
            .AnyAsync(m => m.Email == email && (excluirId == null || m.Id != excluirId));
    }

    public async Task<bool> TieneCitasAsync(int medicoId)
    {
        return await _context.Citas.AnyAsync(c => c.MedicoId == medicoId);
    }

    public async Task AgregarAsync(Medico medico)
    {
        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Medico medico)
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Medico medico)
    {
        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();
    }
}