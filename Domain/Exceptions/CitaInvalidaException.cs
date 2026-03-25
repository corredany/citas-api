namespace CitasApi.Domain.Exceptions;

public class CitaInvalidaException : Exception
{
    public CitaInvalidaException(string mensaje)
        : base(mensaje)
    {
    }
}