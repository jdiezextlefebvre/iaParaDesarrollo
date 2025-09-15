namespace PersonasCrud.Domain.API.DTOs;

/// <summary>
/// DTO con los datos necesarios para crear una nueva persona.
/// </summary>
public class DatosNuevaPersonaDTO
{
    /// <summary>
    /// Nombre completo de la nueva persona.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Documento Nacional de Identidad de la nueva persona.
    /// </summary>
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de correo electrónico de la nueva persona.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Edad de la nueva persona en años.
    /// </summary>
    public int Edad { get; set; }
}
