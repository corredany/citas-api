namespace CitasApi.Domain.Entities;

public class Cita
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public string Estado { get; set; } = "pendiente";
    public string? Notas { get; set; }
    public string? AtendidoPor { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime ActualizadoEn { get; set; }

    // Reglas de negocio
    public bool PuedeCancelarse() =>
        Estado == "pendiente" || Estado == "confirmada";

    public bool PuedeConfirmarse() =>
        Estado == "pendiente";

    public bool PuedeCompletarse() =>
        Estado == "confirmada";

    public bool EsHoraValida() =>
        Hora >= TimeSpan.FromHours(8) && Hora <= TimeSpan.FromHours(13);

    public bool EsDiaValido() =>
        Fecha.DayOfWeek != DayOfWeek.Saturday && Fecha.DayOfWeek != DayOfWeek.Sunday;

    public bool EsFechaFutura() =>
        Fecha.Date >= DateTime.Today;
}