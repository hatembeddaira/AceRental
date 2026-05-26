import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ServicesComponent } from './services/services.component';
import { EquipmentsComponent } from './equipments/equipments.component';
import { PacksComponent } from './packs/packs.component';
import { ClientsComponent } from './clients/clients.component';
import { FicheClientComponent } from './ficheClient/fiche-client.component';
import { EquipmentComponent } from './equipment/equipment.component';

const routes: Routes = [
  
  {path:'services', data: { 
    breadcrumb: 'Services' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:ServicesComponent}, 
  {path:'equipments', data: { 
    breadcrumb: 'Equipments' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' }
    ]}, component:EquipmentsComponent},
  {path:'equipment/:id', data: { 
    breadcrumb: 'Equipment' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/admin' },
      { label: 'Administration' },
      { label: 'Equipments', url: '/admin/administration/equipments' }
    ]}, component:EquipmentComponent},
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
