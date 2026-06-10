import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AcceuilComponent } from './acceuil.component';
import { ServicesComponent } from './services/services.component';
import { InstallationComponent } from './instalation/installation.component';
import { SonorisationComponent } from './location/sonorisation/sonorisation.component';
import { LumiereComponent } from './location/lumiere/lumiere.component';
import { PackComponent } from './location/pack/pack.component';
import { EquipmentComponent } from './location/equipment/equipment.component';
import { BasketComponent } from './basket/basket.component';

const routes: Routes = [
  {path:'', data: { breadcrumb: 'Acceuil' }, component:AcceuilComponent},
  {path:'services', data: { breadcrumb: 'Service' }, component:ServicesComponent},
  {path:'installation', data: { 
    breadcrumb: 'Installation', 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' }
    ]}, component:InstallationComponent},
  {path:'sonorisation', data: {
    breadcrumb: 'Sonorisation', 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' }
    ]}, component:SonorisationComponent},
  {path:'panier', data: {
    breadcrumb: 'Panier', 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' }
    ]}, component:BasketComponent},
  {path:'lumiere', data: { breadcrumb: 'Lumière' }, component:LumiereComponent},
  {path:'equipment/:id', data: { breadcrumb: 'Équipement' }, component:EquipmentComponent},
  {path:'pack/:id', data: { 
    breadcrumb: 'Pack' , 
    breadcrumbParents: [
      { label: 'Acceuil', url: '/' },
      { label: 'Sonorisation', url: '/sonorisation' }
    ]}, component:PackComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AcceuilRoutingModule { }
