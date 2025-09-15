using PersonasCrud.Domain.API.DTOs;

namespace PersonasCrud.Domain.API.Repositories;

/// <summary>
/// Interfaz del repositorio para gestionar personas en la capa de dominio.
/// Define las operaciones CRUD básicas para la entidad Persona.
/// </summary>
public interface IPersonasRepository
{
    /// <summary>
    /// Añade una nueva persona al repositorio.
    /// </summary>
    /// <param name="datosNuevaPersona">Datos de la nueva persona a crear.</param>
    /// <returns>La persona creada con su ID asignado.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos proporcionados no son válidos.</exception>
    /// <exception cref="InvalidOperationException">Se lanza cuando ya existe una persona con el mismo DNI.</exception>
    Task<PersonaDTO> AddPersonaAsync(DatosNuevaPersonaDTO datosNuevaPersona);

    /// <summary>
    /// Obtiene una persona por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único (UUID) de la persona.</param>
    /// <returns>La persona encontrada o null si no existe.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el ID proporcionado no es válido.</exception>
    Task<PersonaDTO?> GetPersonaAsync(string id);

    /// <summary>
    /// Elimina una persona del repositorio por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único (UUID) de la persona a eliminar.</param>
    /// <returns>La persona eliminada o null si no existía.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el ID proporcionado no es válido.</exception>
    Task<PersonaDTO?> DeletePersonaAsync(string id);

    /// <summary>
    /// Actualiza los datos de una persona existente.
    /// </summary>
    /// <param name="datosModificarPersona">Datos actualizados de la persona.</param>
    /// <returns>La persona actualizada o null si no existe.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos proporcionados no son válidos.</exception>
    Task<PersonaDTO?> UpdatePersonaAsync(DatosModificarPersonaDTO datosModificarPersona);

    /// <summary>
    /// Obtiene una lista de todas las personas en el repositorio.
    /// </summary>
    /// <returns>Lista de todas las personas.</returns>
    Task<IList<PersonaDTO>> ListPersonasAsync();
}
