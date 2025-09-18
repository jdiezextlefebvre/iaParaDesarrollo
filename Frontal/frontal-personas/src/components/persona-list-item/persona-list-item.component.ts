import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Persona } from '../../environments/models/frontal/persona.model';
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

  @Output() view = new EventEmitter<string>();
  @Output() edit = new EventEmitter<string>();
  @Output() delete = new EventEmitter<string>();

  solicitarVista() {
    this.view.emit(this.persona.id);
  }

  solicitarEdicion() {
    this.edit.emit(this.persona.id);
  }

  solicitarBorrado() {
    this.delete.emit(this.persona.id);
  }
}
