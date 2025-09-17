import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Persona } from '../../models/frontal/persona.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-persona-list-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './persona-list-item.component.html',
  styleUrl: './persona-list-item.component.css'
})
export class PersonaListItemComponent {

  @Input({ required: true }) persona!: Persona;

  @Output() onView = new EventEmitter<string>();
  @Output() onEdit = new EventEmitter<string>();
  @Output() onDelete = new EventEmitter<string>();

  solicitarVista() {
    this.onView.emit(this.persona.id);
  }

  solicitarEdicion() {
    this.onEdit.emit(this.persona.id);
  }

  solicitarBorrado() {
    this.onDelete.emit(this.persona.id);
  }
}
