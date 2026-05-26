import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

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
  //     // {path:'', data: { breadcrumb: 'Reservations' }, component:ReservationsComponent},
  //     {path:'Reservations', data: { breadcrumb: 'Reservations' }, component:ReservationsComponent},
  //     {path:'devis', data: { breadcrumb: 'Devis' }, component:DevisViewerComponent},
  //     {path:'devis/:id', data: { 
  //       breadcrumb: 'Pack' , 
  //       breadcrumbParents: [
  //         { label: 'Devis', url: '/admin' },
  //       ]}, component:DevisViewerComponent},
  //     {path:'Reservation/:id', data: { 
  //       breadcrumb: 'Pack' , 
  //       breadcrumbParents: [
  //         { label: 'Reservations', url: '/admin' },
  //       ]}, component:ReservationComponent},
  //     {path:'administration', loadChildren:()=> import('./features/administration/administration-module').then(m=> m.AdministrationModule)},
      
  //   ]}
    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
