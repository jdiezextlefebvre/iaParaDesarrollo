namespace PersonasCrud.Domain.API.DTOs;

/// <summary>
/// DTO que representa una persona para transferencia entre capas.
/// </summary>
public class PersonaDTO
{
    /// <summary>
    /// Identificador único universal de la persona (UUID).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo de la persona.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Documento Nacional de Identidad de la persona.
    /// </summary>
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de correo electrónico de la persona.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Edad de la persona en años.
    /// </summary>
    public int Edad { get; set; }
}
