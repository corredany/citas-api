namespace CitasApi.Application.Logic;

using CitasApi.Domain.DTOs;
using CitasApi.Domain.Entities;
using CitasApi.Domain.Interfaces.Repositories;

public class CrearClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;

    public CrearClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Cliente> Execute(CrearClienteDto dto)
    {
        var existente = await _clienteRepository.ObtenerPorEmail(dto.Email);
        if (existente != null)
            return existente;

        return await _clienteRepository.Crear(new Cliente
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Telefono = dto.Telefono,
            CreadoEn = DateTime.UtcNow,
        });
    }
}
