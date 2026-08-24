import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LandingComponent } from './features/landing/landing.component';
import { AdminShellComponent } from './features/admin/shell/admin-shell.component';
import { catalogosGuard } from './core/guards/catalogos.guard';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  {
    path: 'admin',
    component: AdminShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/admin/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent
          )
      },
      {
        path: 'usuarios',
        loadComponent: () =>
          import('./features/admin/usuarios/usuarios.component').then(
            (m) => m.UsuariosComponent
          )
      },
      {
        path: 'roles',
        loadComponent: () =>
          import('./features/admin/roles/roles.component').then(
            (m) => m.RolesComponent
          )
      },
      {
        path: 'catalogos',
        canActivate: [catalogosGuard],
        loadComponent: () =>
          import('./features/admin/catalogos/catalogos.component').then(
            (m) => m.CatalogosComponent
          )
      },
      {
        path: 'auditoria',
        loadComponent: () =>
          import('./features/admin/auditoria/auditoria.component').then(
            (m) => m.AuditoriaComponent
          )
      }
    ]
  },
  { path: '**', redirectTo: '' }
];