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
  rolesUsuario: string[] = [];
  permisosUsuario: string[] = [];

  usuarioActual = {
    nombre: 'Funcionario',
    ci: '---'
  };

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.permisosUsuario = this.authService.obtenerPermisosDelToken();
    this.rolesUsuario = this.authService.obtenerRolesDelToken();
    this.cargarDatosUsuario();
  }

  tieneRol(rol: string): boolean {
    return this.rolesUsuario.includes(rol);
  }

  tienePermiso(permiso: string): boolean {
    return this.permisosUsuario.includes(permiso);
  }

  cerrarSesion(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  private cargarDatosUsuario(): void {
    const token = this.authService.obtenerToken();
    if (token) {
      try {
        const payload = JSON.parse(this.decodificarPayload(token));

        this.usuarioActual = {
          nombre: payload.name || 'Wilson Denis Arriola',
          ci: payload.sub || payload.nameid || '43791806'
        };
      } catch {
        // Fallback en caso de error
      }
    }
  }

  private decodificarPayload(token: string): string {
    const segmento = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    const padding = segmento.length % 4;
    const base64 = padding ? segmento.padEnd(segmento.length + 4 - padding, '=') : segmento;
    const bytes = Uint8Array.from(atob(base64), (caracter) => caracter.charCodeAt(0));
    return new TextDecoder().decode(bytes);
  }
}