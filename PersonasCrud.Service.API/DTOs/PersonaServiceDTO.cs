namespace PersonasCrud.Service.API.DTOs;

/// <summary>
/// DTO de servicio que representa una persona completa para transferencia entre capas de servicio.
/// </summary>
public class PersonaServiceDTO
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
