import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { PersonasApiRestService } from './personas.api-rest.service';
import { PersonasService } from './personas.service';
import { PersonaRestDTO } from '../environments/models/rest/persona.rest.dto';
import { Persona } from '../environments/models/frontal/persona.model';
import { environment } from '../environments/environment';
import { firstValueFrom } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';

describe('PersonasApiRestService - getAll', () => {
  let service: PersonasApiRestService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrl}/personas`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: PersonasService, useClass: PersonasApiRestService }
      ]
    });
    service = TestBed.inject(PersonasService) as PersonasApiRestService;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería devolver una lista de personas mapeada correctamente', async () => {
    const mockPersonasDTO: PersonaRestDTO[] = [
      { id: '1', nombre: 'Juan', dni: '123A', email: 'juan@test.com', edad: 30 },
      { id: '2', nombre: null, dni: '456B', email: null, edad: 25 }
    ];

    const expectedPersonas: Persona[] = [
      { id: '1', nombre: 'Juan', dni: '123A', email: 'juan@test.com', edad: 30 },
      { id: '2', nombre: '', dni: '456B', email: '', edad: 25 }
    ];

    const personasPromise = firstValueFrom(service.getAll());

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPersonasDTO);

    const personas = await personasPromise;
    expect(personas.length).toBe(2);
    expect(personas).toEqual(expectedPersonas);
  });

  it('debería devolver una lista vacía si el API no devuelve personas', async () => {
    const personasPromise = firstValueFrom(service.getAll());

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush([]);

    const personas = await personasPromise;
    expect(personas).toEqual([]);
  });

  it('debería gestionar un error HTTP y devolver una lista vacía', async () => {
    const personasPromise = firstValueFrom(service.getAll());

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    
    req.flush('Error del servidor', { status: 500, statusText: 'Internal Server Error' });

    const personas = await personasPromise;
    expect(personas).toEqual([]);
  });
});