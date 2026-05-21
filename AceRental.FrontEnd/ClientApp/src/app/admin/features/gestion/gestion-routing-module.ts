import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommandesComponent } from './commandes/commandes.component';
import { DevisViewerComponent } from './devis/devis-viewer/devis-viewer.component';
import { CommandeDetailsComponent } from './commandes/commandeDetails/commande-details.component';
import { DevisComponent } from './devis/devis.component';

const routes: Routes = [
  
  {path:'', data: { 
    breadcrumb: 'Commandes',
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' }
    ]}, component:CommandesComponent}, 
  {path:'commande/:id', data: { 
    breadcrumb: 'Commande Détails' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Gestion', url: '/gestion' },
      { label: 'Commandes ', url: '/admin' },
    ]}, component:CommandeDetailsComponent},
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
