import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditoriaLog {
  id: number;
  fecha: string;
  usuarioCi?: string;
  ipOrigen: string;
  metodoHttp: string;
  ruta: string;
  codigoEstado: number;
  duracionMs: number;
  errorExcepcion?: string;
}

export interface RespuestaPaginada<T> {
  elementos: T[];
  paginaActual: number;
  totalPaginas: number;
  totalRegistros: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuditoriaService {
  private apiUrl = 'http://localhost:5283/api/Auditoria';

  constructor(private http: HttpClient) {}

  obtenerLogsPaginados(
    pagina: number = 1,
    registrosPorPagina: number = 10,
    usuarioCi?: string,
    codigoEstado?: number
  ): Observable<RespuestaPaginada<AuditoriaLog>> {
    let params = new HttpParams()
      .set('pagina', pagina)
      .set('registrosPorPagina', registrosPorPagina);

    if (usuarioCi) params = params.set('usuarioCi', usuarioCi);
    if (codigoEstado) params = params.set('codigoEstado', codigoEstado);

    return this.http.get<RespuestaPaginada<AuditoriaLog>>(this.apiUrl, { params });
  }

  exportarCsv(usuarioCi?: string, codigoEstado?: number): Observable<Blob> {
    let params = new HttpParams();
    if (usuarioCi) params = params.set('usuarioCi', usuarioCi);
    if (codigoEstado) params = params.set('codigoEstado', codigoEstado);

    return this.http.get(`${this.apiUrl}/exportar`, {
      params,
      responseType: 'blob'
    });
  }
}