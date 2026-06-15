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

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        component: DashboardComponent,
        data: {
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }
      },
      {
        path: 'reservations',
        component: ReservationsComponent,
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
