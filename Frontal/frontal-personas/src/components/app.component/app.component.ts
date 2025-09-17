import { Component } from '@angular/core';
import { PersonaListComponent } from '../persona-list/persona-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PersonaListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontal-personas';
}
