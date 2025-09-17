using System.ComponentModel.DataAnnotations;

namespace PersonasCrud.RestV1.API.DTOs;

/// <summary>
/// DTO para modificar una persona existente desde la capa REST.
/// Incluye validaciones con DataAnnotations para entrada HTTP.
/// </summary>
/// <param name="Nombre">Nuevo nombre de la persona. Debe tener entre 2 y 100 caracteres.</param>
/// <param name="Dni">Nuevo Documento Nacional de Identidad de la persona.</param>
/// <param name="Email">Nueva dirección de correo electrónico válida.</param>
/// <param name="Edad">Nueva edad en años. Debe estar entre 18 y 120.</param>
public record DatosModificarPersonaRestDTO(
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    string Nombre,

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "El DNI debe tener entre 8 y 20 caracteres")]
    string Dni,

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    string Email,

    [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años")]
    int Edad
);
