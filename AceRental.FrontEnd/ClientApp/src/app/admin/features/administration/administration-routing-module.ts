import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ServicesComponent } from './services/services.component';
import { ProduitsComponent } from './produits/produits.component';
import { PacksComponent } from './packs/packs.component';
import { ClientsComponent } from './clients/clients.component';
import { FicheClientComponent } from './ficheClient/fiche-client.component';
import { FicheProduitComponent } from './ficheProduit/fiche-produit.component';

const routes: Routes = [
  
  {path:'services', data: { 
    breadcrumb: 'Services' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:ServicesComponent}, 
  {path:'produits', data: { 
    breadcrumb: 'Produits' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:ProduitsComponent},
  {path:'produit/:id', data: { 
    breadcrumb: 'Fiche Produit' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' },
      { label: 'Produits', url: '/admin/administration/produits' }
    ]}, component:FicheProduitComponent},
  {path:'packs', data: { 
    breadcrumb: 'Packs' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:PacksComponent},
  {path:'clients', data: { 
    breadcrumb: 'Clients' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:ClientsComponent},
  {path:'client/:id', data: { 
    breadcrumb: 'Fiche Client' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' },
      { label: 'Clients', url: '/admin/administration/clients' }
    ]}, component:FicheClientComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrationRoutingModule { }
