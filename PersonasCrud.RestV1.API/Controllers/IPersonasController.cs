using Microsoft.AspNetCore.Mvc;
using PersonasCrud.RestV1.API.DTOs;

namespace PersonasCrud.RestV1.API.Controllers;

/// <summary>
/// Contrato para el controlador REST de personas.
/// Define operaciones CRUD puras sin lógica de servidor ni configuración.
/// </summary>
public interface IPersonasController
{
    /// <summary>
    /// Crea una nueva persona en el sistema.
    /// </summary>
    /// <param name="datos">Datos de la nueva persona a crear.</param>
    /// <returns>La persona creada con su identificador asignado.</returns>
    Task<ActionResult<PersonaRestDTO>> AddPersonaAsync(DatosNuevaPersonaRestDTO datos);

    /// <summary>
    /// Obtiene una persona por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único de la persona a buscar.</param>
    /// <returns>La persona encontrada o null si no existe.</returns>
    Task<ActionResult<PersonaRestDTO?>> GetPersonaAsync(string id);

    /// <summary>
    /// Modifica los datos de una persona existente.
    /// </summary>
    /// <param name="id">Identificador único de la persona a modificar.</param>
    /// <param name="datos">Nuevos datos de la persona.</param>
    /// <returns>La persona modificada con los nuevos datos.</returns>
    Task<ActionResult<PersonaRestDTO>> UpdatePersonaAsync(string id, DatosModificarPersonaRestDTO datos);

    /// <summary>
    /// Elimina una persona del sistema.
    /// </summary>
    /// <param name="id">Identificador único de la persona a eliminar.</param>
    /// <returns>La persona eliminada o null si no existe.</returns>
    Task<ActionResult<PersonaRestDTO?>> DeletePersonaAsync(string id);

    /// <summary>
    /// Obtiene todas las personas registradas en el sistema.
    /// </summary>
    /// <returns>Lista de todas las personas registradas.</returns>
    Task<ActionResult<IEnumerable<PersonaRestDTO>>> ListPersonasAsync();
}
