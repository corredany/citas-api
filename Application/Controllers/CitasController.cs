namespace CitasApi.Application.Controllers;

using CitasApi.Application.Logic;
using CitasApi.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/citas")]
public class CitasController : ControllerBase
{
    private readonly CrearCitaUseCase _crearCitaUseCase;
    private readonly ObtenerCitasUseCase _obtenerCitasUseCase;
    private readonly ObtenerCitaPorIdUseCase _obtenerCitaPorIdUseCase;
    private readonly ActualizarCitaUseCase _actualizarCitaUseCase;
    private readonly EliminarCitaUseCase _eliminarCitaUseCase;

    public CitasController(
        CrearCitaUseCase crearCitaUseCase,
        ObtenerCitasUseCase obtenerCitasUseCase,
        ObtenerCitaPorIdUseCase obtenerCitaPorIdUseCase,
        ActualizarCitaUseCase actualizarCitaUseCase,
        EliminarCitaUseCase eliminarCitaUseCase)
    {
        _crearCitaUseCase = crearCitaUseCase;
        _obtenerCitasUseCase = obtenerCitasUseCase;
        _obtenerCitaPorIdUseCase = obtenerCitaPorIdUseCase;
        _actualizarCitaUseCase = actualizarCitaUseCase;
        _eliminarCitaUseCase = eliminarCitaUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearCitaDto dto)
    {
        var cita = await _crearCitaUseCase.Execute(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cita.Id }, cita);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerTodas()
    {
        var citas = await _obtenerCitasUseCase.Execute();
        return Ok(citas);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cita = await _obtenerCitaPorIdUseCase.Execute(id);
        return Ok(cita);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCitaDto dto)
    {
        var rolId = User.Claims.FirstOrDefault(c => c.Type == "rolId")?.Value;
        var esAdmin = rolId == "1";
        var cita = await _actualizarCitaUseCase.Execute(id, dto, esAdmin);
        return Ok(cita);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _eliminarCitaUseCase.Execute(id);
        return Ok(new { mensaje = "Cita eliminada correctamente" });
    }
}