import axios from 'axios';
import type { Persona } from '@/models/frontal/persona.model';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';
import type { ModificarPersona } from '@/models/frontal/modificar-persona.model';
import type { PersonaRestDTO } from '@/models/rest/persona.rest.dto';
import { fromModificarPersonaToDatosModificarPersonaRestDTO, fromNuevaPersonaToDatosNuevaPersonaRestDTO, fromPersonaRestDTOListToPersonaList, fromPersonaRestDTOToPersona } from '@/mappers/persona.mapper';

const API_URL = import.meta.env.DEV ? 'http://localhost:3000' : 'http://localhost:5278/api/v1';

export class PersonasApiRestService {
  private readonly baseUrl = `${API_URL}/personas`;

  async getAll(): Promise<Persona[]> {
    const { data } = await axios.get<PersonaRestDTO[]>(this.baseUrl);
    return fromPersonaRestDTOListToPersonaList(data);
  }

  async getById(id: string): Promise<Persona> {
    const { data } = await axios.get<PersonaRestDTO>(`${this.baseUrl}/${id}`);
    return fromPersonaRestDTOToPersona(data);
  }

  async create(persona: NuevaPersona): Promise<Persona> {
    const dto = fromNuevaPersonaToDatosNuevaPersonaRestDTO(persona);
    const { data } = await axios.post<PersonaRestDTO>(this.baseUrl, dto);
    return fromPersonaRestDTOToPersona(data);
  }

  async update(id: string, persona: ModificarPersona): Promise<Persona> {
    const dto = fromModificarPersonaToDatosModificarPersonaRestDTO(persona);
    const { data } = await axios.put<PersonaRestDTO>(`${this.baseUrl}/${id}`, dto);
    return fromPersonaRestDTOToPersona(data);
  }

  async delete(id: string): Promise<void> {
    await axios.delete(`${this.baseUrl}/${id}`);
  }
}
