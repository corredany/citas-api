using CitasApi.Application.Logic;
using CitasApi.Domain.Interfaces.Repositories;
using CitasApi.Domain.Interfaces.Services;
using CitasApi.Infrastructure.Database;
using CitasApi.Infrastructure.Helpers;
using CitasApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Controllers con ExceptionFilter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddOpenApi();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Servicios
builder.Services.AddScoped<ITokenService, TokenHelper>();

// Casos de uso
builder.Services.AddScoped<CrearCitaUseCase>();
builder.Services.AddScoped<ObtenerCitasUseCase>();
builder.Services.AddScoped<ObtenerCitaPorIdUseCase>();
builder.Services.AddScoped<ActualizarCitaUseCase>();
builder.Services.AddScoped<EliminarCitaUseCase>();
builder.Services.AddScoped<ObtenerClientesUseCase>();
builder.Services.AddScoped<ObtenerClientePorIdUseCase>();

// JWT
// .NET 9 / Microsoft.IdentityModel 7.x exige >256 bits para HS256, pero el secreto tiene 168 bits.
// Se usa SignatureValidator personalizado con HMACSHA256 nativo para omitir esa restricción.
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret no configurado");
var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtSecret);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            SignatureValidator = (token, _) =>
            {
                var parts = token.Split('.');
                if (parts.Length != 3)
                    throw new SecurityTokenMalformedException("Formato de token inválido");

                var signingInput = Encoding.ASCII.GetBytes(parts[0] + "." + parts[1]);
                using var hmac = new System.Security.Cryptography.HMACSHA256(jwtKeyBytes);
                var computedSig = Convert.ToBase64String(hmac.ComputeHash(signingInput))
                    .Replace("+", "-").Replace("/", "_").TrimEnd('=');

                if (!string.Equals(computedSig, parts[2], StringComparison.Ordinal))
                    throw new SecurityTokenInvalidSignatureException("Firma JWT inválida");

                return new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler().ReadJsonWebToken(token);
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}
