
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
    selector: 'app-inicio',
    standalone: true,
    imports: [RouterModule, ButtonModule],
    template: `
        <section class="w-full max-w-2xl content-center rounded-2xl border border-slate-200 dark:border-neutral-800 bg-white/80 dark:bg-neutral-900/80 backdrop-blur shadow-lg">
          <header class="px-6 py-5 border-b border-slate-200 dark:border-neutral-800">
            <h1 class="text-xl font-semibold text-slate-800 dark:text-slate-100">
              Perfil (mockup) — Administrador
            </h1>
            <p class="mt-1 text-sm text-slate-500 dark:text-slate-400">
              Vista preliminar del panel de administración.
            </p>
          </header>

          <div class="px-6 py-6">
            <h2 class="text-base font-medium text-slate-700 dark:text-slate-200">
              Características de esta demo
            </h2>

            <ul class="mt-3 space-y-2 text-sm text-slate-600 dark:text-slate-300">
              <li class="flex items-start gap-3">
                <span class="mt-0.5 inline-block h-2 w-2 rounded-full bg-blue-500"></span>
                <span>Listar las tareas dentro de la base de datos por usuario, el administrador ve <span class="font-semibold">todas</span>.</span>
              </li>
              <li class="flex items-start gap-3">
                <span class="mt-0.5 inline-block h-2 w-2 rounded-full bg-blue-500"></span>
                <span>Crear y asignar (o no) tareas nuevas.</span>
              </li>
              <li class="flex items-start gap-3">
                <span class="mt-0.5 inline-block h-2 w-2 rounded-full bg-blue-500"></span>
                <span>Cambiar el estado de las tareas.</span>
              </li>
            </ul>

            <div class="mt-6">
              <button
                type="button"
                [routerLink]="['/tareas']"
                class="inline-flex items-center gap-2 rounded-lg px-4 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 active:bg-blue-800 focus:outline-none focus:ring-2 focus:ring-blue-500/50 transition"
              >
                Ir al listado de tareas
              </button>
            </div>
          </div>
        </section>

    `
})
export class inicio {}
