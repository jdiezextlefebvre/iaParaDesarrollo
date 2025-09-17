namespace PersonasCrud.RestV1.API.DTOs;

/// <summary>
/// DTO estándar para respuestas de error en la API REST.
/// Proporciona información estructurada sobre errores HTTP.
/// </summary>
/// <param name="Error">Tipo o código del error.</param>
/// <param name="Message">Mensaje descriptivo del error para el usuario.</param>
/// <param name="Details">Detalles técnicos adicionales del error (opcional).</param>
/// <param name="Timestamp">Fecha y hora UTC cuando ocurrió el error.</param>
/// <param name="TraceId">Identificador de trazabilidad para correlacionar logs (opcional).</param>
public record ErrorResponseDTO(
    string Error,
    string Message,
    string? Details = null,
    DateTime Timestamp = default,
    string? TraceId = null
)
{
    /// <summary>
    /// Timestamp inicializado con la fecha y hora actual en UTC si no se proporciona.
    /// </summary>
    public DateTime Timestamp { get; init; } = Timestamp == default ? DateTime.UtcNow : Timestamp;
};
