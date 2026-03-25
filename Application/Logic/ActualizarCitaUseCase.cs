namespace CitasApi.Application.Logic;

using CitasApi.Domain.DTOs;
using CitasApi.Domain.Exceptions;
using CitasApi.Domain.Interfaces.Repositories;

public class ActualizarCitaUseCase
{
    private readonly ICitaRepository _citaRepository;

    public ActualizarCitaUseCase(ICitaRepository citaRepository)
    {
        _citaRepository = citaRepository;
    }

    public async Task<CitaResponseDto> Execute(int id, ActualizarCitaDto dto)
    {
        // 1. Buscar cita
        var cita = await _citaRepository.ObtenerPorId(id);
        if (cita == null) throw new CitaNoEncontradaException(id);

        // 2. Validar estado si se quiere cambiar
        if (dto.Estado != null)
        {
            if (dto.Estado == "confirmada" && !cita.PuedeConfirmarse())
                throw new CitaInvalidaException("La cita no puede confirmarse en su estado actual");

            if (dto.Estado == "cancelada" && !cita.PuedeCancelarse())
                throw new CitaInvalidaException("La cita no puede cancelarse en su estado actual");

            if (dto.Estado == "completada" && !cita.PuedeCompletarse())
                throw new CitaInvalidaException("La cita no puede completarse en su estado actual");
        }

        // 3. Actualizar campos
        if (dto.Fecha.HasValue) cita.Fecha = dto.Fecha.Value;
        if (dto.Hora.HasValue) cita.Hora = dto.Hora.Value;
        if (dto.Estado != null) cita.Estado = dto.Estado;
        if (dto.Notas != null) cita.Notas = dto.Notas;
        if (dto.AtendidoPor != null) cita.AtendidoPor = dto.AtendidoPor;

        var citaActualizada = await _citaRepository.Actualizar(cita);

        return new CitaResponseDto
        {
            Id = citaActualizada.Id,
            NombreCliente = citaActualizada.Cliente?.Nombre ?? string.Empty,
            EmailCliente = citaActualizada.Cliente?.Email ?? string.Empty,
            TelefonoCliente = citaActualizada.Cliente?.Telefono ?? string.Empty,
            Fecha = citaActualizada.Fecha,
            Hora = citaActualizada.Hora,
            Estado = citaActualizada.Estado,
            Notas = citaActualizada.Notas,
            AtendidoPor = citaActualizada.AtendidoPor,
            CreadoEn = citaActualizada.CreadoEn,
        };
    }
}