import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuditoriaService, AuditoriaLog } from '../../../core/services/auditoria.service';

@Component({
  selector: 'app-auditoria',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auditoria.component.html',
  styleUrls: ['./auditoria.component.scss']
})
export class AuditoriaComponent implements OnInit {
  logs: AuditoriaLog[] = [];
  cargando: boolean = false;
  exportando: boolean = false;

  // Filtros de búsqueda
  filtroUsuarioCi: string = '';
  filtroCodigoEstado?: number;

  // Paginación
  paginaActual: number = 1;
  registrosPorPagina: number = 10;
  totalPaginas: number = 1;
  totalRegistros: number = 0;

  constructor(private auditoriaService: AuditoriaService) {}

  ngOnInit(): void {
    this.cargarLogs();
  }

  cargarLogs(): void {
    this.cargando = true;
    this.auditoriaService
      .obtenerLogsPaginados(
        this.paginaActual,
        this.registrosPorPagina,
        this.filtroUsuarioCi,
        this.filtroCodigoEstado
      )
      .subscribe({
        next: (res) => {
          this.logs = res.elementos;
          this.paginaActual = res.paginaActual;
          this.totalPaginas = res.totalPaginas;
          this.totalRegistros = res.totalRegistros;
          this.cargando = false;
        },
        error: (err) => {
          console.error('Error al consultar logs de auditoría:', err);
          this.cargando = false;
        }
      });
  }

  buscar(): void {
    this.paginaActual = 1;
    this.cargarLogs();
  }

  limpiarFiltros(): void {
    this.filtroUsuarioCi = '';
    this.filtroCodigoEstado = undefined;
    this.paginaActual = 1;
    this.cargarLogs();
  }

  cambiarPagina(nuevaPagina: number): void {
    if (nuevaPagina >= 1 && nuevaPagina <= this.totalPaginas) {
      this.paginaActual = nuevaPagina;
      this.cargarLogs();
    }
  }

  exportarReporteCsv(): void {
    this.exportando = true;
    this.auditoriaService
      .exportarCsv(this.filtroUsuarioCi, this.filtroCodigoEstado)
      .subscribe({
        next: (blob) => {
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `auditoria_logs_${new Date().toISOString().slice(0, 10)}.csv`;
          a.click();
          window.URL.revokeObjectURL(url);
          this.exportando = false;
        },
        error: (err) => {
          console.error('Error al descargar reporte CSV:', err);
          this.exportando = false;
        }
      });
  }
}