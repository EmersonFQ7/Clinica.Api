using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades.Dtos;

public class CitaActualizarDto
{
    [Range(1, int.MaxValue)]
    public int MedicoId { get; set; }

    [Required, StringLength(120, MinimumLength = 3)]
    public string NombrePaciente { get; set; } = null!;

    [Required, StringLength(15, MinimumLength = 8)]
    public string DniPaciente { get; set; } = null!;

    [Required]
    public DateTime FechaHora { get; set; }

    [StringLength(250)]
    public string? Motivo { get; set; }

    [Required, RegularExpression("Programada|Atendida|Cancelada",
        ErrorMessage = "Estado debe ser Programada, Atendida o Cancelada.")]
    public string Estado { get; set; } = null!;
}