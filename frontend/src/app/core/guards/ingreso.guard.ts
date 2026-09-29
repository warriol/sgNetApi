import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const ingresoGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const roles = authService.obtenerRolesDelToken();

  if (roles.includes('Ingreso') || roles.includes('Administrador')) {
    return true;
  }

  return router.createUrlTree(['/admin/dashboard']);
};