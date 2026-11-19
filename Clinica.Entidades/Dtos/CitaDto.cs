namespace Clinica.Entidades.Dtos;

public class CitaDto
{
    public int Id { get; set; }
    public int MedicoId { get; set; }
    public string Medico { get; set; } = null!;   // nombre completo del médico
    public string NombrePaciente { get; set; } = null!;
    public string DniPaciente { get; set; } = null!;
    public DateTime FechaHora { get; set; }
    public string? Motivo { get; set; }
    public string Estado { get; set; } = null!;
}