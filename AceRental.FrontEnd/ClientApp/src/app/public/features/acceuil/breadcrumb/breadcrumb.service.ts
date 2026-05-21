import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

export interface Breadcrumb {
  label: string;
  url: string;
}

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {

  breadcrumbs: Breadcrumb[] = [];

  constructor(private router: Router) {
      this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe(() => {
        const root = this.router.routerState.snapshot.root;
        this.breadcrumbs = this.getBreadcrumbs(root);
      });
  }

  private getBreadcrumbs(route: ActivatedRouteSnapshot, url: string = '', breadcrumbs: Breadcrumb[] = []): Breadcrumb[] 
  {
    if (route.data['breadcrumbParents']) {
      route.data['breadcrumbParents'].forEach((root: any, index: number) => {
        breadcrumbs.push(root);
      });
    }
    // Récupère le breadcrumb de la route
    if (route.data['breadcrumb']) {
      let routePath = route.routeConfig?.path || '';
      
      // Remplace les params (ex: :id)
      if (route.params) {
        for (const key of Object.keys(route.params)) {
          routePath = routePath.replace(':' + key, route.params[key]);
        }
      }
      
      if (routePath) {
        url += `/${routePath}`;
      }

      breadcrumbs.push({ label: route.data['breadcrumb'], url });
    }

    // Descend dans tous les enfants
    if (route.children && route.children.length > 0) {
      route.children.forEach(child => {
        this.getBreadcrumbs(child, url, breadcrumbs);
      });
    }

    return breadcrumbs;
  }
}