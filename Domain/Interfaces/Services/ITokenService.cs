namespace CitasApi.Domain.Interfaces.Services;

public interface ITokenService
{
    bool ValidarToken(string token);
    int ObtenerUsuarioId(string token);
}