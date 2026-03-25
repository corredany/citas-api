namespace CitasApi.Domain.Interfaces.Repositories;

using CitasApi.Domain.Entities;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> ObtenerTodos();
    Task<Cliente?> ObtenerPorId(int id);
    Task<Cliente?> ObtenerPorEmail(string email);
    Task<Cliente> Crear(Cliente cliente);
}