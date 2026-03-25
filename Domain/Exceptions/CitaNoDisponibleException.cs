namespace CitasApi.Domain.Exceptions;

public class CitaNoDisponibleException : Exception
{
    public CitaNoDisponibleException()
        : base("La fecha y hora seleccionada no está disponible")
    {
    }
}