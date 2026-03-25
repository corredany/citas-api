namespace CitasApi.Domain.DTOs;

public class ActualizarCitaDto
{
    public DateTime? Fecha { get; set; }
    public TimeSpan? Hora { get; set; }
    public string? Estado { get; set; }
    public string? Notas { get; set; }
    public string? AtendidoPor { get; set; }
}