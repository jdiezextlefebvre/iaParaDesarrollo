import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NuevaPersona } from '../../models/frontal/nueva-persona.model';
import { PersonasService } from '../../services/personas.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-persona-create-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './persona-create-modal.component.html',
  styleUrl: './persona-create-modal.component.css'
})
export class PersonaCreateModalComponent {

  @Input() isVisible: boolean = false;

  @Output() closeModal = new EventEmitter<void>();
  @Output() personaCreated = new EventEmitter<void>();

  personaForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private personasService: PersonasService
  ) {
    this.personaForm = this.createForm();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      nombre: ['', [Validators.required, Validators.minLength(2)]],
      dni: ['', [Validators.required, Validators.pattern(/^\d{8}[A-Z]$/)]],
      email: ['', [Validators.required, Validators.email]],
      edad: ['', [Validators.required, Validators.min(1), Validators.max(120)]]
    });
  }

  cerrarModal(): void {
    this.closeModal.emit();
    this.personaForm.reset();
    this.error = null;
  }

  onOverlayClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.cerrarModal();
    }
  }

  onSubmit(): void {
    if (this.personaForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.personaForm.value;
      const nuevaPersona: NuevaPersona = {
        nombre: formValue.nombre,
        dni: formValue.dni,
        email: formValue.email,
        edad: formValue.edad
      };

      this.personasService.create(nuevaPersona).subscribe({
        next: () => {
          this.isLoading = false;
          this.personaCreated.emit();
          this.cerrarModal();
        },
        error: (err) => {
          this.isLoading = false;
          this.error = 'Error al crear la persona. Inténtalo de nuevo.';
          console.error('Error creating persona:', err);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.personaForm.controls).forEach(key => {
      const control = this.personaForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string | null {
    const control = this.personaForm.get(fieldName);
    if (control?.touched && control?.errors) {
      if (control.errors['required']) return `${fieldName} es requerido`;
      if (control.errors['email']) return 'Email inválido';
      if (control.errors['minlength']) return `${fieldName} debe tener al menos ${control.errors['minlength'].requiredLength} caracteres`;
      if (control.errors['pattern']) return 'DNI debe tener formato 12345678A';
      if (control.errors['min']) return 'La edad debe ser mayor a 0';
      if (control.errors['max']) return 'La edad debe ser menor a 120';
    }
    return null;
  }
}
