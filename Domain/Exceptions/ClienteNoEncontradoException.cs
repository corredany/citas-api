namespace CitasApi.Domain.Exceptions;

public class ClienteNoEncontradoException : Exception
{
    public ClienteNoEncontradoException(int id)
        : base($"Cliente con id {id} no encontrado")
    {
    }
}