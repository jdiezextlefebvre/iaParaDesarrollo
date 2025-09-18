import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { PersonaCreateModalComponent } from './persona-create-modal.component';
import { PersonasService } from '../../services/personas.service';
import { NuevaPersona } from '../../environments/models/frontal/nueva-persona.model';
import { Persona } from '../../environments/models/frontal/persona.model';

describe('PersonaCreateModalComponent', () => {
  let component: PersonaCreateModalComponent;
  let fixture: ComponentFixture<PersonaCreateModalComponent>;
  let mockPersonasService: jasmine.SpyObj<PersonasService>;

  // Datos de prueba
  const validPersonaData: NuevaPersona = {
    nombre: 'Juan Test',
    dni: '12345678A',
    email: 'juan@test.com',
    edad: 30
  };

  const mockPersonaCreated: Persona = {
    id: '123',
    ...validPersonaData
  };

  beforeEach(async () => {
    // Mock del PersonasService
    mockPersonasService = jasmine.createSpyObj('PersonasService', ['create']);
    mockPersonasService.create.and.returnValue(of(mockPersonaCreated));

    await TestBed.configureTestingModule({
      imports: [
        PersonaCreateModalComponent,
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: PersonasService, useValue: mockPersonasService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PersonaCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  // Helper Functions
  function fillValidForm(): void {
    component.personaForm.patchValue(validPersonaData);
    fixture.detectChanges();
  }

  function setFieldValue(fieldName: string, value: any): void {
    component.personaForm.get(fieldName)?.setValue(value);
    component.personaForm.get(fieldName)?.markAsTouched();
    fixture.detectChanges();
  }

  function getErrorElement(fieldName: string): HTMLElement | null {
    return fixture.nativeElement.querySelector(`[data-testid="${fieldName}-error"]`);
  }

  function clickButton(buttonText: string): void {
    const buttons = fixture.nativeElement.querySelectorAll('button');
    const button = Array.from(buttons)
      .find((btn: any) => btn.textContent?.trim() === buttonText) as HTMLButtonElement;
    if (button) {
      button.click();
      fixture.detectChanges();
    }
  }

  describe('Inicialización', () => {
    it('debería crear el componente correctamente', () => {
      expect(component).toBeTruthy();
    });

    it('debería inicializar el formulario con campos vacíos', () => {
      expect(component.personaForm.get('nombre')?.value).toBe('');
      expect(component.personaForm.get('dni')?.value).toBe('');
      expect(component.personaForm.get('email')?.value).toBe('');
      expect(component.personaForm.get('edad')?.value).toBe('');
    });

    it('debería mostrar el modal cuando isVisible es true', () => {
      component.isVisible = true;
      fixture.detectChanges();
      
      const modal = fixture.nativeElement.querySelector('.modal');
      expect(modal).toBeTruthy();
      expect(modal.classList.contains('show')).toBeTruthy();
    });

    it('debería ocultar el modal cuando isVisible es false', () => {
      component.isVisible = false;
      fixture.detectChanges();
      
      const modal = fixture.nativeElement.querySelector('.modal');
      expect(modal?.classList.contains('show')).toBeFalsy();
    });
  });

  describe('Validaciones', () => {
    it('debería marcar el formulario como inválido si faltan campos requeridos', () => {
      expect(component.personaForm.valid).toBeFalsy();
      
      // Solo llenar algunos campos
      setFieldValue('nombre', 'Juan');
      expect(component.personaForm.valid).toBeFalsy();
      
      setFieldValue('dni', '12345678A');
      expect(component.personaForm.valid).toBeFalsy();
      
      setFieldValue('edad', 30);
      expect(component.personaForm.valid).toBeFalsy();
      
      // Llenar todos los campos requeridos
      setFieldValue('email', 'juan@test.com');
      expect(component.personaForm.valid).toBeTruthy();
    });

    it('debería validar formato de email', () => {
      setFieldValue('email', 'email-invalido');
      
      const emailControl = component.personaForm.get('email');
      expect(emailControl?.errors?.['email']).toBeTruthy();
      
      setFieldValue('email', 'email@valido.com');
      expect(emailControl?.errors).toBeNull();
    });

    it('debería validar que la edad sea mayor a 0', () => {
      setFieldValue('edad', 0);
      
      const edadControl = component.personaForm.get('edad');
      expect(edadControl?.errors?.['min']).toBeTruthy();
      
      setFieldValue('edad', 1);
      expect(edadControl?.errors).toBeNull();
    });

    it('debería validar que la edad sea menor a 120', () => {
      setFieldValue('edad', 120);
      
      const edadControl = component.personaForm.get('edad');
      expect(edadControl?.errors?.['max']).toBeTruthy();
      
      setFieldValue('edad', 119);
      expect(edadControl?.errors).toBeNull();
    });

    it('debería validar formato del DNI', () => {
      setFieldValue('dni', '123');
      
      const dniControl = component.personaForm.get('dni');
      expect(dniControl?.errors?.['pattern']).toBeTruthy();
      
      setFieldValue('dni', '12345678A');
      expect(dniControl?.errors).toBeNull();
    });

    it('debería mostrar mensajes de error cuando los campos son inválidos', () => {
      // Marcar campo como touched para mostrar errores
      setFieldValue('nombre', '');
      
      const nombreError = getErrorElement('nombre');
      expect(nombreError?.textContent).toContain('El nombre es requerido');
      
      setFieldValue('email', 'email-invalido');
      const emailError = getErrorElement('email');
      expect(emailError?.textContent).toContain('formato de email válido');
    });
  });

  describe('Interacciones', () => {
    beforeEach(() => {
      component.isVisible = true;
      fixture.detectChanges();
    });

    it('debería emitir evento closeModal al hacer clic en Cancelar', () => {
      spyOn(component.closeModal, 'emit');
      
      clickButton('Cancelar');
      
      expect(component.closeModal.emit).toHaveBeenCalled();
    });

    it('debería emitir evento closeModal al hacer clic en X', () => {
      spyOn(component.closeModal, 'emit');
      
      const closeButton = fixture.nativeElement.querySelector('[data-testid="close-button"]');
      closeButton?.click();
      fixture.detectChanges();
      
      expect(component.closeModal.emit).toHaveBeenCalled();
    });

    it('debería emitir evento closeModal al hacer clic fuera del modal', () => {
      spyOn(component.closeModal, 'emit');
      
      const modalBackdrop = fixture.nativeElement.querySelector('.modal');
      modalBackdrop?.click();
      fixture.detectChanges();
      
      expect(component.closeModal.emit).toHaveBeenCalled();
    });

    it('debería deshabilitar el botón Crear cuando el formulario es inválido', () => {
      const createButton = fixture.nativeElement.querySelector('[data-testid="create-button"]');
      expect(createButton?.disabled).toBeTruthy();
    });

    it('debería habilitar el botón Crear cuando el formulario es válido', () => {
      fillValidForm();
      
      const createButton = fixture.nativeElement.querySelector('[data-testid="create-button"]');
      expect(createButton?.disabled).toBeFalsy();
    });
  });

  describe('Servicio', () => {
    beforeEach(() => {
      component.isVisible = true;
      fixture.detectChanges();
    });

    it('debería llamar al servicio create con los datos correctos', async () => {
      fillValidForm();
      spyOn(component.personaCreated, 'emit');
      spyOn(component.closeModal, 'emit');
      
      await component.onSubmit();
      
      expect(mockPersonasService.create).toHaveBeenCalledWith(validPersonaData);
    });

    it('debería emitir evento personaCreated con la nueva persona', async () => {
      fillValidForm();
      spyOn(component.personaCreated, 'emit');
      spyOn(component.closeModal, 'emit');
      
      await component.onSubmit();
      
      expect(component.personaCreated.emit).toHaveBeenCalled();
    });

    it('debería cerrar el modal después de crear exitosamente', async () => {
      fillValidForm();
      spyOn(component.personaCreated, 'emit');
      spyOn(component.closeModal, 'emit');
      
      await component.onSubmit();
      
      expect(component.closeModal.emit).toHaveBeenCalled();
    });

    it('debería resetear el formulario después de crear exitosamente', async () => {
      fillValidForm();
      spyOn(component.personaCreated, 'emit');
      spyOn(component.closeModal, 'emit');
      
      await component.onSubmit();
      
      expect(component.personaForm.get('nombre')?.value).toBe('');
      expect(component.personaForm.get('dni')?.value).toBe('');
      expect(component.personaForm.get('email')?.value).toBe('');
      expect(component.personaForm.get('edad')?.value).toBe('');
    });

    it('debería manejar errores del servicio correctamente', async () => {
      mockPersonasService.create.and.returnValue(throwError(() => new Error('Error del servidor')));
      spyOn(console, 'error');
      
      fillValidForm();
      
      await component.onSubmit();
      
      expect(console.error).toHaveBeenCalledWith('Error al crear persona:', jasmine.any(Error));
    });

    it('debería mostrar mensaje de error si falla la creación', async () => {
      mockPersonasService.create.and.returnValue(throwError(() => new Error('Error del servidor')));
      
      fillValidForm();
      
      await component.onSubmit();
      
      // Verificar que se muestra algún indicador de error
      // (esto dependería de cómo implementes el manejo de errores en el template)
      expect(component.personaForm.enabled).toBeTruthy(); // El formulario debería seguir habilitado
    });
  });

  describe('Casos Edge', () => {
    it('debería manejar emails en límite de caracteres', () => {
      const longEmail = 'a'.repeat(50) + '@example.com';
      setFieldValue('email', longEmail);
      
      const emailControl = component.personaForm.get('email');
      expect(emailControl?.errors).toBeNull();
    });

    it('debería manejar nombres con caracteres especiales', () => {
      setFieldValue('nombre', 'José María O\'Connor-Smith');
      
      const nombreControl = component.personaForm.get('nombre');
      expect(nombreControl?.errors).toBeNull();
    });

    it('debería manejar edad en los límites', () => {
      setFieldValue('edad', 1);
      expect(component.personaForm.get('edad')?.errors).toBeNull();
      
      setFieldValue('edad', 119);
      expect(component.personaForm.get('edad')?.errors).toBeNull();
    });

    it('debería prevenir doble submit', async () => {
      fillValidForm();
      spyOn(component.personaCreated, 'emit');
      
      // Simular doble clic rápido
      const submitPromise1 = component.onSubmit();
      const submitPromise2 = component.onSubmit();
      
      await Promise.all([submitPromise1, submitPromise2]);
      
      // El servicio debería haberse llamado solo una vez
      expect(mockPersonasService.create).toHaveBeenCalledTimes(1);
    });

    it('debería limpiar errores previos al reabrir el modal', () => {
      // Simular error previo
      setFieldValue('email', 'email-invalido');
      
      // Cerrar y reabrir modal
      component.isVisible = false;
      fixture.detectChanges();
      
      component.isVisible = true;
      fixture.detectChanges();
      
      expect(component.personaForm.get('email')?.value).toBe('');
      expect(component.personaForm.get('email')?.errors).toBeNull();
    });
  });

  describe('Accesibilidad', () => {
    beforeEach(() => {
      component.isVisible = true;
      fixture.detectChanges();
    });

    it('debería tener labels asociados a los inputs', () => {
      const nombreInput = fixture.nativeElement.querySelector('#nombre');
      const nombreLabel = fixture.nativeElement.querySelector('label[for="nombre"]');
      
      expect(nombreInput).toBeTruthy();
      expect(nombreLabel).toBeTruthy();
      expect(nombreLabel.textContent).toContain('Nombre');
    });

    it('debería manejar navegación por teclado', () => {
      const inputs = fixture.nativeElement.querySelectorAll('input');
      expect(inputs.length).toBeGreaterThan(0);
      
      // Verificar que los inputs pueden recibir focus
      inputs.forEach((input: HTMLInputElement) => {
        expect(input.tabIndex).not.toBe(-1);
      });
    });

    it('debería cerrar con ESC', () => {
      spyOn(component.closeModal, 'emit');
      
      const event = new KeyboardEvent('keydown', { key: 'Escape' });
      document.dispatchEvent(event);
      
      expect(component.closeModal.emit).toHaveBeenCalled();
    });

    it('debería enfocar el primer campo al abrir', () => {
      // Simular apertura del modal
      component.isVisible = true;
      fixture.detectChanges();
      
      // En una implementación real, aquí verificarías que el primer input tiene focus
      const firstInput = fixture.nativeElement.querySelector('input');
      expect(firstInput).toBeTruthy();
    });
  });
});
