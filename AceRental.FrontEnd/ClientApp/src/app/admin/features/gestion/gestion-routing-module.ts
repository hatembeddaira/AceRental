import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReservationsComponent } from './reservations/reservations.component';
import { DevisViewerComponent } from './devis/devis-viewer/devis-viewer.component';
import { ReservationComponent } from './reservation/reservation.component';
import { DevisComponent } from './devis/devis.component';

const routes: Routes = [
  
  {path:'', data: { 
    breadcrumb: 'Reservations',
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' }
    ]}, component:ReservationsComponent}, 
  {path:'reservation', data: { 
    breadcrumb: 'Créer une reservation',
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' },
      { label: 'Reservations ', url: '/admin' },
    ]}, component:ReservationComponent},
  {path:'reservation/:id', data: { 
    breadcrumb: 'Reservation Détails' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' },
      { label: 'Reservations ', url: '/admin' },
    ]}, component:ReservationComponent},
  {path:'devis', data: { 
    breadcrumb: 'Devis',
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' }
    ]}, component:DevisComponent},  
  {path:'devis/:id', data: { 
    breadcrumb: 'Devis Viewer',
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' }
    ]}, component:DevisViewerComponent}  
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionRoutingModule { }
