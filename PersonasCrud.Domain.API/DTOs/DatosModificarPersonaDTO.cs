namespace PersonasCrud.Domain.API.DTOs;

/// <summary>
/// DTO con los datos necesarios para modificar una persona existente.
/// </summary>
public class DatosModificarPersonaDTO
{
    /// <summary>
    /// Identificador único universal de la persona a modificar (UUID).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo nombre completo de la persona.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo Documento Nacional de Identidad de la persona.
    /// </summary>
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Nueva dirección de correo electrónico de la persona.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nueva edad de la persona en años.
    /// </summary>
    public int Edad { get; set; }
}
