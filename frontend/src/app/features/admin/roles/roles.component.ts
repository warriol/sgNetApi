import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuariosService, Rol, Permiso } from '../../../core/services/usuarios.service';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  listaRoles: Rol[] = [];
  catalogoPermisos: Permiso[] = [];
  cargando: boolean = false;

  // Estado del Modal (Crear / Editar)
  mostrarModal: boolean = false;
  modoEdicion: boolean = false;
  idRolEdicion: number | null = null;
  nombreRol: string = '';
  descripcionRol: string = '';
  idsPermisosSeleccionados: number[] = [];
  guardando: boolean = false;
  mensajeError: string | null = null;

  constructor(
    private usuariosService: UsuariosService
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando = true;

    // Cargar catálogo de permisos
    this.usuariosService.obtenerPermisos().subscribe({
      next: (permisos) => {
        this.catalogoPermisos = permisos;
        this.cargarRoles();
      },
      error: (err) => {
        console.error('Error al cargar permisos:', err);
        this.cargando = false;
      }
    });
  }

  cargarRoles(): void {
    this.usuariosService.obtenerRoles().subscribe({
      next: (roles) => {
        this.listaRoles = roles;
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error al cargar roles:', err);
        this.cargando = false;
      }
    });
  }

  abrirModalCrear(): void {
    this.modoEdicion = false;
    this.idRolEdicion = null;
    this.nombreRol = '';
    this.descripcionRol = '';
    this.idsPermisosSeleccionados = [];
    this.mensajeError = null;
    this.mostrarModal = true;
  }

  abrirModalEditar(rol: Rol): void {
    this.modoEdicion = true;
    this.idRolEdicion = rol.idRol;
    this.nombreRol = rol.nombre;
    this.descripcionRol = rol.descripcion ?? '';
    this.idsPermisosSeleccionados = rol.permisos.map((p) => p.idPermiso);
    this.mensajeError = null;
    this.mostrarModal = true;
  }

  cerrarModal(): void {
    this.mostrarModal = false;
    this.nombreRol = '';
    this.descripcionRol = '';
    this.idsPermisosSeleccionados = [];
  }

  togglePermiso(idPermiso: number): void {
    const idx = this.idsPermisosSeleccionados.indexOf(idPermiso);
    if (idx > -1) {
      this.idsPermisosSeleccionados.splice(idx, 1);
    } else {
      this.idsPermisosSeleccionados.push(idPermiso);
    }
  }

  guardarRol(): void {
    if (!this.nombreRol.trim()) {
      this.mensajeError = 'Debe ingresar un nombre para el Rol.';
      return;
    }

    this.guardando = true;
    this.mensajeError = null;

    const body = {
      nombre: this.nombreRol.trim(),
      descripcion: this.descripcionRol.trim() || undefined,
      idsPermisos: this.idsPermisosSeleccionados
    };

    if (this.modoEdicion && this.idRolEdicion) {
      // PUT /api/Roles/{idRol}
      this.usuariosService.actualizarRol(this.idRolEdicion, body).subscribe({
        next: () => {
          this.guardando = false;
          this.cerrarModal();
          this.cargarRoles();
        },
        error: (err) => {
          this.guardando = false;
          this.mensajeError = err.error?.mensaje || 'Error al actualizar el Rol.';
        }
      });
    } else {
      // POST /api/Roles
      this.usuariosService.crearRol(body).subscribe({
        next: () => {
          this.guardando = false;
          this.cerrarModal();
          this.cargarRoles();
        },
        error: (err) => {
          this.guardando = false;
          this.mensajeError = err.error?.mensaje || 'Error al crear el Rol.';
        }
      });
    }
  }
}