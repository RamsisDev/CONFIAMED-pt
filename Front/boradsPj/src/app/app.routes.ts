import { Routes } from '@angular/router';
import { AppLayout } from './layout/components/app.layout';
import { Notfound } from './apps/common/notfound/notfound';

export const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    children:[
      {
        path: '',
        loadComponent: () => import('./apps/common/inicio/inicio').then((c) => c.inicio),
        data: { breadcrumb: 'Dashboard' }
      },
      {
        path: 'tareas',
        loadComponent: () => import('./apps/kanban/index').then((c)=> c.Kanban),
        data: { breadcrumb: 'Tareas' }
      }
    ]

  },
  { path: 'notfound', component: Notfound },
  {
        path: 'auth',
        loadChildren: () => import('@/apps/auth/auth.routes'),
  },
  { path: '**', redirectTo: '/notfound' },
];
