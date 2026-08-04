import { Routes } from '@angular/router';

export const categoryRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/category-landing/category-landing').then(
        (component) => component.CategoryLanding,
      ),
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./pages/category-create/category-create').then(
        (component) => component.CategoryCreate,
      ),
  },
  {
    path: ':id/update',
    loadComponent: () =>
      import('./pages/category-update/category-update').then(
        (component) => component.CategoryUpdate,
      ),
  },
];
