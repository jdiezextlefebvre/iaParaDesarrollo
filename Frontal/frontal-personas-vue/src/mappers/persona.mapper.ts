import type { ModificarPersona } from '@/models/frontal/modificar-persona.model';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';
import type { Persona } from '@/models/frontal/persona.model';
import type { DatosModificarPersonaRestDTO } from '@/models/rest/datos-modificar-persona.rest.dto';
import type { DatosNuevaPersonaRestDTO } from '@/models/rest/datos-nueva-persona.rest.dto';
import type { PersonaRestDTO } from '@/models/rest/persona.rest.dto';

export function fromPersonaRestDTOToPersona(dto: PersonaRestDTO): Persona {
  return {
    id: dto.id ?? '',
    nombre: dto.nombre ?? '',
    dni: dto.dni ?? '',
    email: dto.email ?? '',
    edad: dto.edad,
  };
}

export function fromPersonaRestDTOListToPersonaList(dtos: PersonaRestDTO[]): Persona[] {
  return dtos.map(fromPersonaRestDTOToPersona);
}

export function fromNuevaPersonaToDatosNuevaPersonaRestDTO(persona: NuevaPersona): DatosNuevaPersonaRestDTO {
  return {
    nombre: persona.nombre,
    dni: persona.dni,
    email: persona.email,
    edad: persona.edad,
  };
}

export function fromModificarPersonaToDatosModificarPersonaRestDTO(persona: ModificarPersona): DatosModificarPersonaRestDTO {
  const dto: DatosModificarPersonaRestDTO = {};
  if (persona.nombre !== undefined) dto.nombre = persona.nombre;
  if (persona.dni !== undefined) dto.dni = persona.dni;
  if (persona.email !== undefined) dto.email = persona.email;
  if (persona.edad !== undefined) dto.edad = persona.edad;
  return dto;
}
