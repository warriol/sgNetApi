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
        const payload = JSON.parse(atob(token.split('.')[1]));
        
        // Extraer Roles del JWT
        const roles = payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        this.rolesUsuario = Array.isArray(roles) ? roles : [roles];

        this.usuarioActual = {
          nombre: payload.name || 'Wilson Denis Arriola',
          ci: payload.sub || payload.nameid || '43791806'
        };
      } catch {
        // Fallback en caso de error
      }
    }
  }
}