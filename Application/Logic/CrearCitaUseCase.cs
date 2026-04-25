namespace CitasApi.Application.Logic;

using CitasApi.Domain.DTOs;
using CitasApi.Domain.Entities;
using CitasApi.Domain.Exceptions;
using CitasApi.Domain.Interfaces.Repositories;

public class CrearCitaUseCase
{
    private readonly ICitaRepository _citaRepository;
    private readonly IClienteRepository _clienteRepository;

    public CrearCitaUseCase(ICitaRepository citaRepository, IClienteRepository clienteRepository)
    {
        _citaRepository = citaRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<Cita> Execute(CrearCitaDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorId(dto.ClienteId)
            ?? throw new ClienteNoEncontradoException(dto.ClienteId);

        var cita = new Cita
        {
            ClienteId = cliente.Id,
            Fecha = dto.Fecha.Date == DateTime.MinValue.Date ? DateTime.UtcNow.Date : dto.Fecha.Date,
            Hora = dto.Fecha.TimeOfDay,
            Estado = "pendiente",
            Notas = dto.Descripcion,
            CreadoEn = DateTime.UtcNow,
            ActualizadoEn = DateTime.UtcNow,
        };

        return await _citaRepository.Crear(cita);
    }
}
