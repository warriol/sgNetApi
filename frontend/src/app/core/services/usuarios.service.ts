import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Usuario {
  ci: number;
  nombreUsuario: string;
  nombre: string;
  apellido: string;
  correo: string;
  celular?: number;
  habilitado: boolean;
  intentosFallidos: number;
  grado?: string;
  escalafon?: string;
  roles: string[];
}

export interface CrearUsuarioDto {
  ci: number;
  nombreUsuario: string;
  nombre: string;
  apellido: string;
  correo: string;
  password?: string;
  celular?: number;
  grado?: string;
  escalafon?: string;
}

export interface Rol {
  idRol: number;
  nombre: string;
  permisos: Permiso[];
}

export interface Permiso {
  idPermiso: number;
  nombre: string;
  descripcion: string;
}

export interface AsignarRolesPermisosDto {
  idsRoles: number[];
  idsPermisosDirectos: number[];
}

@Injectable({
  providedIn: 'root'
})
export class UsuariosService {
  private apiUrl = 'http://localhost:5283/api';

  constructor(private http: HttpClient) {}

  obtenerUsuarios(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.apiUrl}/Usuarios`);
  }

  crearUsuario(dto: CrearUsuarioDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/Usuarios`, dto);
  }

  obtenerUsuarioPorCi(ci: number): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.apiUrl}/Usuarios/${ci}`);
  }

  cambiarEstadoUsuario(ci: number, habilitado: boolean): Observable<any> {
    return this.http.patch(`${this.apiUrl}/Usuarios/${ci}/estado`, { habilitado });
  }

  actualizarRolesYPermisos(ci: number, dto: AsignarRolesPermisosDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuarios/${ci}/roles-permisos`, dto);
  }

  obtenerRoles(): Observable<Rol[]> {
    return this.http.get<Rol[]>(`${this.apiUrl}/Roles`);
  }

  obtenerPermisos(): Observable<Permiso[]> {
    return this.http.get<Permiso[]>(`${this.apiUrl}/Permisos`);
  }
}