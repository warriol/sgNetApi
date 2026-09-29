import { Injectable } from '@angular/core';

export type FichaNombre = 'intervinientes' | 'armas' | 'vehiculos' | 'objetos' | 'transito' | 'multimedias';

export interface ReglaTipificacionFicha {
  idClasificacion: number;
  tipoDenuncia?: string;
  fichaRequerida: FichaNombre;
  cantMinima: number;
  rolPersonaRequerido?: string;
  visible: boolean;
}

export interface FichaEvaluada extends ReglaTipificacionFicha {
  estado: 'obligatoria' | 'opcional' | 'oculta';
}

@Injectable({ providedIn: 'root' })
export class IngresoService {
  private readonly reglas: ReglaTipificacionFicha[] = [
    { idClasificacion: 1, tipoDenuncia: 'DelitoFalta', fichaRequerida: 'intervinientes', cantMinima: 1, rolPersonaRequerido: 'Victima', visible: true },
    { idClasificacion: 1, tipoDenuncia: 'DelitoFalta', fichaRequerida: 'objetos', cantMinima: 1, visible: true },
    { idClasificacion: 2, tipoDenuncia: 'DelitoFalta', fichaRequerida: 'intervinientes', cantMinima: 1, rolPersonaRequerido: 'Victima', visible: true },
    { idClasificacion: 2, tipoDenuncia: 'DelitoFalta', fichaRequerida: 'vehiculos', cantMinima: 1, visible: true },
    { idClasificacion: 3, tipoDenuncia: 'Accidente', fichaRequerida: 'intervinientes', cantMinima: 2, visible: true },
    { idClasificacion: 3, tipoDenuncia: 'Accidente', fichaRequerida: 'vehiculos', cantMinima: 1, visible: true },
    { idClasificacion: 3, tipoDenuncia: 'Accidente', fichaRequerida: 'transito', cantMinima: 1, visible: true }
  ];

  obtenerReglas(tipoDenuncia: string, idClasificacion: number): FichaEvaluada[] {
    const aplicables = this.reglas.filter((regla) => regla.idClasificacion === idClasificacion && (!regla.tipoDenuncia || regla.tipoDenuncia === tipoDenuncia));
    const fichas: FichaNombre[] = ['intervinientes', 'armas', 'vehiculos', 'objetos', 'transito', 'multimedias'];
    return fichas.map((ficha) => {
      const regla = aplicables.find((item) => item.fichaRequerida === ficha);
      return regla ? { ...regla, estado: regla.cantMinima > 0 ? 'obligatoria' : 'opcional' } : { idClasificacion, fichaRequerida: ficha, cantMinima: 0, visible: false, estado: 'oculta' };
    });
  }
}