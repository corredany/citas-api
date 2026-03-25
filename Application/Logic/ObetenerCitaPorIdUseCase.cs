namespace CitasApi.Application.Logic;

using CitasApi.Domain.DTOs;
using CitasApi.Domain.Exceptions;
using CitasApi.Domain.Interfaces.Repositories;

public class ObtenerCitaPorIdUseCase
{
    private readonly ICitaRepository _citaRepository;

    public ObtenerCitaPorIdUseCase(ICitaRepository citaRepository)
    {
        _citaRepository = citaRepository;
    }

    public async Task<CitaResponseDto> Execute(int id)
    {
        var cita = await _citaRepository.ObtenerPorId(id);
        if (cita == null) throw new CitaNoEncontradaException(id);

        return new CitaResponseDto
        {
            Id = cita.Id,
            NombreCliente = cita.Cliente?.Nombre ?? string.Empty,
            EmailCliente = cita.Cliente?.Email ?? string.Empty,
            TelefonoCliente = cita.Cliente?.Telefono ?? string.Empty,
            Fecha = cita.Fecha,
            Hora = cita.Hora,
            Estado = cita.Estado,
            Notas = cita.Notas,
            AtendidoPor = cita.AtendidoPor,
            CreadoEn = cita.CreadoEn,
        };
    }
}