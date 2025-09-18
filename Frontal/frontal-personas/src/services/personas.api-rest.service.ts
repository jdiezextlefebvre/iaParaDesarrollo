import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';
import { environment } from '../environments/environment';
import { fromPersonaRestDTOListToPersonaList, fromPersonaRestDTOToPersona, fromNuevaPersonaToDatosNuevaPersonaRestDTO, fromModificarPersonaToDatosModificarPersonaRestDTO } from '../mappers/persona.mapper';
import { ModificarPersona } from '../environments/models/frontal/modificar-persona.model';
import { NuevaPersona } from '../environments/models/frontal/nueva-persona.model';
import { Persona } from '../environments/models/frontal/persona.model';
import { PersonaRestDTO } from '../environments/models/rest/persona.rest.dto';
import { PersonasService } from './personas.service';

@Injectable({
  providedIn: 'root'
})
export class PersonasApiRestService extends PersonasService {

  private readonly apiUrl = `${environment.apiUrl}/personas`;

  constructor(private readonly http: HttpClient) {
    super();
  }

  override getAll(): Observable<Persona[]> {
    return this.http.get<PersonaRestDTO[]>(this.apiUrl).pipe(
      map(fromPersonaRestDTOListToPersonaList),
      catchError((error) => {
        console.error('Error al obtener personas:', error);
        return of([]);
      })
    );
  }

  override getById(id: string): Observable<Persona> {
    return this.http.get<PersonaRestDTO>(`${this.apiUrl}/${id}`).pipe(
      map(fromPersonaRestDTOToPersona),
      catchError((error) => {
        console.error(`Error al obtener la persona con id ${id}:`, error);
        throw error;
      })
    );
  }

  override create(persona: NuevaPersona): Observable<Persona> {
    const personaDTO = fromNuevaPersonaToDatosNuevaPersonaRestDTO(persona);
    return this.http.post<PersonaRestDTO>(this.apiUrl, personaDTO).pipe(
      map(fromPersonaRestDTOToPersona),
      catchError((error) => {
        console.error('Error al crear la persona:', error);
        throw error;
      })
    );
  }

  override update(id: string, persona: ModificarPersona): Observable<Persona> {
    const personaDTO = fromModificarPersonaToDatosModificarPersonaRestDTO(persona);
    return this.http.put<PersonaRestDTO>(`${this.apiUrl}/${id}`, personaDTO).pipe(
      map(fromPersonaRestDTOToPersona),
      catchError((error) => {
        console.error(`Error al actualizar la persona con id ${id}:`, error);
        throw error;
      })
    );
  }

  override delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error(`Error al eliminar la persona con id ${id}:`, error);
        throw error;
      })
    );
  }
}
