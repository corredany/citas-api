namespace CitasApi.Application.Logic;

using CitasApi.Domain.DTOs;
using CitasApi.Domain.Entities;
using CitasApi.Domain.Interfaces.Repositories;

public class ObtenerCitasUseCase
{
    private readonly ICitaRepository _citaRepository;

    public ObtenerCitasUseCase(ICitaRepository citaRepository)
    {
        _citaRepository = citaRepository;
    }

    public async Task<IEnumerable<CitaResponseDto>> Execute()
    {
        var citas = await _citaRepository.ObtenerTodas();
        return citas.Select(c => new CitaResponseDto
        {
            Id = c.Id,
            NombreCliente = c.Cliente?.Nombre ?? string.Empty,
            EmailCliente = c.Cliente?.Email ?? string.Empty,
            TelefonoCliente = c.Cliente?.Telefono ?? string.Empty,
            Fecha = c.Fecha,
            Hora = c.Hora,
            Estado = c.Estado,
            Notas = c.Notas,
            AtendidoPor = c.AtendidoPor,
            CreadoEn = c.CreadoEn,
        });
    }
}