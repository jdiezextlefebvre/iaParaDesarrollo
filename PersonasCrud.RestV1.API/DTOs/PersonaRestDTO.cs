using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PersonasCrud.RestV1.API.DTOs;

/// <summary>
/// DTO para representar una persona en la capa REST.
/// Optimizado para respuestas HTTP con serialización JSON.
/// </summary>
/// <param name="Id">Identificador único de la persona (UUID).</param>
/// <param name="Nombre">Nombre completo de la persona.</param>
/// <param name="Dni">Documento Nacional de Identidad de la persona.</param>
/// <param name="Email">Dirección de correo electrónico.</param>
/// <param name="Edad">Edad en años.</param>
/// <example>
/// {
///   "id": "123e4567-e89b-12d3-a456-426614174000",
///   "nombre": "Juan Pérez García",
///   "dni": "12345678A",
///   "email": "juan.perez@example.com",
///   "edad": 35
/// }
/// </example>
public record PersonaRestDTO(
    [Description("Identificador único de la persona (UUID)")]
    string Id,
    
    [Description("Nombre completo de la persona")]
    string Nombre,
    
    [Description("Documento Nacional de Identidad")]
    string Dni,
    
    [Description("Dirección de correo electrónico")]
    string Email,
    
    [Description("Edad en años")]
    int Edad
);
