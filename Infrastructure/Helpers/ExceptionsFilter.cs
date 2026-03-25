namespace CitasApi.Infrastructure.Helpers;

using CitasApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CitaNoEncontradaException ||
            context.Exception is ClienteNoEncontradoException)
        {
            context.Result = new NotFoundObjectResult(new { error = context.Exception.Message });
            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is CitaInvalidaException ||
            context.Exception is CitaNoDisponibleException)
        {
            context.Result = new BadRequestObjectResult(new { error = context.Exception.Message });
            context.ExceptionHandled = true;
            return;
        }

        context.Result = new ObjectResult(new { error = "Error interno del servidor" })
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}