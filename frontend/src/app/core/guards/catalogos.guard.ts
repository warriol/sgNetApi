import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const catalogosGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  return authService.obtenerPermisosDelToken().includes('admin.catalogos.gestion')
    ? true
    : router.createUrlTree(['/admin/dashboard']);
};
