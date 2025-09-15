using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Mappers;

namespace PersonasCrud.Domain.Impl.Repositories;

/// <summary>
/// Implementación del repositorio de personas usando Entity Framework.
/// </summary>
public class PersonasRepositoryEF : IPersonasRepository
{
    private readonly PersonasDbContext _context;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public PersonasRepositoryEF(PersonasDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<PersonaDTO> AddPersonaAsync(DatosNuevaPersonaDTO datosNuevaPersona)
    {
        if (datosNuevaPersona == null)
            throw new ArgumentNullException(nameof(datosNuevaPersona));

        if (string.IsNullOrWhiteSpace(datosNuevaPersona.Nombre))
            throw new ArgumentException("El nombre es obligatorio", nameof(datosNuevaPersona));

        if (string.IsNullOrWhiteSpace(datosNuevaPersona.Dni))
            throw new ArgumentException("El DNI es obligatorio", nameof(datosNuevaPersona));

        if (string.IsNullOrWhiteSpace(datosNuevaPersona.Email))
            throw new ArgumentException("El email es obligatorio", nameof(datosNuevaPersona));

        if (datosNuevaPersona.Edad < 0 || datosNuevaPersona.Edad > 150)
            throw new ArgumentException("La edad debe estar entre 0 y 150 años", nameof(datosNuevaPersona));

        // Verificar si ya existe una persona con el mismo DNI
        var existingPersonByDni = await _context.Personas
            .FirstOrDefaultAsync(p => p.Dni == datosNuevaPersona.Dni);
        
        if (existingPersonByDni != null)
            throw new InvalidOperationException($"Ya existe una persona con el DNI {datosNuevaPersona.Dni}");

        // Verificar si ya existe una persona con el mismo email
        var existingPersonByEmail = await _context.Personas
            .FirstOrDefaultAsync(p => p.Email == datosNuevaPersona.Email);
        
        if (existingPersonByEmail != null)
            throw new InvalidOperationException($"Ya existe una persona con el email {datosNuevaPersona.Email}");

        // Crear y añadir la nueva entidad
        var nuevaPersonaEntity = PersonasMapper.NuevaPersonaDtoToEntity(datosNuevaPersona);
        
        _context.Personas.Add(nuevaPersonaEntity);
        await _context.SaveChangesAsync();

        return PersonasMapper.EntityToDto(nuevaPersonaEntity);
    }

    /// <inheritdoc />
    public async Task<PersonaDTO?> GetPersonaAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El ID no puede ser nulo o vacío", nameof(id));

        var personaEntity = await _context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == id);

        return personaEntity != null ? PersonasMapper.EntityToDto(personaEntity) : null;
    }

    /// <inheritdoc />
    public async Task<PersonaDTO?> DeletePersonaAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El ID no puede ser nulo o vacío", nameof(id));

        var personaEntity = await _context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == id);

        if (personaEntity == null)
            return null;

        var personaDto = PersonasMapper.EntityToDto(personaEntity);
        
        _context.Personas.Remove(personaEntity);
        await _context.SaveChangesAsync();

        return personaDto;
    }

    /// <inheritdoc />
    public async Task<PersonaDTO?> UpdatePersonaAsync(DatosModificarPersonaDTO datosModificarPersona)
    {
        if (datosModificarPersona == null)
            throw new ArgumentNullException(nameof(datosModificarPersona));

        if (string.IsNullOrWhiteSpace(datosModificarPersona.Id))
            throw new ArgumentException("El ID es obligatorio", nameof(datosModificarPersona));

        if (string.IsNullOrWhiteSpace(datosModificarPersona.Nombre))
            throw new ArgumentException("El nombre es obligatorio", nameof(datosModificarPersona));

        if (string.IsNullOrWhiteSpace(datosModificarPersona.Dni))
            throw new ArgumentException("El DNI es obligatorio", nameof(datosModificarPersona));

        if (string.IsNullOrWhiteSpace(datosModificarPersona.Email))
            throw new ArgumentException("El email es obligatorio", nameof(datosModificarPersona));

        if (datosModificarPersona.Edad < 0 || datosModificarPersona.Edad > 150)
            throw new ArgumentException("La edad debe estar entre 0 y 150 años", nameof(datosModificarPersona));

        var personaEntity = await _context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == datosModificarPersona.Id);

        if (personaEntity == null)
            return null;

        // Verificar si ya existe otra persona con el mismo DNI
        var existingPersonByDni = await _context.Personas
            .FirstOrDefaultAsync(p => p.Dni == datosModificarPersona.Dni && p.Uuid != datosModificarPersona.Id);
        
        if (existingPersonByDni != null)
            throw new InvalidOperationException($"Ya existe otra persona con el DNI {datosModificarPersona.Dni}");

        // Verificar si ya existe otra persona con el mismo email
        var existingPersonByEmail = await _context.Personas
            .FirstOrDefaultAsync(p => p.Email == datosModificarPersona.Email && p.Uuid != datosModificarPersona.Id);
        
        if (existingPersonByEmail != null)
            throw new InvalidOperationException($"Ya existe otra persona con el email {datosModificarPersona.Email}");

        // Actualizar la entidad
        PersonasMapper.UpdateEntityFromModificarDto(personaEntity, datosModificarPersona);
        
        await _context.SaveChangesAsync();

        return PersonasMapper.EntityToDto(personaEntity);
    }

    /// <inheritdoc />
    public async Task<IList<PersonaDTO>> ListPersonasAsync()
    {
        var personasEntities = await _context.Personas
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return personasEntities
            .Select(PersonasMapper.EntityToDto)
            .ToList();
    }
}
