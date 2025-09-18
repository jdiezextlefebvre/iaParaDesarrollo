import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Persona } from '../../models/frontal/persona.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-persona-detail-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './persona-detail-modal.component.html',
  styleUrl: './persona-detail-modal.component.css'
})
export class PersonaDetailModalComponent {

  @Input() persona: Persona | null = null;
  @Input() isVisible: boolean = false;

  @Output() closeModal = new EventEmitter<void>();
  @Output() edit = new EventEmitter<string>();
  @Output() delete = new EventEmitter<string>();

  cerrarModal() {
    this.closeModal.emit();
  }

  editarPersona() {
    if (this.persona) {
      this.edit.emit(this.persona.id);
    }
  }

  borrarPersona() {
    if (this.persona) {
      this.delete.emit(this.persona.id);
    }
  }

  onOverlayClick(event: Event) {
    // Cerrar modal solo si se hace clic en el overlay, no en el contenido del modal
    if (event.target === event.currentTarget) {
      this.cerrarModal();
    }
  }
}
