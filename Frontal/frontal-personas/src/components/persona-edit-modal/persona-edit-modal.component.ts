import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Persona } from '../../models/frontal/persona.model';
import { ModificarPersona } from '../../models/frontal/modificar-persona.model';
import { PersonasService } from '../../services/personas.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-persona-edit-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './persona-edit-modal.component.html',
  styleUrl: './persona-edit-modal.component.css'
})
export class PersonaEditModalComponent implements OnChanges {

  @Input() persona: Persona | null = null;
  @Input() isVisible: boolean = false;

  @Output() closeModal = new EventEmitter<void>();
  @Output() personaUpdated = new EventEmitter<void>();

  editForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private personasService: PersonasService
  ) {
    this.editForm = this.createForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['persona'] && this.persona) {
      this.initializeForm();
      this.error = null;
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      nombre: ['', [Validators.required, Validators.minLength(2)]],
      dni: ['', [Validators.required, Validators.pattern(/^\d{8}[A-Z]$/)]],
      email: ['', [Validators.required, Validators.email]],
      edad: ['', [Validators.required, Validators.min(1), Validators.max(120)]]
    });
  }

  private initializeForm(): void {
    if (this.persona) {
      this.editForm.patchValue({
        nombre: this.persona.nombre,
        dni: this.persona.dni,
        email: this.persona.email,
        edad: this.persona.edad
      });
    }
  }

  cerrarModal(): void {
    this.closeModal.emit();
    this.editForm.reset();
    this.error = null;
  }

  onOverlayClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.cerrarModal();
    }
  }

  onSubmit(): void {
    if (this.editForm.valid && this.persona) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.editForm.value;
      const datosModificar: ModificarPersona = {
        nombre: formValue.nombre,
        dni: formValue.dni,
        email: formValue.email,
        edad: formValue.edad
      };

      this.personasService.update(this.persona.id, datosModificar).subscribe({
        next: () => {
          this.isLoading = false;
          this.personaUpdated.emit();
          this.cerrarModal();
        },
        error: (err) => {
          this.isLoading = false;
          this.error = 'Error al actualizar la persona. Inténtalo de nuevo.';
          console.error('Error updating persona:', err);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.editForm.controls).forEach(key => {
      const control = this.editForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string | null {
    const control = this.editForm.get(fieldName);
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
