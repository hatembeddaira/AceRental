import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { ServicesComponent } from './features/services/services.component';
import { EquipmentsComponent } from './features/equipments/equipments.component';
import { EquipmentComponent } from './features/equipment/equipment.component';
import { PacksComponent } from './features/packs/packs.component';
import { ClientsComponent } from './features/clients/clients.component';
import { ClientComponent } from './features/client/client.component';
import { ReservationsComponent } from './features/reservations/reservations.component';
import { ReservationComponent } from './features/reservation/reservation.component';
import { DevisComponent } from './features/devis/devis.component';
import { DevisViewerComponent } from './features/devis/devis-viewer/devis-viewer.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { authGuard } from '../auth.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        component: DashboardComponent,
        canActivate: [authGuard],
        data: {
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }
      },
      {
        path: 'reservations',
        component: ReservationsComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Reservations',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }
      },
      {
        path: 'services',
        component: ServicesComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Services',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }
      },
      {
        path: 'equipments',
        component: EquipmentsComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Equipments',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }
      },
      {
        path: 'equipment/:id',
        component: EquipmentComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Equipment',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' },
            { label: 'Equipments', url: '/admin/equipments' }
          ]
        }
      },
      {
        path: 'packs',
        component: PacksComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Packs',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }
      },
      {
        path: 'clients',
        component: ClientsComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Clients',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }
      },
      {
        path: 'client/:id',
        component: ClientComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Fiche Client',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' },
            { label: 'Clients', url: '/admin/clients' }
          ]
        }
      },
      {
        path: 'reservation',
        component: ReservationComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Créer une reservation',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
            { label: 'Reservations ', url: '/admin/reservations' },
          ]
        }
      },
      {
        path: 'reservation/:id',
        component: ReservationComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Reservation Détails',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
            { label: 'Reservations ', url: '/admin/reservations' },
          ]
        }
      },
      {
        path: 'devis',
        component: DevisComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Devis',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }
      },
      {
        path: 'devis/:id',
        component: DevisViewerComponent,
        canActivate: [authGuard],
        data: {
          breadcrumb: 'Devis Viewer',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
