import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./sections/dashboard/pages/dashboard/dashboard').then(
        (component) => component.Dashboard,
      ),
  },
  {
    path: 'products',
    loadComponent: () =>
      import('./sections/products/pages/product-landing/product-landing').then(
        (component) => component.ProductLanding,
      ),
  },
  {
    path: 'categories',
    loadChildren: () =>
      import('./sections/categories/category.routes').then(
        (routing) => routing.categoryRoutes,
      ),
  },
  { path: '**', redirectTo: 'dashboard' },
];
