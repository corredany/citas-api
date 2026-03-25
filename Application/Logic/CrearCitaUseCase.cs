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
        // 1. Crear cita temporal para validar reglas
        var citaTemporal = new Cita
        {
            Fecha = dto.Fecha,
            Hora = dto.Hora,
        };

        // 2. Validar reglas de negocio
        if (!citaTemporal.EsFechaFutura())
            throw new CitaInvalidaException("La fecha no puede ser en el pasado");

        if (!citaTemporal.EsDiaValido())
            throw new CitaInvalidaException("Solo se pueden agendar citas de lunes a viernes");

        if (!citaTemporal.EsHoraValida())
            throw new CitaInvalidaException("El horario de atención es de 8:00 a 13:00");

        // 3. Verificar disponibilidad
        var horarioOcupado = await _citaRepository.ExisteEnHorario(dto.Fecha, dto.Hora);
        if (horarioOcupado)
            throw new CitaNoDisponibleException();

        // 4. Buscar o crear cliente
        var cliente = await _clienteRepository.ObtenerPorEmail(dto.Email);
        if (cliente == null)
        {
            cliente = await _clienteRepository.Crear(new Cliente
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                CreadoEn = DateTime.UtcNow,
            });
        }

        // 5. Crear cita
        var cita = new Cita
        {
            ClienteId = cliente.Id,
            Fecha = dto.Fecha,
            Hora = dto.Hora,
            Estado = "pendiente",
            Notas = dto.Notas,
            CreadoEn = DateTime.UtcNow,
            ActualizadoEn = DateTime.UtcNow,
        };

        return await _citaRepository.Crear(cita);
    }
}