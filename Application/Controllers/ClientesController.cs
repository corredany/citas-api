namespace CitasApi.Application.Controllers;

using CitasApi.Application.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly ObtenerClientesUseCase _obtenerClientesUseCase;
    private readonly ObtenerClientePorIdUseCase _obtenerClientePorIdUseCase;

    public ClientesController(
        ObtenerClientesUseCase obtenerClientesUseCase,
        ObtenerClientePorIdUseCase obtenerClientePorIdUseCase)
    {
        _obtenerClientesUseCase = obtenerClientesUseCase;
        _obtenerClientePorIdUseCase = obtenerClientePorIdUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var clientes = await _obtenerClientesUseCase.Execute();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cliente = await _obtenerClientePorIdUseCase.Execute(id);
        return Ok(cliente);
    }
}