import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DependenciaService, Dependencia, Turno } from '../../../core/services/dependencia.service';

@Component({
  selector: 'app-dependencias-escalafon',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './escalafon.component.html',
  styleUrls: ['./escalafon.component.scss']
})
export class DependenciasEscalafonComponent implements OnInit {
  dependencias: Dependencia[] = [];
  usuarios: any[] = [];
  turnos: Turno[] = [];
  seleccionada: Dependencia | null = null;
  funcionarioSeleccionado: any = null;
  turnoSeleccionado: number | null = null;
  formularioEquipo: any = {
    idArmaPolicial: 1,
    idChalecoAntibalaPolicial: 1,
    idEsposasPolicial: 1,
    observaciones: ''
  };
  inventario = {
    armas: [
      { id: 1, nombre: 'Pistola 9mm' },
      { id: 2, nombre: 'Rifle corto' },
      { id: 3, nombre: 'Escopeta' }
    ],
    chalecos: [
      { id: 1, nombre: 'Chaleco nivel II' },
      { id: 2, nombre: 'Chaleco nivel III' }
    ],
    esposas: [
      { id: 1, nombre: 'Juego de esposas estándar' },
      { id: 2, nombre: 'Juego de esposas reforzadas' }
    ]
  };
  mensaje = '';

  constructor(private dependenciaService: DependenciaService) {}

  ngOnInit(): void {
    this.cargarDependencias();
  }

  cargarDependencias(): void {
    this.dependenciaService.getDependencias().subscribe({
      next: (dependencias) => {
        this.dependencias = dependencias;
        if (this.dependencias.length) {
          this.seleccionarDependencia(this.dependencias[0]);
        }
      }
    });

    this.dependenciaService.getTurnos().subscribe({
      next: (turnos) => {
        this.turnos = turnos;
      }
    });

    this.dependenciaService.getUsuarios().subscribe({
      next: (usuarios) => {
        this.usuarios = usuarios;
      }
    });
  }

  turnosDisponibles(): Turno[] {
    return this.turnos.filter((turno) => turno.habilitado);
  }

  seleccionarDependencia(dependencia: Dependencia): void {
    this.seleccionada = dependencia;
    this.funcionarioSeleccionado = null;
    this.mensaje = '';
  }

  usuariosDeDependencia(): any[] {
    return this.usuarios.filter((usuario) => usuario.idDependencia === this.seleccionada?.idDependencia);
  }

  abrirRevista(usuario: any): void {
    this.funcionarioSeleccionado = usuario;
    this.turnoSeleccionado = usuario?.idTurnoAsignado ?? null;
    this.formularioEquipo = {
      idArmaPolicial: 1,
      idChalecoAntibalaPolicial: 1,
      idEsposasPolicial: 1,
      observaciones: ''
    };
  }

  guardarTurnoFuncionario(): void {
    if (!this.funcionarioSeleccionado) {
      this.mensaje = 'Seleccione un funcionario para asignarle su turno.';
      return;
    }

    const turno = this.turnos.find((item) => item.idTurno === this.turnoSeleccionado);
    this.dependenciaService.asignarTurnoUsuario(this.funcionarioSeleccionado.nombreUsuario, this.turnoSeleccionado ?? null).subscribe({
      next: () => {
        this.mensaje = turno ? `Turno asignado: ${turno.nombre}` : 'Turno actualizado correctamente.';
        this.cargarDependencias();
      },
      error: (err) => {
        this.mensaje = err?.error?.mensaje ?? 'No se pudo asignar el turno.';
      }
    });
  }

  guardarRevista(): void {
    if (!this.funcionarioSeleccionado) {
      this.mensaje = 'Seleccione un funcionario para revista.';
      return;
    }

    const payload = {
      nombreUsuario: this.funcionarioSeleccionado.nombreUsuario,
      idArmaPolicial: this.formularioEquipo.idArmaPolicial,
      idChalecoAntibalaPolicial: this.formularioEquipo.idChalecoAntibalaPolicial,
      idEsposasPolicial: this.formularioEquipo.idEsposasPolicial,
      observaciones: this.formularioEquipo.observaciones
    };

    this.dependenciaService.crearAsignacionEquipo(payload).subscribe({
      next: () => {
        this.mensaje = 'Revista registrada correctamente.';
        this.funcionarioSeleccionado = null;
      },
      error: (err) => {
        this.mensaje = err?.error?.mensaje ?? 'No se pudo registrar la revista.';
      }
    });
  }
}
