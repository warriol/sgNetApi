import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface UnidadEjecutora {
  idUuee: number;
  nombre: string;
  siglas: string;
}

export interface Dependencia {
  idDependencia: number;
  idUuee: number;
  nombreUuee?: string;
  nombre: string;
  siglas: string;
  idDireccion?: number | null;
  nombreUsuarioJefe?: string | null;
  nombreUsuarioSegundoJefe?: string | null;
  idTurno?: number | null;
  nombreTurno?: string | null;
  funcionariosAsignados?: number;
  completada?: boolean;
}

export interface Turno {
  idTurno: number;
  nombre: string;
  tipoTurno: '4x6' | '3x8' | '2x12' | '1x24';
  horaInicio: string;
  horaFin: string;
  descripcion?: string;
  habilitado: boolean;
}

export interface Direccion {
  idDireccion: number;
  tipoDireccion: number;
  pais: string;
  departamento: string;
  localidad: string;
  calle?: string | null;
  cruce1?: string | null;
  cruce2?: string | null;
  numero?: string | null;
  ruta?: string | null;
  km?: number | null;
  latitud?: number | null;
  longitud?: number | null;
  codigoPostal?: string | null;
  telefono?: string | null;
}

@Injectable({ providedIn: 'root' })
export class DependenciaService {
  private apiUrl = 'http://localhost:5283/api';

  constructor(private http: HttpClient) {}

  getUnidadesEjecutoras(): Observable<UnidadEjecutora[]> {
    return this.http.get<any>(`${this.apiUrl}/Catalogos`).pipe(
      map((response) => response?.unidadesEjecutoras ?? [])
    );
  }

  getDependencias(): Observable<Dependencia[]> {
    return this.http.get<Dependencia[]>(`${this.apiUrl}/Dependencias`);
  }

  getDependencia(id: number): Observable<Dependencia> {
    return this.http.get<Dependencia>(`${this.apiUrl}/Dependencias/${id}`);
  }

  crearDependencia(dto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Dependencias`, dto);
  }

  actualizarDependencia(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Dependencias/${id}`, dto);
  }

  eliminarDependencia(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Dependencias/${id}`);
  }

  getTurnos(): Observable<Turno[]> {
    return this.http.get<Turno[]>(`${this.apiUrl}/Turnos`);
  }

  asignarTurnoUsuario(nombreUsuario: string, idTurnoAsignado: number | null): Observable<any> {
    return this.http.patch(`${this.apiUrl}/Usuarios/${nombreUsuario}/turno`, { idTurnoAsignado });
  }

  crearTurno(dto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Turnos`, dto);
  }

  actualizarTurno(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Turnos/${id}`, dto);
  }

  eliminarTurno(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Turnos/${id}`);
  }

  getDirecciones(): Observable<Direccion[]> {
    return this.http.get<Direccion[]>(`${this.apiUrl}/Direcciones`);
  }

  crearDireccion(dto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Direcciones`, dto);
  }

  actualizarDireccion(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Direcciones/${id}`, dto);
  }

  eliminarDireccion(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Direcciones/${id}`);
  }

  getUsuarios(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Usuarios`);
  }

  actualizarFuncionarios(idDependencia: number, dto: { nombresUsuario: string[] }): Observable<any> {
    return this.http.put(`${this.apiUrl}/Dependencias/${idDependencia}/funcionarios`, dto);
  }

  actualizarJefaturas(idDependencia: number, dto: { nombreUsuarioJefe: string; nombreUsuarioSegundoJefe: string }): Observable<any> {
    return this.http.put(`${this.apiUrl}/Dependencias/${idDependencia}/jefaturas`, dto);
  }

  asignarTurno(idDependencia: number, dto: { idTurno: number }): Observable<any> {
    return this.http.put(`${this.apiUrl}/Dependencias/${idDependencia}/turno`, dto);
  }

  getCompletitud(idDependencia: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/Dependencias/${idDependencia}/completitud`);
  }

  getEquiposAsignados(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/FuncionarioEquipoPolicial`);
  }

  crearAsignacionEquipo(dto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/FuncionarioEquipoPolicial`, dto);
  }

  actualizarAsignacionEquipo(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/FuncionarioEquipoPolicial/${id}`, dto);
  }

  registrarDevolucion(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/FuncionarioEquipoPolicial/${id}/devolucion`, dto);
  }
}
