using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonasCrud.RestV1.API.Controllers;
using PersonasCrud.RestV1.API.DTOs;
using PersonasCrud.RestV1.Impl.Mappers;
using PersonasCrud.Service.API.Services;

namespace PersonasCrud.RestV1.Impl.Controllers;

/// <summary>
/// Controlador REST puro para gestión de personas.
/// No contiene lógica de negocio, solo mapeo y delegación al servicio.
/// Implementa el contrato IPersonasController sin acoplamiento al servidor.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[ProducesResponseType<ErrorResponseDTO>(StatusCodes.Status500InternalServerError)]
[Tags("Gestión de Personas")]
public class PersonasController : ControllerBase
{
    private readonly IPersonasService _personasService;

    /// <summary>
    /// Constructor del controlador con inyección de dependencias.
    /// </summary>
    /// <param name="personasService">Servicio de lógica de negocio para personas.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando personasService es null.</exception>
    public PersonasController(IPersonasService personasService)
    {
        _personasService = personasService ?? throw new ArgumentNullException(nameof(personasService));
    }

    /// <summary>
    /// Crea una nueva persona en el sistema.
    /// </summary>
    /// <param name="datos">Datos de la nueva persona a crear.</param>
    /// <returns>La persona creada con su identificador asignado.</returns>
    /// <response code="201">Persona creada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="409">Conflicto con datos existentes (ej: email duplicado).</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost]
    [ProducesResponseType<PersonaRestDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponseDTO>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponseDTO>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PersonaRestDTO>> AddPersonaAsync([FromBody] DatosNuevaPersonaRestDTO datos)
    {
        // Mapear REST → Service
        var datosService = RestMapper.RestToService(datos);
        
        // Delegar la operación al servicio
        var personaCreada = await _personasService.AddPersonaAsync(datosService);
        
        // Mapear Service → REST
        var resultado = RestMapper.ServiceToRest(personaCreada);
        
        // Retornar 201 Created
        return Created($"/api/v1/personas/{resultado.Id}", resultado);
    }

    /// <summary>
    /// Obtiene una persona por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único de la persona a buscar.</param>
    /// <returns>La persona encontrada o NotFound si no existe.</returns>
    /// <response code="200">Persona encontrada exitosamente.</response>
    /// <response code="404">Persona no encontrada.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType<PersonaRestDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonaRestDTO?>> GetPersonaAsync(string id)
    {
        // Delegar la operación al servicio
        var persona = await _personasService.GetPersonaAsync(id);
        
        // Si no existe, retornar 404
        if (persona == null)
        {
            return NotFound();
        }
        
        // Mapear Service → REST y retornar 200
        var resultado = RestMapper.ServiceToRest(persona);
        return Ok(resultado);
    }

    /// <summary>
    /// Modifica los datos de una persona existente.
    /// </summary>
    /// <param name="id">Identificador único de la persona a modificar.</param>
    /// <param name="datos">Nuevos datos de la persona.</param>
    /// <returns>La persona modificada con los nuevos datos.</returns>
    /// <response code="200">Persona modificada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="404">Persona no encontrada.</response>
    /// <response code="409">Conflicto con datos existentes.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id}")]
    [ProducesResponseType<PersonaRestDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponseDTO>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponseDTO>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PersonaRestDTO>> UpdatePersonaAsync(string id, [FromBody] DatosModificarPersonaRestDTO datos)
    {
        // Mapear REST → Service
        var datosService = RestMapper.RestToService(datos);
        datosService.Id = id; // Asignar el ID del path parameter
        
        // Delegar la operación al servicio
        var personaModificada = await _personasService.UpdatePersonaAsync(datosService);
        
        // Si no existe, el servicio retorna null
        if (personaModificada == null)
        {
            return NotFound();
        }
        
        // Mapear Service → REST y retornar 200
        var resultado = RestMapper.ServiceToRest(personaModificada);
        return Ok(resultado);
    }

    /// <summary>
    /// Elimina una persona del sistema.
    /// </summary>
    /// <param name="id">Identificador único de la persona a eliminar.</param>
    /// <returns>La persona eliminada o NotFound si no existe.</returns>
    /// <response code="200">Persona eliminada exitosamente.</response>
    /// <response code="404">Persona no encontrada.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType<PersonaRestDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonaRestDTO?>> DeletePersonaAsync(string id)
    {
        // Delegar la operación al servicio
        var personaEliminada = await _personasService.DeletePersonaAsync(id);
        
        // Si no existe, retornar 404
        if (personaEliminada == null)
        {
            return NotFound();
        }
        
        // Mapear Service → REST y retornar 200
        var resultado = RestMapper.ServiceToRest(personaEliminada);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene todas las personas registradas en el sistema.
    /// </summary>
    /// <returns>Lista de todas las personas registradas.</returns>
    /// <response code="200">Lista de personas obtenida exitosamente.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType<IEnumerable<PersonaRestDTO>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonaRestDTO>>> ListPersonasAsync()
    {
        // Delegar la operación al servicio
        var personas = await _personasService.ListPersonasAsync();
        
        // Mapear Service → REST y retornar 200
        var resultado = RestMapper.ServiceToRest(personas);
        return Ok(resultado);
    }
}
