using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Service.API.DTOs;

namespace PersonasCrud.Service.Impl.Mappers;

/// <summary>
/// Mapeador estático para convertir entre DTOs del dominio y DTOs del servicio.
/// Actúa como muralla de contención entre las capas de dominio y servicio.
/// </summary>
public static class ServiceMapper
{
    /// <summary>
    /// Convierte un PersonaDTO del dominio a PersonaServiceDTO del servicio.
    /// </summary>
    /// <param name="domainDto">DTO del dominio a convertir.</param>
    /// <returns>DTO del servicio equivalente.</returns>
    public static PersonaServiceDTO DomainToService(PersonaDTO domainDto)
    {
        return new PersonaServiceDTO
        {
            Id = domainDto.Id,
            Nombre = domainDto.Nombre,
            Dni = domainDto.Dni,
            Email = domainDto.Email,
            Edad = domainDto.Edad
        };
    }

    /// <summary>
    /// Convierte un DatosNuevaPersonaServiceDTO del servicio a DatosNuevaPersonaDTO del dominio.
    /// </summary>
    /// <param name="serviceDto">DTO del servicio a convertir.</param>
    /// <returns>DTO del dominio equivalente.</returns>
    public static DatosNuevaPersonaDTO ServiceToDomain(DatosNuevaPersonaServiceDTO serviceDto)
    {
        return new DatosNuevaPersonaDTO
        {
            Nombre = serviceDto.Nombre,
            Dni = serviceDto.Dni,
            Email = serviceDto.Email,
            Edad = serviceDto.Edad
        };
    }

    /// <summary>
    /// Convierte un DatosModificarPersonaServiceDTO del servicio a DatosModificarPersonaDTO del dominio.
    /// </summary>
    /// <param name="serviceDto">DTO del servicio a convertir.</param>
    /// <returns>DTO del dominio equivalente.</returns>
    public static DatosModificarPersonaDTO ServiceToDomain(DatosModificarPersonaServiceDTO serviceDto)
    {
        return new DatosModificarPersonaDTO
        {
            Id = serviceDto.Id,
            Nombre = serviceDto.Nombre,
            Dni = serviceDto.Dni,
            Email = serviceDto.Email,
            Edad = serviceDto.Edad
        };
    }

    /// <summary>
    /// Convierte una lista de PersonaDTO del dominio a una lista de PersonaServiceDTO del servicio.
    /// </summary>
    /// <param name="domainDtos">Lista de DTOs del dominio a convertir.</param>
    /// <returns>Lista de DTOs del servicio equivalente.</returns>
    public static IList<PersonaServiceDTO> DomainToService(IList<PersonaDTO> domainDtos)
    {
        return domainDtos.Select(DomainToService).ToList();
    }
}
