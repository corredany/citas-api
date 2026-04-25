namespace CitasApi.Application.Controllers;

using CitasApi.Application.Logic;
using CitasApi.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly ObtenerClientesUseCase _obtenerClientesUseCase;
    private readonly ObtenerClientePorIdUseCase _obtenerClientePorIdUseCase;
    private readonly CrearClienteUseCase _crearClienteUseCase;

    public ClientesController(
        ObtenerClientesUseCase obtenerClientesUseCase,
        ObtenerClientePorIdUseCase obtenerClientePorIdUseCase,
        CrearClienteUseCase crearClienteUseCase)
    {
        _obtenerClientesUseCase = obtenerClientesUseCase;
        _obtenerClientePorIdUseCase = obtenerClientePorIdUseCase;
        _crearClienteUseCase = crearClienteUseCase;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Crear([FromBody] CrearClienteDto dto)
    {
        var cliente = await _crearClienteUseCase.Execute(dto);
        return Ok(new { id = cliente.Id });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerTodos()
    {
        var clientes = await _obtenerClientesUseCase.Execute();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cliente = await _obtenerClientePorIdUseCase.Execute(id);
        return Ok(cliente);
    }
}
