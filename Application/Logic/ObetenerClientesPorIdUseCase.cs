namespace CitasApi.Application.Logic;

using CitasApi.Domain.Entities;
using CitasApi.Domain.Exceptions;
using CitasApi.Domain.Interfaces.Repositories;

public class ObtenerClientePorIdUseCase
{
    private readonly IClienteRepository _clienteRepository;

    public ObtenerClientePorIdUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Cliente> Execute(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorId(id);
        if (cliente == null) throw new ClienteNoEncontradoException(id);
        return cliente;
    }
}