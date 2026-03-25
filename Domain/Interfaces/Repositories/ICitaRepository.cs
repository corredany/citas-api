namespace CitasApi.Domain.Interfaces.Repositories;

using CitasApi.Domain.Entities;
using CitasApi.Domain.DTOs;

public interface ICitaRepository
{
    Task<IEnumerable<Cita>> ObtenerTodas();
    Task<Cita?> ObtenerPorId(int id);
    Task<bool> ExisteEnHorario(DateTime fecha, TimeSpan hora);
    Task<Cita> Crear(Cita cita);
    Task<Cita> Actualizar(Cita cita);
    Task Eliminar(int id);
}