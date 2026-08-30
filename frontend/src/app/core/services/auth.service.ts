import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  nombreUsuario: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  roles: string[];
  permisos: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5283/api/Auth'; // URL de tu API .NET
  private TOKEN_KEY = 'sgnet_token';

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((res) => {
        if (res.token) {
          localStorage.setItem(this.TOKEN_KEY, res.token);
        }
      })
    );
  }

  obtenerToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  estaAutenticado(): boolean {
    return !!this.obtenerToken();
  }

  // Extrae la lista de permisos guardados dentro del token JWT decodificado
  obtenerPermisosDelToken(): string[] {
    const token = this.obtenerToken();
    if (!token) return [];

    try {
      const payloadJson = this.decodificarPayload(token);
      const payload = JSON.parse(payloadJson);

      const permisos = payload.permiso ?? payload['permiso[]'];
      if (!permisos) return [];

      return Array.isArray(permisos) ? permisos : [permisos];
    } catch {
      return [];
    }
  }

  obtenerRolesDelToken(): string[] {
    const token = this.obtenerToken();
    if (!token) return [];

    try {
      const payload = JSON.parse(this.decodificarPayload(token));
      const roles = payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (!roles) return [];
      return Array.isArray(roles) ? roles : [roles];
    } catch {
      return [];
    }
  }

  private decodificarPayload(token: string): string {
    const segmento = token.split('.')[1]
      .replace(/-/g, '+')
      .replace(/_/g, '/');
    const padding = segmento.length % 4;
    const base64 = padding ? segmento.padEnd(segmento.length + 4 - padding, '=') : segmento;
    const bytes = Uint8Array.from(atob(base64), (caracter) => caracter.charCodeAt(0));
    return new TextDecoder().decode(bytes);
  }
}