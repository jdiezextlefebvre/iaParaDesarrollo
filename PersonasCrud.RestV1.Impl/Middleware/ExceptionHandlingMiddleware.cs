using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonasCrud.RestV1.API.DTOs;

namespace PersonasCrud.RestV1.Impl.Middleware;

/// <summary>
/// Middleware para manejo centralizado de excepciones en la capa REST.
/// Captura excepciones y las convierte en respuestas HTTP estructuradas.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Constructor del middleware de manejo de excepciones.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline.</param>
    /// <param name="logger">Logger para registrar eventos de excepción.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando algún parámetro es null.</exception>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Ejecuta el middleware capturando y manejando excepciones.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición actual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Maneja excepciones de validación (ValidationException).
    /// Las convierte en respuestas HTTP 400 Bad Request.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición.</param>
    /// <param name="ex">Excepción de validación capturada.</param>
    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        _logger.LogWarning(ex, "Validation error occurred. TraceId: {TraceId}", context.TraceIdentifier);
        
        var response = new ErrorResponseDTO(
            Error: "ValidationError",
            Message: "Los datos proporcionados no son válidos",
            Details: ex.Message,
            TraceId: context.TraceIdentifier
        );

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Maneja excepciones de lógica de negocio (InvalidOperationException).
    /// Las convierte en respuestas HTTP 409 Conflict.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición.</param>
    /// <param name="ex">Excepción de negocio capturada.</param>
    private async Task HandleBusinessExceptionAsync(HttpContext context, InvalidOperationException ex)
    {
        _logger.LogWarning(ex, "Business logic error occurred. TraceId: {TraceId}", context.TraceIdentifier);
        
        var response = new ErrorResponseDTO(
            Error: "BusinessError",
            Message: "No se puede completar la operación solicitada",
            Details: ex.Message,
            TraceId: context.TraceIdentifier
        );

        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Maneja excepciones genéricas no controladas.
    /// Las convierte en respuestas HTTP 500 Internal Server Error.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición.</param>
    /// <param name="ex">Excepción genérica capturada.</param>
    private async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unexpected error occurred. TraceId: {TraceId}", context.TraceIdentifier);
        
        var response = new ErrorResponseDTO(
            Error: "InternalError",
            Message: "Ha ocurrido un error interno del servidor",
            Details: null, // No exponemos detalles técnicos al cliente
            TraceId: context.TraceIdentifier
        );

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Escribe una respuesta JSON estructurada al cliente.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición.</param>
    /// <param name="response">DTO de respuesta de error a serializar.</param>
    private static async Task WriteJsonResponseAsync(HttpContext context, ErrorResponseDTO response)
    {
        context.Response.ContentType = "application/json";
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        var json = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(json);
    }
}
