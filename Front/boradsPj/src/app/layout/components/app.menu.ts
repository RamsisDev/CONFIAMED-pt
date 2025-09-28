import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppMenuitem } from './app.menuitem';

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [CommonModule, AppMenuitem, RouterModule],
    template: `<ul class="layout-menu">
        <ng-container *ngFor="let item of model; let i = index">
            <li
                app-menuitem
                *ngIf="!item.separator"
                [item]="item"
                [index]="i"
                [root]="true"
            ></li>
            <li *ngIf="item.separator" class="menu-separator"></li>
        </ng-container>
    </ul> `,
})
export class AppMenu {
    model: any[] = [];

    ngOnInit() {
        this.model = [
            {
                label: 'Dashboards',
                icon: 'pi pi-home',
                items: [
                    {
                        label: 'Inicio',
                        icon: 'pi pi-fw pi-home',
                        routerLink: ['/'],
                    },
                    {
                        label: 'Tareas',
                        icon: 'pi pi-fw pi-sliders-v',
                        routerLink: ['/dashboard-banking'],
                    },
                ],
            },
            {
                label: 'Admins',
                icon: 'pi pi-th-large',
                items: [
                    {
                        label: 'Personal',
                        icon: 'pi pi-id-card',
                        items: [
                            {
                                label: 'Lista',
                                icon: 'pi pi-fw pi-image',
                                routerLink: ['/apps/blog/list'],
                            },
                            {
                                label: 'Detalles',
                                icon: 'pi pi-fw pi-list',
                                routerLink: ['/apps/blog/detail'],
                            },
                            {
                                label: 'Editar',
                                icon: 'pi pi-fw pi-pencil',
                                routerLink: ['/apps/blog/edit'],
                            },
                            {
                                label: 'Nuevo',
                                icon: 'pi pi-fw pi-user-plus',
                                routerLink: ['/auth/register'],
                            },
                        ],
                    },
                    {
                        label: 'Task List',
                        icon: 'pi pi-fw pi-check-square',
                        routerLink: ['/apps/tasklist'],
                    },
                ],
            },
            {
              //label: 'Cuenta',
              icon: 'pi pi-fw pi-user',
              items: [
                  {
                      label: 'Cerrar Sesion',
                      icon: 'pi pi-fw pi-sign-in',
                      routerLink: ['/auth/login'],
                  },
                  {
                      label: 'Suspender sesion',
                      icon: 'pi pi-fw pi-eye-slash',
                      routerLink: ['/auth/lockscreen'],
                  },
              ],
          },
        ];
    }
}
