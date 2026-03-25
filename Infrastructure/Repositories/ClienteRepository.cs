namespace CitasApi.Infrastructure.Repositories;

using CitasApi.Domain.Entities;
using CitasApi.Domain.Interfaces.Repositories;
using CitasApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodos()
    {
        return await _context.Clientes
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorId(int id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> ObtenerPorEmail(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Cliente> Crear(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }
}