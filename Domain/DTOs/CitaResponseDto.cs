namespace CitasApi.Domain.DTOs;

public class CitaResponseDto
{
    public int Id { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public string TelefonoCliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public string? AtendidoPor { get; set; }
    public DateTime CreadoEn { get; set; }
}