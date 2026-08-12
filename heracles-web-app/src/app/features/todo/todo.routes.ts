import { Routes } from '@angular/router';

export const TODO_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./todos-page/todos-page').then(m => m.TodosPage)
  }
]
