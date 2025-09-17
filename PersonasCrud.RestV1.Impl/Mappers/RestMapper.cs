using PersonasCrud.RestV1.API.DTOs;
using PersonasCrud.Service.API.DTOs;

namespace PersonasCrud.RestV1.Impl.Mappers;

/// <summary>
/// Mapper estático para conversión entre DTOs de la capa REST y la capa Service.
/// Mantiene la separación de responsabilidades entre capas arquitecturales.
/// </summary>
public static class RestMapper
{
    /// <summary>
    /// Convierte un DTO de nueva persona de REST a Service.
    /// </summary>
    /// <param name="restDto">DTO de REST con datos de nueva persona.</param>
    /// <returns>DTO de Service equivalente.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando restDto es null.</exception>
    public static DatosNuevaPersonaServiceDTO RestToService(DatosNuevaPersonaRestDTO restDto)
    {
        ArgumentNullException.ThrowIfNull(restDto);

        return new DatosNuevaPersonaServiceDTO
        {
            Nombre = restDto.Nombre,
            Dni = restDto.Dni,
            Email = restDto.Email,
            Edad = restDto.Edad
        };
    }

    /// <summary>
    /// Convierte un DTO de modificación de persona de REST a Service.
    /// </summary>
    /// <param name="restDto">DTO de REST con datos de modificación de persona.</param>
    /// <returns>DTO de Service equivalente.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando restDto es null.</exception>
    public static DatosModificarPersonaServiceDTO RestToService(DatosModificarPersonaRestDTO restDto)
    {
        ArgumentNullException.ThrowIfNull(restDto);

        return new DatosModificarPersonaServiceDTO
        {
            Nombre = restDto.Nombre,
            Dni = restDto.Dni,
            Email = restDto.Email,
            Edad = restDto.Edad
        };
    }

    /// <summary>
    /// Convierte un DTO de persona de Service a REST.
    /// </summary>
    /// <param name="serviceDto">DTO de Service con datos de persona.</param>
    /// <returns>DTO de REST equivalente.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando serviceDto es null.</exception>
    public static PersonaRestDTO ServiceToRest(PersonaServiceDTO serviceDto)
    {
        ArgumentNullException.ThrowIfNull(serviceDto);

        return new PersonaRestDTO(
            Id: serviceDto.Id,
            Nombre: serviceDto.Nombre,
            Dni: serviceDto.Dni,
            Email: serviceDto.Email,
            Edad: serviceDto.Edad
        );
    }

    /// <summary>
    /// Convierte una colección de DTOs de persona de Service a REST.
    /// </summary>
    /// <param name="serviceDtos">Colección de DTOs de Service.</param>
    /// <returns>Colección de DTOs de REST equivalentes.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando serviceDtos es null.</exception>
    public static IEnumerable<PersonaRestDTO> ServiceToRest(IEnumerable<PersonaServiceDTO> serviceDtos)
    {
        ArgumentNullException.ThrowIfNull(serviceDtos);

        return serviceDtos.Select(ServiceToRest);
    }
}
