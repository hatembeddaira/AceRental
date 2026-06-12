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
    path: '', component: AdminComponent, children: [
      {
        path: '', data: {
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }, component: DashboardComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'reservations', data: {
          breadcrumb: 'Reservations',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }, component: ReservationsComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'services', data: {
          breadcrumb: 'Services',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }, component: ServicesComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'equipments', data: {
          breadcrumb: 'Equipments',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }, component: EquipmentsComponent
      }
    ]
  },







  {
    path: '', component: AdminComponent, children: [
      {
        path: 'equipment/:id', data: {
          breadcrumb: 'Equipment',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' },
            { label: 'Equipments', url: '/admin/equipments' }
          ]
        }, component: EquipmentComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'packs', data: {
          breadcrumb: 'Packs',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }, component: PacksComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'clients', data: {
          breadcrumb: 'Clients',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' }
          ]
        }, component: ClientsComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'client/:id', data: {
          breadcrumb: 'Fiche Client',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/admin' },
            { label: 'Clients', url: '/admin/clients' }
          ]
        }, component: ClientComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'reservation', data: {
          breadcrumb: 'Créer une reservation',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
            { label: 'Reservations ', url: '/admin/reservations' },
          ]
        }, component: ReservationComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'reservation/:id', data: {
          breadcrumb: 'Reservation Détails',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
            { label: 'Reservations ', url: '/admin/reservations' },
          ]
        }, component: ReservationComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'reservation/:id', data: {
          breadcrumb: 'Reservation Détails',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
            { label: 'Reservations ', url: '/admin' },
          ]
        }, component: ReservationComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'devis', data: {
          breadcrumb: 'Devis',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }, component: DevisComponent
      }
    ]
  },
  {
    path: '', component: AdminComponent, children: [
      {
        path: 'devis/:id', data: {
          breadcrumb: 'Devis Viewer',
          breadcrumbParents: [
            { label: 'Acceuil', url: '/' },
          ]
        }, component: DevisViewerComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
