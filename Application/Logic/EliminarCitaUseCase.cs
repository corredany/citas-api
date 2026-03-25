namespace CitasApi.Application.Logic;

using CitasApi.Domain.Exceptions;
using CitasApi.Domain.Interfaces.Repositories;

public class EliminarCitaUseCase
{
    private readonly ICitaRepository _citaRepository;

    public EliminarCitaUseCase(ICitaRepository citaRepository)
    {
        _citaRepository = citaRepository;
    }

    public async Task Execute(int id)
    {
        var cita = await _citaRepository.ObtenerPorId(id);
        if (cita == null) throw new CitaNoEncontradaException(id);

        await _citaRepository.Eliminar(id);
    }
}