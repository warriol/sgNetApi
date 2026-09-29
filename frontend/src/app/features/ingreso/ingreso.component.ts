import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FichaEvaluada, FichaNombre, IngresoService } from '../../core/services/ingreso.service';

interface RegistroIngreso { [key: string]: string | number | boolean | null; }

@Component({
  selector: 'app-ingreso', standalone: true, imports: [CommonModule, FormsModule],
  templateUrl: './ingreso.component.html', styleUrls: ['./ingreso.component.scss']
})
export class IngresoComponent {
  readonly pasos = ['General', 'Ubicación', 'Intervinientes', 'Fichas', 'Narración', 'Revisión'];
  pasoActual = 0; mostrarModal = false; fichaModal: FichaNombre = 'intervinientes'; mensajeError = ''; mensajeExito = '';
  formulario = { tipoDenuncia: '', idClasificacion: 0, fechaHechoDesde: '', fechaHechoHasta: '', diaHoraConocimiento: '', idUuee: null as number | null, idDependencia: null as number | null, narracion: '' };
  borradorFicha: RegistroIngreso = {};
  registros: Record<FichaNombre, RegistroIngreso[]> = { intervinientes: [], armas: [], vehiculos: [], objetos: [], transito: [], multimedias: [] };
  fichas: FichaEvaluada[] = [];

  constructor(private readonly ingresoService: IngresoService) {}

  seleccionarTipificacion(): void {
    this.fichas = this.formulario.tipoDenuncia && this.formulario.idClasificacion ? this.ingresoService.obtenerReglas(this.formulario.tipoDenuncia, this.formulario.idClasificacion) : [];
    this.mensajeError = '';
  }
  get fichasVisibles(): FichaEvaluada[] { return this.fichas.filter((ficha) => ficha.estado !== 'oculta'); }
  cantidad(ficha: FichaNombre): number { return this.registros[ficha].length; }
  abrirFicha(ficha: FichaNombre): void { this.fichaModal = ficha; this.borradorFicha = {}; this.mensajeError = ''; this.mostrarModal = true; }
  cerrarModal(): void { this.mostrarModal = false; }
  agregarFicha(): void {
    if (!Object.values(this.borradorFicha).some((valor) => `${valor ?? ''}`.trim().length > 0)) { this.mensajeError = 'Complete al menos un dato de la ficha antes de agregarla.'; return; }
    this.registros[this.fichaModal].push({ ...this.borradorFicha }); this.cerrarModal();
  }
  eliminarFicha(ficha: FichaNombre, indice: number): void { this.registros[ficha].splice(indice, 1); }
  avanzar(): void { this.mensajeError = ''; if (this.validarPasoActual()) this.pasoActual = Math.min(this.pasoActual + 1, this.pasos.length - 1); }
  retroceder(): void { this.pasoActual = Math.max(this.pasoActual - 1, 0); }
  irAPaso(indice: number): void { if (indice <= this.pasoActual || this.validarPasoActual()) this.pasoActual = indice; }
  guardar(): void { this.mensajeError = ''; if (this.validarFormularioCompleto()) this.mensajeExito = 'La denuncia está validada y lista para enviarse al backend.'; }
  etiquetaFicha(ficha: FichaNombre): string { return { intervinientes: 'Intervinientes', armas: 'Armas', vehiculos: 'Vehículos', objetos: 'Objetos', transito: 'Tránsito', multimedias: 'Multimedias' }[ficha]; }

  private validarPasoActual(): boolean {
    if (this.pasoActual === 0 && (!this.formulario.tipoDenuncia || !this.formulario.idClasificacion)) { this.mensajeError = 'Seleccione el tipo de denuncia y su clasificación.'; return false; }
    if (this.pasoActual === 1 && (!this.formulario.fechaHechoDesde || !this.formulario.diaHoraConocimiento)) { this.mensajeError = 'Complete las fechas del hecho y de conocimiento.'; return false; }
    if (this.pasoActual === 4 && this.formulario.narracion.trim().length < 10) { this.mensajeError = 'La narración debe contener al menos 10 caracteres.'; return false; }
    return true;
  }
  private validarFormularioCompleto(): boolean {
    for (const ficha of this.fichas) {
      if (ficha.estado === 'obligatoria' && this.cantidad(ficha.fichaRequerida) < ficha.cantMinima) { this.mensajeError = `La ficha ${this.etiquetaFicha(ficha.fichaRequerida)} requiere al menos ${ficha.cantMinima} registro(s).`; this.pasoActual = 3; return false; }
    }
    return this.validarDatosBasicos();
  }
  private validarDatosBasicos(): boolean {
    const valido = Boolean(this.formulario.tipoDenuncia && this.formulario.idClasificacion && this.formulario.fechaHechoDesde && this.formulario.diaHoraConocimiento && this.formulario.narracion.trim().length >= 10);
    if (!valido) this.mensajeError = 'Complete los datos obligatorios antes de guardar.';
    return valido;
  }
}