using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.Impl.Entities;

namespace PersonasCrud.Domain.Impl.Mappers;

/// <summary>
/// Mapeador entre DTOs y entidades de persona.
/// </summary>
public static class PersonasMapper
{
    /// <summary>
    /// Convierte una PersonaEntity a PersonaDTO.
    /// </summary>
    /// <param name="entity">Entidad de persona a convertir.</param>
    /// <returns>DTO de persona.</returns>
    public static PersonaDTO EntityToDto(PersonaEntity entity)
    {
        return new PersonaDTO
        {
            Id = entity.Uuid,
            Nombre = entity.Nombre,
            Dni = entity.Dni,
            Email = entity.Email,
            Edad = entity.Edad
        };
    }

    /// <summary>
    /// Convierte un PersonaDTO a PersonaEntity.
    /// </summary>
    /// <param name="dto">DTO de persona a convertir.</param>
    /// <param name="generateNewUuid">Si true, genera un nuevo UUID para la entidad.</param>
    /// <returns>Entidad de persona.</returns>
    public static PersonaEntity DtoToEntity(PersonaDTO dto, bool generateNewUuid = false)
    {
        return new PersonaEntity
        {
            Uuid = generateNewUuid ? Guid.NewGuid().ToString() : dto.Id,
            Nombre = dto.Nombre,
            Dni = dto.Dni,
            Email = dto.Email,
            Edad = dto.Edad
        };
    }

    /// <summary>
    /// Convierte un DatosNuevaPersonaDTO a PersonaEntity.
    /// </summary>
    /// <param name="dto">DTO con datos de nueva persona.</param>
    /// <returns>Entidad de persona con UUID generado automáticamente.</returns>
    public static PersonaEntity NuevaPersonaDtoToEntity(DatosNuevaPersonaDTO dto)
    {
        return new PersonaEntity
        {
            Uuid = Guid.NewGuid().ToString(),
            Nombre = dto.Nombre,
            Dni = dto.Dni,
            Email = dto.Email,
            Edad = dto.Edad
        };
    }

    /// <summary>
    /// Actualiza una PersonaEntity existente con los datos de un DatosModificarPersonaDTO.
    /// </summary>
    /// <param name="entity">Entidad existente a actualizar.</param>
    /// <param name="dto">DTO con los nuevos datos.</param>
    public static void UpdateEntityFromModificarDto(PersonaEntity entity, DatosModificarPersonaDTO dto)
    {
        entity.Nombre = dto.Nombre;
        entity.Dni = dto.Dni;
        entity.Email = dto.Email;
        entity.Edad = dto.Edad;
    }
}
