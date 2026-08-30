import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DependenciaService, Turno } from '../../../core/services/dependencia.service';

@Component({
  selector: 'app-dependencias-turnos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './turnos.component.html',
  styleUrls: ['./turnos.component.scss']
})
export class DependenciasTurnosComponent implements OnInit {
  turnos: Turno[] = [];
  seleccionada: Turno | null = null;
  readonly tiposTurno = [
    { value: '4x6', label: '4 turnos de 6 hs' },
    { value: '3x8', label: '3 turnos de 8 hs' },
    { value: '2x12', label: '2 turnos de 12 hs' },
    { value: '1x24', label: '1 turno de 24 hs' }
  ];

  formulario: any = {
    nombre: '',
    tipoTurno: '4x6',
    horaInicio: '08:00',
    horaFin: '16:00',
    descripcion: '',
    habilitado: true
  };
  cargando = false;
  guardando = false;
  mensaje = '';

  constructor(private dependenciaService: DependenciaService) {}

  ngOnInit(): void {
    this.cargarTurnos();
  }

  cargarTurnos(): void {
    this.cargando = true;
    this.dependenciaService.getTurnos().subscribe({
      next: (turnos) => {
        this.turnos = turnos;
        this.cargando = false;
      },
      error: () => {
        this.mensaje = 'No se pudieron cargar los turnos.';
        this.cargando = false;
      }
    });
  }

  seleccionar(turno: Turno): void {
    this.seleccionada = turno;
    this.formulario = {
      nombre: turno.nombre,
      tipoTurno: turno.tipoTurno ?? '4x6',
      horaInicio: turno.horaInicio,
      horaFin: turno.horaFin,
      descripcion: turno.descripcion ?? '',
      habilitado: turno.habilitado
    };
  }

  nueva(): void {
    this.seleccionada = null;
    this.formulario = {
      nombre: '',
      tipoTurno: '4x6',
      horaInicio: '08:00',
      horaFin: '16:00',
      descripcion: '',
      habilitado: true
    };
  }

  guardar(): void {
    if (!this.formulario.nombre || !this.formulario.tipoTurno || !this.formulario.horaInicio || !this.formulario.horaFin) {
      this.mensaje = 'Debe completar nombre, tipo y horarios del turno.';
      return;
    }

    this.guardando = true;
    const solicitud = this.seleccionada
      ? this.dependenciaService.actualizarTurno(this.seleccionada.idTurno, this.formulario)
      : this.dependenciaService.crearTurno(this.formulario);

    solicitud.subscribe({
      next: () => {
        this.guardando = false;
        this.mensaje = 'Turno guardado correctamente.';
        this.cargarTurnos();
        this.nueva();
      },
      error: (err) => {
        this.guardando = false;
        this.mensaje = err?.error?.mensaje ?? 'No se pudo guardar el turno.';
      }
    });
  }

  eliminar(turno: Turno): void {
    if (!confirm(`¿Desea deshabilitar o eliminar el turno ${turno.nombre}?`)) {
      return;
    }

    this.dependenciaService.eliminarTurno(turno.idTurno).subscribe({
      next: () => {
        this.mensaje = 'Turno eliminado o deshabilitado.';
        this.cargarTurnos();
      },
      error: (err) => {
        this.mensaje = err?.error?.mensaje ?? 'No se pudo eliminar el turno.';
      }
    });
  }
}
