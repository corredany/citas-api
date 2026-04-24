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
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret no configurado");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
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