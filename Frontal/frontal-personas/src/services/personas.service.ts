import { Observable } from 'rxjs';
import { ModificarPersona } from '../environments/models/frontal/modificar-persona.model';
import { NuevaPersona } from '../environments/models/frontal/nueva-persona.model';
import { Persona } from '../environments/models/frontal/persona.model';

/**
 * Contrato que define las operaciones disponibles para la gestión de personas.
 * Sirve como base para la inyección de dependencias y permite intercambiar
 * la implementación (ej: API REST real, mock, json-server) sin afectar
 * a los componentes que lo consumen.
 */
export abstract class PersonasService {
  /**
   * Obtiene todas las personas.
   * @returns Un observable que emite un array de personas.
   */
  abstract getAll(): Observable<Persona[]>;

  /**
   * Obtiene una persona por su identificador único.
   * @param id El ID de la persona a buscar.
   * @returns Un observable que emite la persona encontrada.
   */
  abstract getById(id: string): Observable<Persona>;

  /**
   * Crea una nueva persona.
   * @param persona Los datos de la nueva persona.
   * @returns Un observable que emite la persona recién creada.
   */
  abstract create(persona: NuevaPersona): Observable<Persona>;

  /**
   * Actualiza una persona existente.
   * @param id El ID de la persona a actualizar.
   * @param persona Los nuevos datos para la persona.
   * @returns Un observable que emite la persona actualizada.
   */
  abstract update(id: string, persona: ModificarPersona): Observable<Persona>;

  /**
   * Elimina una persona por su identificador único.
   * @param id El ID de la persona a eliminar.
   * @returns Un observable que se completa cuando la operación finaliza.
   */
  abstract delete(id: string): Observable<void>;
}
