using System.ComponentModel.DataAnnotations;

namespace PersonasCrud.RestV1.API.DTOs;

/// <summary>
/// DTO para representar una persona en la capa REST.
/// Optimizado para respuestas HTTP con serialización JSON.
/// </summary>
/// <param name="Id">Identificador único de la persona.</param>
/// <param name="Nombre">Nombre de la persona.</param>
/// <param name="Dni">Documento Nacional de Identidad de la persona.</param>
/// <param name="Email">Dirección de correo electrónico.</param>
/// <param name="Edad">Edad en años.</param>
public record PersonaRestDTO(
    string Id,
    string Nombre,
    string Dni,
    string Email,
    int Edad
);
