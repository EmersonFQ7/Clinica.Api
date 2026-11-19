using Clinica.Datos.Models;
using Clinica.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Datos.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly ClinicaContext _context;
    public UsuarioRepositorio(ClinicaContext context) => _context = context;

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

    public async Task AgregarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }
}