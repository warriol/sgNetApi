import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="welcome-container">
      <div class="welcome-card">
        <h2>¡Bienvenido al Sistema sgNet!</h2>
        <p>Seleccione una opción del menú superior para comenzar a operar en la plataforma.</p>
      </div>
    </div>
  `,
  styles: [`
    .welcome-container { padding: 2rem; display: flex; justify-content: center; }
    .welcome-card { background: #fff; padding: 2.5rem; border-radius: 8px; border: 1px solid #e2e8f0; text-align: center; max-width: 600px; }
    h2 { color: #0f172a; margin-bottom: 0.5rem; }
    p { color: #64748b; margin: 0; }
  `]
})
export class DashboardComponent {}