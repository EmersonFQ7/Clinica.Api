using System;
using System.Collections.Generic;

namespace Clinica.Entidades.Models;

public partial class Cita
{
    public int Id { get; set; }

    public int MedicoId { get; set; }

    public string NombrePaciente { get; set; } = null!;

    public string DniPaciente { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public string? Motivo { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Medico Medico { get; set; } = null!;
}
