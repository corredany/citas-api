namespace CitasApi.Infrastructure.Repositories;

using CitasApi.Domain.Entities;
using CitasApi.Domain.Interfaces.Repositories;
using CitasApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class CitaRepository : ICitaRepository
{
    private readonly AppDbContext _context;

    public CitaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cita>> ObtenerTodas()
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
    }

    public async Task<Cita?> ObtenerPorId(int id)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExisteEnHorario(DateTime fecha, TimeSpan hora)
    {
        return await _context.Citas
            .AnyAsync(c => c.Fecha.Date == fecha.Date && c.Hora == hora && c.Estado != "cancelada");
    }

    public async Task<Cita> Crear(Cita cita)
    {
        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();
        return cita;
    }

    public async Task<Cita> Actualizar(Cita cita)
    {
        cita.ActualizadoEn = DateTime.UtcNow;
        _context.Citas.Update(cita);
        await _context.SaveChangesAsync();
        return cita;
    }

    public async Task Eliminar(int id)
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita != null)
        {
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}