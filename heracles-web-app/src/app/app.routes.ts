import { Routes } from '@angular/router';
import { authenticationGuard } from './core/guards/authentication/authentication-guard';
import { guestGuard } from './core/guards/guest/guest-guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/authentication/login-page/login-page').then(m => m.LoginPage)
  },
  {
    path: 'todos',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./features/todo/todo.routes').then(m => m.TODO_ROUTES)
  },
  {
    path: "",
    redirectTo: 'todos',
    pathMatch: 'full'
  },
  {
    path: '*',
    redirectTo: 'todos'
  }
];
