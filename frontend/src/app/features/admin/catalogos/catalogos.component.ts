import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CatalogoCrudDto, Catalogos, UsuariosService } from '../../../core/services/usuarios.service';

type CatalogoClave = 'nacionalidades' | 'estadosCiviles' | 'profesiones' | 'grados' | 'escalafones' | 'unidadesEjecutoras' | 'dependencias';

@Component({
  selector: 'app-catalogos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalogos.component.html',
  styleUrls: ['./catalogos.component.scss']
})
export class CatalogosComponent implements OnInit {
  catalogos: Catalogos | null = null;
  catalogoActivo: CatalogoClave = 'nacionalidades';
  mostrarModal = false;
  modoEdicion = false;
  idEdicion: number | null = null;
  cargando = false;
  guardando = false;
  mensajeError: string | null = null;
  formulario: CatalogoCrudDto = { nombre: '' };

  readonly opciones: Array<{ clave: CatalogoClave; titulo: string }> = [
    { clave: 'nacionalidades', titulo: 'Nacionalidades' },
    { clave: 'estadosCiviles', titulo: 'Estados civiles' },
    { clave: 'profesiones', titulo: 'Profesiones' },
    { clave: 'grados', titulo: 'Grados' },
    { clave: 'escalafones', titulo: 'Escalafones' },
    { clave: 'unidadesEjecutoras', titulo: 'Unidades ejecutoras' },
    { clave: 'dependencias', titulo: 'Dependencias' }
  ];

  constructor(private usuariosService: UsuariosService) {}

  ngOnInit(): void { this.cargarCatalogos(); }

  cargarCatalogos(): void {
    this.cargando = true;
    this.usuariosService.obtenerCatalogos().subscribe({
      next: (catalogos) => { this.catalogos = catalogos; this.cargando = false; },
      error: () => { this.mensajeError = 'No se pudieron cargar los catálogos.'; this.cargando = false; }
    });
  }

  get registros(): any[] { return this.catalogos?.[this.catalogoActivo] ?? []; }
  get tituloCatalogo(): string { return this.opciones.find((o) => o.clave === this.catalogoActivo)?.titulo ?? ''; }

  seleccionarCatalogo(clave: CatalogoClave): void { this.catalogoActivo = clave; }

  abrirCrear(): void {
    this.modoEdicion = false;
    this.idEdicion = null;
    this.mensajeError = null;
    this.formulario = { nombre: '' };
    this.mostrarModal = true;
  }

  abrirEditar(registro: any): void {
    this.modoEdicion = true;
    this.idEdicion = this.identificador(registro);
    this.mensajeError = null;
    this.formulario = {
      nombre: registro.nombre ?? registro.texto ?? '',
      codigoIso: registro.codigoIso,
      esUruguaya: registro.esUruguaya,
      numero: registro.numero,
      texto: registro.texto,
      abreviatura: registro.abreviatura,
      idUuee: registro.idUuee,
      siglas: registro.siglas
    };
    this.mostrarModal = true;
  }

  cerrarModal(): void { this.mostrarModal = false; }

  guardar(): void {
    if (!this.formulario.nombre && this.catalogoActivo !== 'grados') {
      this.mensajeError = 'El nombre es obligatorio.';
      return;
    }
    if (this.catalogoActivo === 'grados' && (!this.formulario.texto || !this.formulario.abreviatura)) {
      this.mensajeError = 'Texto y abreviatura son obligatorios.';
      return;
    }
    if (this.catalogoActivo === 'dependencias' && !this.formulario.idUuee) {
      this.mensajeError = 'Seleccione una unidad ejecutora.';
      return;
    }
    if ((this.catalogoActivo === 'dependencias' || this.catalogoActivo === 'unidadesEjecutoras') && !this.formulario.siglas?.trim()) {
      this.mensajeError = 'Las siglas son obligatorias.';
      return;
    }

    this.guardando = true;
    const operacion = this.modoEdicion && this.idEdicion !== null
      ? this.usuariosService.actualizarCatalogo(this.catalogoActivo, this.idEdicion, this.formulario)
      : this.usuariosService.crearCatalogo(this.catalogoActivo, this.formulario);
    operacion.subscribe({
      next: () => { this.guardando = false; this.cerrarModal(); this.cargarCatalogos(); },
      error: (err) => { this.guardando = false; this.mensajeError = err.error?.mensaje ?? 'No se pudo guardar el catálogo.'; }
    });
  }

  eliminar(registro: any): void {
    const id = this.identificador(registro);
    if (!id || !window.confirm('¿Desea eliminar este registro?')) return;
    this.usuariosService.eliminarCatalogo(this.catalogoActivo, id).subscribe({
      next: () => this.cargarCatalogos(),
      error: (err) => this.mensajeError = err.error?.mensaje ?? 'No se pudo eliminar el catálogo.'
    });
  }

  identificador(registro: any): number {
    return registro.idNacionalidad ?? registro.idEstadoCivil ?? registro.idProfesion ?? registro.idGrado ??
      registro.idEscalafon ?? registro.idUuee ?? registro.idDependencia;
  }
}
