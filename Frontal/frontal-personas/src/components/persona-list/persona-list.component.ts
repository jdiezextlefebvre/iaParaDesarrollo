import { Component, OnInit } from '@angular/core';
import { Persona } from '../../environments/models/frontal/persona.model';
import { PersonasService } from '../../services/personas.service';
import { CommonModule } from '@angular/common';
import { PersonaListItemComponent } from '../persona-list-item/persona-list-item.component';
import { PersonaDetailModalComponent } from '../persona-detail-modal/persona-detail-modal.component';
import { PersonaEditModalComponent } from '../persona-edit-modal/persona-edit-modal.component';
import { PersonaCreateModalComponent } from '../persona-create-modal/persona-create-modal.component';

@Component({
  selector: 'app-persona-list',
  standalone: true,
  imports: [CommonModule, PersonaListItemComponent, PersonaDetailModalComponent, PersonaEditModalComponent, PersonaCreateModalComponent],
  templateUrl: './persona-list.component.html',
  styleUrl: './persona-list.component.css'
})
export class PersonaListComponent implements OnInit {

  personas: Persona[] = [];
  isLoading = true;
  error: string | null = null;
  personaSeleccionada: Persona | null = null;
  modalVisible = false;
  editModalVisible = false;
  createModalVisible = false;

  constructor(private readonly personasService: PersonasService) {}

  ngOnInit(): void {
    this.cargarPersonas();
  }

  cargarPersonas(): void {
    this.isLoading = true;
    this.error = null;
    this.personasService.getAll().subscribe({
      next: (data) => {
        this.personas = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar las personas. Inténtalo de nuevo más tarde.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  onVerPersona(id: string): void {
    console.log(`LISTA: Ver detalle de la persona con ID: ${id}`);
    const persona = this.personas.find(p => p.id === id);
    if (persona) {
      this.personaSeleccionada = persona;
      this.modalVisible = true;
    }
  }

  onEditarPersona(id: string): void {
    console.log(`LISTA: Editar persona con ID: ${id}`);
    const persona = this.personas.find(p => p.id === id);
    if (persona) {
      this.personaSeleccionada = persona;
      this.editModalVisible = true;
    }
  }

  onBorrarPersona(id: string): void {
    console.log(`LISTA: Borrando persona con ID: ${id}`);
    this.personasService.delete(id).subscribe({
      next: () => {
        console.log(`Persona con ID: ${id} borrada con éxito.`);
        // Actualiza la lista local para reflejar el cambio en la UI
        this.personas = this.personas.filter(p => p.id !== id);
        // Si la persona borrada era la seleccionada, cerrar el modal
        if (this.personaSeleccionada?.id === id) {
          this.cerrarModal();
        }
      },
      error: (err) => {
        console.error(`Error al borrar la persona con ID: ${id}`, err);
        // Opcional: mostrar un mensaje de error al usuario
      }
    });
  }

  // Métodos para el modal
  cerrarModal(): void {
    this.modalVisible = false;
    this.personaSeleccionada = null;
  }

  onEditarDesdeModal(id: string): void {
    console.log(`MODAL: Editar persona con ID: ${id}`);
    this.cerrarModal();
    this.onEditarPersona(id);
  }

  onBorrarDesdeModal(id: string): void {
    console.log(`MODAL: Borrar persona con ID: ${id}`);
    this.onBorrarPersona(id); // Reutiliza la lógica de borrado
  }

  // Métodos para el modal de edición
  cerrarEditModal(): void {
    this.editModalVisible = false;
    this.personaSeleccionada = null;
  }

  onPersonaActualizada(): void {
    console.log('Persona actualizada, recargando lista...');
    this.cargarPersonas(); // Recargar la lista para mostrar los cambios
  }

  // Métodos para el modal de creación
  abrirModalCrear(): void {
    this.createModalVisible = true;
  }

  cerrarCreateModal(): void {
    this.createModalVisible = false;
  }

  onPersonaCreada(): void {
    console.log('Persona creada, recargando lista...');
    this.cargarPersonas(); // Recargar la lista para mostrar la nueva persona
  }
}
