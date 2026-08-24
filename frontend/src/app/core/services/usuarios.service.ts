import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Usuario {
  ci?: number;
  nombreUsuario: string;
  nombre: string;
  apellido: string;
  correo: string;
  celular?: string;
  habilitado: boolean;
  intentosFallidos: number;
  grado?: string;
  escalafon?: string;
  fechaNacimiento?: string;
  idNacionalidad?: number;
  idEstadoCivil?: number;
  idProfesion?: number;
  idGrado?: number;
  idEscalafon?: number;
  idDependencia?: number;
  roles: string[];
}

export interface CrearUsuarioDto {
  ci: number | null;
  nombreUsuario: string;
  nombre: string;
  apellido: string;
  correo: string;
  password?: string;
  celular?: string;
  fechaNacimiento: string;
  idNacionalidad: number | null;
  idEstadoCivil?: number | null;
  idProfesion?: number | null;
  idGrado?: number | null;
  idEscalafon?: number | null;
  idUuee: number | null;
  idDependencia?: number | null;
}

export interface EditarUsuarioDto {
  nombre: string;
  apellido: string;
  correo: string;
  celular?: string;
  fechaNacimiento: string;
  idNacionalidad: number | null;
  idEstadoCivil?: number | null;
  idProfesion?: number | null;
  idGrado?: number | null;
  idEscalafon?: number | null;
  idUuee?: number | null;
  idDependencia?: number | null;
}

export interface CatalogoItem { id: number; nombre: string; }
export interface Nacionalidad extends CatalogoItem { esUruguaya: boolean; idNacionalidad: number; }
export interface UnidadEjecutora extends CatalogoItem { idUuee: number; siglas: string; }
export interface Dependencia extends CatalogoItem { idDependencia: number; idUuee: number; siglas: string; }
export interface Catalogos {
  nacionalidades: Nacionalidad[];
  estadosCiviles: CatalogoItem[];
  profesiones: CatalogoItem[];
  grados: Array<CatalogoItem & { idGrado: number; numero: number; texto: string }>;
  escalafones: Array<CatalogoItem & { idEscalafon: number; abreviatura: string }>;
  unidadesEjecutoras: UnidadEjecutora[];
  dependencias: Dependencia[];
}

export interface CatalogoCrudDto {
  nombre: string;
  codigoIso?: string;
  esUruguaya?: boolean;
  numero?: number;
  texto?: string;
  abreviatura?: string;
  idUuee?: number;
  siglas?: string;
}

export interface Rol {
  idRol: number;
  nombre: string;
  descripcion?: string;
  permisos: Permiso[];
}

export interface RolDto {
  nombre: string;
  descripcion?: string;
  idsPermisos: number[];
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

  obtenerUsuarioPorCi(ci: string): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.apiUrl}/Usuarios/${ci}`);
  }

  cambiarEstadoUsuario(nombreUsuario: string, habilitado: boolean): Observable<any> {
    return this.http.patch(`${this.apiUrl}/Usuarios/${nombreUsuario}/estado`, { habilitado });
  }

  actualizarRolesYPermisos(nombreUsuario: string, dto: AsignarRolesPermisosDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuarios/${nombreUsuario}/roles-permisos`, dto);
  }

  editarUsuario(nombreUsuario: string, dto: EditarUsuarioDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuarios/${nombreUsuario}`, dto);
  }

  obtenerRoles(): Observable<Rol[]> {
    return this.http.get<Rol[]>(`${this.apiUrl}/Roles`);
  }

  crearRol(dto: RolDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Roles`, dto);
  }

  actualizarRol(idRol: number, dto: RolDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Roles/${idRol}`, dto);
  }

  obtenerPermisos(): Observable<Permiso[]> {
    return this.http.get<Permiso[]>(`${this.apiUrl}/Permisos`);
  }

  obtenerCatalogos(): Observable<Catalogos> {
    return this.http.get<Catalogos>(`${this.apiUrl}/Catalogos`);
  }

  crearCatalogo(tipo: string, dto: CatalogoCrudDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Catalogos/${tipo}`, dto);
  }

  actualizarCatalogo(tipo: string, id: number, dto: CatalogoCrudDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Catalogos/${tipo}/${id}`, dto);
  }

  eliminarCatalogo(tipo: string, id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Catalogos/${tipo}/${id}`);
  }
}