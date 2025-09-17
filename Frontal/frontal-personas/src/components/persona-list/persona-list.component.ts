import { Component, OnInit } from '@angular/core';
import { Persona } from '../../models/frontal/persona.model';
import { PersonasService } from '../../services/personas.service';
import { CommonModule } from '@angular/common';
import { PersonaListItemComponent } from '../persona-list-item/persona-list-item.component';

@Component({
  selector: 'app-persona-list',
  standalone: true,
  imports: [CommonModule, PersonaListItemComponent],
  templateUrl: './persona-list.component.html',
  styleUrl: './persona-list.component.css'
})
export class PersonaListComponent implements OnInit {

  personas: Persona[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(private personasService: PersonasService) {}

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
    // Aquí iría la lógica de navegación al detalle
  }

  onEditarPersona(id: string): void {
    console.log(`LISTA: Editar persona con ID: ${id}`);
    // Aquí iría la lógica de navegación a la edición
  }

  onBorrarPersona(id: string): void {
    console.log(`LISTA: Borrando persona con ID: ${id}`);
    this.personasService.delete(id).subscribe({
      next: () => {
        console.log(`Persona con ID: ${id} borrada con éxito.`);
        // Actualiza la lista local para reflejar el cambio en la UI
        this.personas = this.personas.filter(p => p.id !== id);
      },
      error: (err) => {
        console.error(`Error al borrar la persona con ID: ${id}`, err);
        // Opcional: mostrar un mensaje de error al usuario
      }
    });
  }
}
