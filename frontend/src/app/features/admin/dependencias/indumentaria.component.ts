import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

type TipoInventario = 'armas' | 'chalecos' | 'esposas';

@Component({
  selector: 'app-dependencias-indumentaria',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './indumentaria.component.html',
  styleUrls: ['./indumentaria.component.scss']
})
export class DependenciasIndumentariaComponent {
  tipoActivo: TipoInventario = 'armas';
  armas: any[] = [
    { id: 1, nombre: 'Pistola 9mm', serie: 'ABC-001', estado: 'Disponible' },
    { id: 2, nombre: 'Rifle corto', serie: 'RIF-22', estado: 'Disponible' }
  ];
  chalecos: any[] = [
    { id: 1, nombre: 'Chaleco nivel II', talle: 'L', estado: 'Disponible' },
    { id: 2, nombre: 'Chaleco nivel III', talle: 'M', estado: 'Disponible' }
  ];
  esposas: any[] = [
    { id: 1, nombre: 'Juego estándar', marca: 'Protec', estado: 'Disponible' },
    { id: 2, nombre: 'Juego reforzado', marca: 'Safar', estado: 'Disponible' }
  ];

  seleccionado: any = null;
  formulario: any = { nombre: '', serie: '', marca: '', estado: 'Disponible' };
  mensaje = '';

  cambiarTipo(tipo: TipoInventario): void {
    this.tipoActivo = tipo;
    this.seleccionado = null;
    this.formulario = { nombre: '', serie: '', marca: '', estado: 'Disponible' };
  }

  guardar(): void {
    if (!this.formulario.nombre) {
      this.mensaje = 'El nombre es obligatorio.';
      return;
    }

    if (this.tipoActivo === 'armas') {
      this.armas.push({
        id: Date.now(),
        nombre: this.formulario.nombre,
        serie: this.formulario.serie,
        estado: this.formulario.estado
      });
    }

    if (this.tipoActivo === 'chalecos') {
      this.chalecos.push({
        id: Date.now(),
        nombre: this.formulario.nombre,
        talle: this.formulario.serie || 'L',
        estado: this.formulario.estado
      });
    }

    if (this.tipoActivo === 'esposas') {
      this.esposas.push({
        id: Date.now(),
        nombre: this.formulario.nombre,
        marca: this.formulario.marca,
        estado: this.formulario.estado
      });
    }

    this.mensaje = 'Item agregado correctamente.';
    this.formulario = { nombre: '', serie: '', marca: '', estado: 'Disponible' };
  }

  editar(item: any): void {
    this.seleccionado = item;
    this.formulario = { ...item };
  }

  eliminar(item: any): void {
    if (this.tipoActivo === 'armas') {
      this.armas = this.armas.filter((x) => x.id !== item.id);
    }
    if (this.tipoActivo === 'chalecos') {
      this.chalecos = this.chalecos.filter((x) => x.id !== item.id);
    }
    if (this.tipoActivo === 'esposas') {
      this.esposas = this.esposas.filter((x) => x.id !== item.id);
    }
    this.mensaje = 'Item eliminado.';
  }

  get elementos(): any[] {
    if (this.tipoActivo === 'armas') return this.armas;
    if (this.tipoActivo === 'chalecos') return this.chalecos;
    return this.esposas;
  }
}
