namespace CitasApi.Domain.DTOs;

public class CrearCitaDto
{
    public int ClienteId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
}
