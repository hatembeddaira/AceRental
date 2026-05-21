import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommandesComponent } from './features/gestion/commandes/commandes.component';
import { CommandeDetailsComponent } from './features/gestion/commandes/commandeDetails/commande-details.component';
import { AdminComponent } from './admin.component';
import { DevisViewerComponent } from './features/gestion/devis/devis-viewer/devis-viewer.component';

const routes: Routes = [
   {path:'', component: AdminComponent, children:[
      {path:'', loadChildren:()=> import('./features/gestion/gestion-module').then(m=> m.GestionModule)}
    ]},
  //  {path:'gestion', component: AdminComponent, children:[
  //     {path:'', loadChildren:()=> import('./features/gestion/gestion-module').then(m=> m.GestionModule)}
  //   ]},
   {path:'administration', component: AdminComponent, children:[
      {path:'', loadChildren:()=> import('./features/administration/administration-module').then(m=> m.AdministrationModule)}
    ]},

  //  {path:'', component: AdminComponent, children:[
  //     // {path:'', data: { breadcrumb: 'Commandes' }, component:CommandesComponent},
  //     {path:'commandes', data: { breadcrumb: 'Commandes' }, component:CommandesComponent},
  //     {path:'devis', data: { breadcrumb: 'Devis' }, component:DevisViewerComponent},
  //     {path:'devis/:id', data: { 
  //       breadcrumb: 'Pack' , 
  //       breadcrumbParents: [
  //         { label: 'Devis', url: '/admin' },
  //       ]}, component:DevisViewerComponent},
  //     {path:'commande/:id', data: { 
  //       breadcrumb: 'Pack' , 
  //       breadcrumbParents: [
  //         { label: 'Commandes', url: '/admin' },
  //       ]}, component:CommandeDetailsComponent},
  //     {path:'administration', loadChildren:()=> import('./features/administration/administration-module').then(m=> m.AdministrationModule)},
      
  //   ]}
    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
