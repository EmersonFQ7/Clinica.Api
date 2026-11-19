namespace Clinica.Negocio.Configuracion;

public class JwtConfig
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int Minutos { get; set; } = 60;
}