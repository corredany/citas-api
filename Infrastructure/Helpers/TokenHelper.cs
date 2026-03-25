namespace CitasApi.Infrastructure.Helpers;

using CitasApi.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class TokenHelper : ITokenService
{
    private readonly string _secret;

    public TokenHelper(IConfiguration configuration)
    {
        _secret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret no configurado");
    }

    public bool ValidarToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public int ObtenerUsuarioId(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");
        return idClaim != null ? int.Parse(idClaim.Value) : 0;
    }
}