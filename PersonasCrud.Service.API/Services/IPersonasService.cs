using PersonasCrud.Service.API.DTOs;

namespace PersonasCrud.Service.API.Services;

/// <summary>
/// Interfaz del servicio para gestionar personas en la capa de servicio.
/// Define las operaciones de negocio para la entidad Persona.
/// </summary>
public interface IPersonasService
{
    /// <summary>
    /// Añade una nueva persona al sistema.
    /// </summary>
    /// <param name="datosNuevaPersona">Datos de la nueva persona a crear.</param>
    /// <returns>La persona creada con su ID asignado.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos proporcionados no son válidos.</exception>
    /// <exception cref="InvalidOperationException">Se lanza cuando ya existe una persona con el mismo DNI o Email.</exception>
    Task<PersonaServiceDTO> AddPersonaAsync(DatosNuevaPersonaServiceDTO datosNuevaPersona);

    /// <summary>
    /// Obtiene una persona por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único (UUID) de la persona.</param>
    /// <returns>La persona encontrada o null si no existe.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el ID proporcionado no es válido.</exception>
    Task<PersonaServiceDTO?> GetPersonaAsync(string id);

    /// <summary>
    /// Actualiza los datos de una persona existente.
    /// </summary>
    /// <param name="datosModificarPersona">Datos actualizados de la persona.</param>
    /// <returns>La persona actualizada o null si no existe.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos proporcionados no son válidos.</exception>
    /// <exception cref="InvalidOperationException">Se lanza cuando ya existe otra persona con el mismo DNI o Email.</exception>
    Task<PersonaServiceDTO?> UpdatePersonaAsync(DatosModificarPersonaServiceDTO datosModificarPersona);

    /// <summary>
    /// Elimina una persona del sistema por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único (UUID) de la persona a eliminar.</param>
    /// <returns>La persona eliminada o null si no existía.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el ID proporcionado no es válido.</exception>
    Task<PersonaServiceDTO?> DeletePersonaAsync(string id);

    /// <summary>
    /// Obtiene una lista de todas las personas en el sistema.
    /// </summary>
    /// <returns>Lista de todas las personas ordenada por nombre.</returns>
    Task<IList<PersonaServiceDTO>> ListPersonasAsync();
}
