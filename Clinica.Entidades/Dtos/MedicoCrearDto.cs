using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades.Dtos;

public class MedicoCrearDto
{
    [Required, StringLength(80, MinimumLength = 2)]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(80, MinimumLength = 2)]
    public string Apellido { get; set; } = null!;

    [Required, StringLength(80)]
    public string Especialidad { get; set; } = null!;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = null!;
}