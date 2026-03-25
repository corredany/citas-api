namespace CitasApi.Domain.Exceptions;

public class CitaNoEncontradaException : Exception
{
    public CitaNoEncontradaException(int id)
        : base($"Cita con id {id} no encontrada")
    {
    }
}