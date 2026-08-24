import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuariosService, Usuario, Rol, Permiso } from '../../../core/services/usuarios.service';
import { CrearUsuarioDto } from '../../../core/services/usuarios.service';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.scss']
})
export class UsuariosComponent implements OnInit {
  listaUsuarios: Usuario[] = [];
  usuariosFiltrados: Usuario[] = [];
  filtroTexto: string = '';
  cargando: boolean = false;

  // Catálogos para la Modal
  catalogoRoles: Rol[] = [];
  catalogoPermisos: Permiso[] = [];

  // Estado del Modal
  mostrarModalRoles: boolean = false;
  usuarioSeleccionado: Usuario | null = null;
  idsRolesSeleccionados: number[] = [];
  idsPermisosDirectos: number[] = [];
  guardandoModal: boolean = false;
  mostrarModalCrear: boolean = false;
  guardandoCrear: boolean = false;
  mensajeErrorCrear: string | null = null;

  nuevoUsuario: CrearUsuarioDto = {
    ci: 0,
    nombreUsuario: '',
    nombre: '',
    apellido: '',
    correo: '',
    password: '',
    grado: '',
    escalafon: ''
  };

  abrirModalCrear(): void {
    this.mensajeErrorCrear = null;
    this.nuevoUsuario = {
      ci: null as any,
      nombreUsuario: '',
      nombre: '',
      apellido: '',
      correo: '',
      password: '',
      grado: '',
      escalafon: ''
    };
    this.mostrarModalCrear = true;
  }

  cerrarModalCrear(): void {
    this.mostrarModalCrear = false;
  }

  guardarNuevoUsuario(): void {
    if (!this.nuevoUsuario.ci || !this.nuevoUsuario.nombre || !this.nuevoUsuario.correo) {
      this.mensajeErrorCrear = 'Por favor complete los campos obligatorios (CI, Nombre, Correo).';
      return;
    }

    this.guardandoCrear = true;
    this.mensajeErrorCrear = null;

    this.usuariosService.crearUsuario(this.nuevoUsuario).subscribe({
      next: () => {
        this.guardandoCrear = false;
        this.cerrarModalCrear();
        this.cargarUsuarios(); // Recarga la tabla de usuarios
      },
      error: (err) => {
        this.guardandoCrear = false;
        this.mensajeErrorCrear = err.error?.mensaje || 'Error al crear el usuario en el servidor.';
      }
    });
  }

  constructor(private usuariosService: UsuariosService) {}

  ngOnInit(): void {
    this.cargarUsuarios();
    this.cargarCatalogos();
  }

  cargarUsuarios(): void {
    this.cargando = true;
    this.usuariosService.obtenerUsuarios().subscribe({
      next: (data) => {
        this.listaUsuarios = data;
        this.aplicarFiltro();
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error al cargar la lista de usuarios:', err);
        this.cargando = false;
      }
    });
  }

  cargarCatalogos(): void {
    this.usuariosService.obtenerRoles().subscribe({
      next: (roles) => (this.catalogoRoles = roles),
      error: (err) => console.error('Error al cargar roles:', err)
    });

    this.usuariosService.obtenerPermisos().subscribe({
      next: (permisos) => (this.catalogoPermisos = permisos),
      error: (err) => console.error('Error al cargar permisos:', err)
    });
  }

  aplicarFiltro(): void {
    if (!this.filtroTexto.trim()) {
      this.usuariosFiltrados = [...this.listaUsuarios];
      return;
    }

    const txt = this.filtroTexto.toLowerCase();
    this.usuariosFiltrados = this.listaUsuarios.filter(
      (u) =>
        u.ci.toString().includes(txt) ||
        u.nombre.toLowerCase().includes(txt) ||
        u.apellido.toLowerCase().includes(txt) ||
        u.correo.toLowerCase().includes(txt)
    );
  }

  toggleEstado(usuario: Usuario): void {
    const nuevoEstado = !usuario.habilitado;
    this.usuariosService.cambiarEstadoUsuario(usuario.ci, nuevoEstado).subscribe({
      next: () => {
        usuario.habilitado = nuevoEstado;
        if (nuevoEstado) usuario.intentosFallidos = 0;
      },
      error: (err) => console.error('Error al cambiar estado del usuario:', err)
    });
  }

  abrirModalEditarRoles(usuario: Usuario): void {
    this.usuarioSeleccionado = usuario;
    
    // Marcar los roles actuales del usuario según el catálogo
    this.idsRolesSeleccionados = this.catalogoRoles
      .filter((r) => usuario.roles.includes(r.nombre))
      .map((r) => r.idRol);

    this.idsPermisosDirectos = [];
    this.mostrarModalRoles = true;
  }

  cerrarModal(): void {
    this.mostrarModalRoles = false;
    this.usuarioSeleccionado = null;
  }

  toggleRol(idRol: number): void {
    const idx = this.idsRolesSeleccionados.indexOf(idRol);
    if (idx > -1) {
      this.idsRolesSeleccionados.splice(idx, 1);
    } else {
      this.idsRolesSeleccionados.push(idRol);
    }
  }

  togglePermisoDirecto(idPermiso: number): void {
    const idx = this.idsPermisosDirectos.indexOf(idPermiso);
    if (idx > -1) {
      this.idsPermisosDirectos.splice(idx, 1);
    } else {
      this.idsPermisosDirectos.push(idPermiso);
    }
  }

  guardarRolesPermisos(): void {
    if (!this.usuarioSeleccionado) return;

    this.guardandoModal = true;
    const dto = {
      idsRoles: this.idsRolesSeleccionados,
      idsPermisosDirectos: this.idsPermisosDirectos
    };

    this.usuariosService
      .actualizarRolesYPermisos(this.usuarioSeleccionado.ci, dto)
      .subscribe({
        next: () => {
          this.guardandoModal = false;
          this.cerrarModal();
          this.cargarUsuarios(); // Recargar la lista para refrescar los badges
        },
        error: (err) => {
          console.error('Error al guardar roles y permisos:', err);
          this.guardandoModal = false;
        }
      });
  }
}