import { Routes } from '@angular/router';

export const routes: Routes = [
    {path:'', loadChildren:()=> import('./public/public-routing-module').then(m=> m.PublicRoutingModule)},
    {path:'admin', loadChildren:()=> import('./admin/admin-routing-module').then(m=> m.AdminRoutingModule), 
        data: { ssr: false, prerender: false }
    }
    
];
