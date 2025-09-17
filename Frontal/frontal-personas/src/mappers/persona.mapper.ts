import { ModificarPersona } from '../models/frontal/modificar-persona.model';
import { NuevaPersona } from '../models/frontal/nueva-persona.model';
import { Persona } from '../models/frontal/persona.model';
import { DatosModificarPersonaRestDTO } from '../models/rest/datos-modificar-persona.rest.dto';
import { DatosNuevaPersonaRestDTO } from '../models/rest/datos-nueva-persona.rest.dto';
import { PersonaRestDTO } from '../models/rest/persona.rest.dto';

/**
 * Convierte un DTO de persona del API a un modelo de persona interno.
 * Asigna valores por defecto a propiedades nulas o indefinidas para garantizar
 * la consistencia del modelo interno.
 * @param dto El DTO de persona recibido del API.
 * @returns Un objeto de tipo Persona.
 */
export function fromPersonaRestDTOToPersona(dto: PersonaRestDTO): Persona {
  return {
    id: dto.id ?? '',
    nombre: dto.nombre ?? '',
    dni: dto.dni ?? '',
    email: dto.email ?? '',
    edad: dto.edad,
  };
}

/**
 * Convierte una lista de DTOs de persona a una lista de modelos de persona internos.
 * @param dtos La lista de DTOs de persona.
 * @returns Una lista de objetos de tipo Persona.
 */
export function fromPersonaRestDTOListToPersonaList(dtos: PersonaRestDTO[]): Persona[] {
  return dtos.map(fromPersonaRestDTOToPersona);
}

/**
 * Convierte un modelo de nueva persona del frontal al DTO esperado por el API.
 * @param persona El modelo de nueva persona del formulario.
 * @returns Un DTO para crear una nueva persona.
 */
export function fromNuevaPersonaToDatosNuevaPersonaRestDTO(persona: NuevaPersona): DatosNuevaPersonaRestDTO {
  return {
    nombre: persona.nombre,
    dni: persona.dni,
    email: persona.email,
    edad: persona.edad,
  };
}

/**
 * Convierte un modelo de modificación de persona del frontal al DTO esperado por el API.
 * @param persona El modelo de modificación de persona del formulario.
 * @returns Un DTO para modificar una persona.
 */
export function fromModificarPersonaToDatosModificarPersonaRestDTO(persona: ModificarPersona): DatosModificarPersonaRestDTO {
  return {
    nombre: persona.nombre,
    dni: persona.dni,
    email: persona.email,
    edad: persona.edad,
  };
}
