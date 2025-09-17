import { ApplicationConfig } from '@angular/core';
import { PersonasService } from '../services/personas.service';
import { PersonasApiRestService } from '../services/personas.api-rest.service';
import { provideHttpClient } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    { provide: PersonasService, useClass: PersonasApiRestService }
  ]
};

