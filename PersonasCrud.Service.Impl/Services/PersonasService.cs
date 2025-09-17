using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Service.API.DTOs;
using PersonasCrud.Service.API.Services;
using PersonasCrud.Service.Impl.Mappers;

namespace PersonasCrud.Service.Impl.Services;

/// <summary>
/// Implementación del servicio para gestionar personas.
/// Actúa como orquestador de la lógica de negocio y coordina las operaciones del dominio.
/// </summary>
public class PersonasService : IPersonasService
{
    private readonly IPersonasRepository _personasRepository;

    /// <summary>
    /// Constructor que inicializa el servicio con sus dependencias.
    /// </summary>
    /// <param name="personasRepository">Repositorio del dominio para gestionar personas.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando el repositorio es null.</exception>
    public PersonasService(IPersonasRepository personasRepository)
    {
        _personasRepository = personasRepository ?? throw new ArgumentNullException(nameof(personasRepository));
    }

    /// <inheritdoc />
    public async Task<PersonaServiceDTO> AddPersonaAsync(DatosNuevaPersonaServiceDTO datosNuevaPersona)
    {
        // Convertir DTO del servicio a DTO del dominio
        var datosNuevaPersonaDomain = ServiceMapper.ServiceToDomain(datosNuevaPersona);
        
        // Delegar la operación al repositorio del dominio
        var personaCreada = await _personasRepository.AddPersonaAsync(datosNuevaPersonaDomain);
        
        // Convertir el resultado del dominio a DTO del servicio
        return ServiceMapper.DomainToService(personaCreada);
    }

    /// <inheritdoc />
    public async Task<PersonaServiceDTO?> GetPersonaAsync(string id)
    {
        // Delegar la operación al repositorio del dominio
        var persona = await _personasRepository.GetPersonaAsync(id);
        
        // Convertir el resultado si existe
        return persona != null ? ServiceMapper.DomainToService(persona) : null;
    }

    /// <inheritdoc />
    public async Task<PersonaServiceDTO?> UpdatePersonaAsync(DatosModificarPersonaServiceDTO datosModificarPersona)
    {
        // Convertir DTO del servicio a DTO del dominio
        var datosModificarPersonaDomain = ServiceMapper.ServiceToDomain(datosModificarPersona);
        
        // Delegar la operación al repositorio del dominio
        var personaActualizada = await _personasRepository.UpdatePersonaAsync(datosModificarPersonaDomain);
        
        // Convertir el resultado si existe
        return personaActualizada != null ? ServiceMapper.DomainToService(personaActualizada) : null;
    }

    /// <inheritdoc />
    public async Task<PersonaServiceDTO?> DeletePersonaAsync(string id)
    {
        // Delegar la operación al repositorio del dominio
        var personaEliminada = await _personasRepository.DeletePersonaAsync(id);
        
        // Convertir el resultado si existe
        return personaEliminada != null ? ServiceMapper.DomainToService(personaEliminada) : null;
    }

    /// <inheritdoc />
    public async Task<IList<PersonaServiceDTO>> ListPersonasAsync()
    {
        // Delegar la operación al repositorio del dominio
        var personas = await _personasRepository.ListPersonasAsync();
        
        // Convertir la lista completa
        return ServiceMapper.DomainToService(personas);
    }
}
