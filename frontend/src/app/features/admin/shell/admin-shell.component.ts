import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-shell',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-shell.component.html',
  styleUrls: ['./admin-shell.component.scss']
})
export class AdminShellComponent implements OnInit {
  sidebarColapsado = false;
  permisosUsuario: string[] = [];

  usuarioActual = {
    nombre: 'Funcionario Autenticado',
    ci: '---',
    correo: '---'
  };

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.permisosUsuario = this.authService.obtenerPermisosDelToken();
    this.cargarDatosUsuario();
  }

  tienePermiso(permiso: string): boolean {
    return this.permisosUsuario.includes(permiso);
  }

  toggleSidebar(): void {
    this.sidebarColapsado = !this.sidebarColapsado;
  }

  cerrarSesion(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  private cargarDatosUsuario(): void {
    const token = this.authService.obtenerToken();
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.usuarioActual = {
          nombre: payload.name || 'Wilson Denis Arriola',
          ci: payload.sub || payload.nameid || '43791806',
          correo: payload.email || 'wilson.arriola@sgnet.com.uy'
        };
      } catch {
        // En caso de error de decodificación mantiene valores por defecto
      }
    }
  }
}