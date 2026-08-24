import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuariosService, Usuario, Rol, Permiso, Catalogos, Dependencia, CrearUsuarioDto, EditarUsuarioDto } from '../../../core/services/usuarios.service';

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
  catalogos!: Catalogos;
  dependenciasFiltradas: Dependencia[] = [];

  // Estado del Modal
  mostrarModalRoles: boolean = false;
  usuarioSeleccionado: Usuario | null = null;
  idsRolesSeleccionados: number[] = [];
  idsPermisosDirectos: number[] = [];
  guardandoModal: boolean = false;
  mostrarModalCrear: boolean = false;
  mostrarModalEditar: boolean = false;
  guardandoCrear: boolean = false;
  guardandoEditar: boolean = false;
  mensajeErrorCrear: string | null = null;
  mensajeErrorEditar: string | null = null;
  usuarioEditado!: EditarUsuarioDto;
  dependenciasEdicion: Dependencia[] = [];

  nuevoUsuario: CrearUsuarioDto = {
    ci: null,
    nombreUsuario: '',
    nombre: '',
    apellido: '',
    correo: '',
    password: '',
    fechaNacimiento: '',
    idNacionalidad: null,
    idEstadoCivil: null,
    idProfesion: null,
    idGrado: null,
    idEscalafon: null,
    idUuee: null,
    idDependencia: null
  };

  abrirModalCrear(): void {
    this.mensajeErrorCrear = null;
    this.nuevoUsuario = {
      ci: null,
      nombreUsuario: '',
      nombre: '',
      apellido: '',
      correo: '',
      password: '',
      fechaNacimiento: '',
      idNacionalidad: null,
      idEstadoCivil: null,
      idProfesion: null,
      idGrado: null,
      idEscalafon: null,
      idUuee: null,
      idDependencia: null
    };
    this.mostrarModalCrear = true;
  }

  cerrarModalCrear(): void {
    this.mostrarModalCrear = false;
  }

  abrirModalEditar(usuario: Usuario): void {
    this.mensajeErrorEditar = null;
    this.usuarioSeleccionado = usuario;
    this.usuarioEditado = {
      nombre: usuario.nombre,
      apellido: usuario.apellido,
      correo: usuario.correo,
      celular: usuario.celular,
      fechaNacimiento: usuario.fechaNacimiento ?? '',
      idNacionalidad: usuario.idNacionalidad ?? null,
      idEstadoCivil: usuario.idEstadoCivil ?? null,
      idProfesion: usuario.idProfesion ?? null,
      idGrado: usuario.idGrado ?? null,
      idEscalafon: usuario.idEscalafon ?? null,
      idUuee: this.catalogos?.dependencias.find((d) => d.idDependencia === usuario.idDependencia)?.idUuee ?? null,
      idDependencia: usuario.idDependencia ?? null
    };
    this.actualizarDependenciasEdicion();
    this.mostrarModalEditar = true;
  }

  cerrarModalEditar(): void {
    this.mostrarModalEditar = false;
    this.usuarioSeleccionado = null;
  }

  actualizarDependenciasEdicion(): void {
    this.dependenciasEdicion = this.usuarioEditado?.idUuee && this.catalogos
      ? this.catalogos.dependencias.filter((d) => d.idUuee === this.usuarioEditado.idUuee)
      : [];
    if (!this.dependenciasEdicion.some((d) => d.idDependencia === this.usuarioEditado?.idDependencia))
      this.usuarioEditado.idDependencia = null;
  }

  guardarEdicion(): void {
    if (!this.usuarioSeleccionado || !this.usuarioEditado.nombre || !this.usuarioEditado.apellido || !this.usuarioEditado.correo ||
      !this.usuarioEditado.fechaNacimiento || !this.usuarioEditado.idNacionalidad) {
      this.mensajeErrorEditar = 'Complete los campos obligatorios del usuario.';
      return;
    }

    this.guardandoEditar = true;
    this.usuariosService.editarUsuario(this.usuarioSeleccionado.nombreUsuario, this.usuarioEditado).subscribe({
      next: () => {
        this.guardandoEditar = false;
        this.cerrarModalEditar();
        this.cargarUsuarios();
      },
      error: (err) => {
        this.guardandoEditar = false;
        this.mensajeErrorEditar = err.error?.mensaje || 'Error al actualizar el usuario.';
      }
    });
  }

  guardarNuevoUsuario(): void {
    if (!this.nuevoUsuario.nombre || !this.nuevoUsuario.apellido || !this.nuevoUsuario.correo ||
      !this.nuevoUsuario.fechaNacimiento || !this.nuevoUsuario.idNacionalidad) {
      this.mensajeErrorCrear = 'Complete los campos obligatorios del usuario.';
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

    this.usuariosService.obtenerCatalogos().subscribe({
      next: (catalogos) => {
        this.catalogos = catalogos;
        this.actualizarDependencias();
      },
      error: (err) => console.error('Error al cargar catálogos:', err)
    });
  }

  actualizarDependencias(): void {
    const idUuee = this.nuevoUsuario.idUuee;
    this.dependenciasFiltradas = idUuee && this.catalogos
      ? this.catalogos.dependencias.filter((d) => d.idUuee === idUuee)
      : [];
    if (!this.dependenciasFiltradas.some((d) => d.idDependencia === this.nuevoUsuario.idDependencia))
      this.nuevoUsuario.idDependencia = null;
  }

  actualizarIdentidad(): void {
    const nacionalidad = this.catalogos?.nacionalidades.find((n) => n.idNacionalidad === this.nuevoUsuario.idNacionalidad);
    if (nacionalidad?.esUruguaya) {
      this.nuevoUsuario.nombreUsuario = this.nuevoUsuario.ci?.toString() ?? '';
    } else if (nacionalidad) {
      this.nuevoUsuario.ci = null;
      this.nuevoUsuario.nombreUsuario = this.nuevoUsuario.nombreUsuario.replace(/[^a-zA-Z0-9]/g, '');
    }
  }

  get nacionalidadUruguayaSeleccionada(): boolean {
    return this.catalogos?.nacionalidades.some((n) => n.idNacionalidad === this.nuevoUsuario.idNacionalidad && n.esUruguaya) ?? false;
  }

  aplicarFiltro(): void {
    if (!this.filtroTexto.trim()) {
      this.usuariosFiltrados = [...this.listaUsuarios];
      return;
    }

    const txt = this.filtroTexto.toLowerCase();
    this.usuariosFiltrados = this.listaUsuarios.filter(
      (u) =>
        (u.ci?.toString() ?? '').includes(txt) ||
        u.nombreUsuario.toLowerCase().includes(txt) ||
        u.nombre.toLowerCase().includes(txt) ||
        u.apellido.toLowerCase().includes(txt) ||
        u.correo.toLowerCase().includes(txt)
    );
  }

  toggleEstado(usuario: Usuario): void {
    const nuevoEstado = !usuario.habilitado;
    this.usuariosService.cambiarEstadoUsuario(usuario.nombreUsuario, nuevoEstado).subscribe({
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
      .actualizarRolesYPermisos(this.usuarioSeleccionado.nombreUsuario, dto)
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