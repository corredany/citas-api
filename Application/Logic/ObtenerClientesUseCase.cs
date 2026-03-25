namespace CitasApi.Application.Logic;

using CitasApi.Domain.Entities;
using CitasApi.Domain.Interfaces.Repositories;

public class ObtenerClientesUseCase
{
    private readonly IClienteRepository _clienteRepository;

    public ObtenerClientesUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<IEnumerable<Cliente>> Execute()
    {
        return await _clienteRepository.ObtenerTodos();
    }
}