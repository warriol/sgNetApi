import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { DependenciaService, Dependencia, Turno, UnidadEjecutora } from '../../../core/services/dependencia.service';

@Component({
  selector: 'app-dependencias-gestion',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dependencias.component.html',
  styleUrls: ['./dependencias.component.scss']
})
export class DependenciasGestionComponent implements OnInit {
  dependencias: Dependencia[] = [];
  turnos: Turno[] = [];
  unidadesEjecutoras: UnidadEjecutora[] = [];
  seleccionada: Dependencia | null = null;
  formulario: any = {
    idUuee: null,
    nombre: '',
    siglas: '',
    idDireccion: null,
    nombreUsuarioJefe: '',
    nombreUsuarioSegundoJefe: '',
    idTurno: null
  };
  cargando = false;
  guardando = false;
  mensaje = '';

  constructor(private dependenciaService: DependenciaService) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando = true;
    forkJoin({
      dependencias: this.dependenciaService.getDependencias(),
      turnos: this.dependenciaService.getTurnos(),
      unidades: this.dependenciaService.getUnidadesEjecutoras()
    }).subscribe({
      next: ({ dependencias, turnos, unidades }) => {
        this.dependencias = dependencias;
        this.turnos = turnos;
        this.unidadesEjecutoras = unidades as UnidadEjecutora[];
        if (this.dependencias.length && !this.seleccionada) {
          this.seleccionar(this.dependencias[0]);
        }
        this.cargando = false;
      },
      error: () => {
        this.mensaje = 'No se pudieron cargar las dependencias.';
        this.cargando = false;
      }
    });
  }

  seleccionar(item: Dependencia): void {
    this.seleccionada = item;
    this.formulario = {
      idUuee: item.idUuee,
      nombre: item.nombre,
      siglas: item.siglas,
      idDireccion: item.idDireccion ?? null,
      nombreUsuarioJefe: item.nombreUsuarioJefe ?? '',
      nombreUsuarioSegundoJefe: item.nombreUsuarioSegundoJefe ?? '',
      idTurno: item.idTurno ?? null
    };
  }

  guardar(): void {
    if (!this.formulario.idUuee || !this.formulario.nombre || !this.formulario.siglas) {
      this.mensaje = 'Debe completar la unidad ejecutora, nombre y siglas.';
      return;
    }

    this.guardando = true;
    const solicitud = this.seleccionada
      ? this.dependenciaService.actualizarDependencia(this.seleccionada.idDependencia, this.formulario)
      : this.dependenciaService.crearDependencia(this.formulario);

    solicitud.subscribe({
      next: () => {
        this.guardando = false;
        this.mensaje = 'Dependencia guardada correctamente.';
        this.cargarDatos();
      },
      error: (err) => {
        this.guardando = false;
        this.mensaje = err?.error?.mensaje ?? 'No se pudo guardar la dependencia.';
      }
    });
  }

  nueva(): void {
    this.seleccionada = null;
    this.formulario = {
      idUuee: null,
      nombre: '',
      siglas: '',
      idDireccion: null,
      nombreUsuarioJefe: '',
      nombreUsuarioSegundoJefe: '',
      idTurno: null
    };
  }

  eliminar(item: Dependencia): void {
    if (!confirm(`¿Desea eliminar la dependencia ${item.nombre}?`)) {
      return;
    }

    this.dependenciaService.eliminarDependencia(item.idDependencia).subscribe({
      next: () => {
        this.mensaje = 'Dependencia eliminada.';
        this.cargarDatos();
      },
      error: (err) => {
        this.mensaje = err?.error?.mensaje ?? 'No se pudo eliminar la dependencia.';
      }
    });
  }
}
