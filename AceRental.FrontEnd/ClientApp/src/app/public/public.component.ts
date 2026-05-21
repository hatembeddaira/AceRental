import { Component } from '@angular/core';
import { PromoComponent } from "./@theme/components/promo/promo.component";
import { HeaderComponent } from "./@theme/components/header/header.component";
import { FooterComponent } from "./@theme/components/footer/footer.component";
import { RouterModule } from '@angular/router';
import { BreadcrumbComponent } from "./features/acceuil/breadcrumb/breadcrumb.component";

@Component({
  selector: 'app-public',
  standalone: true,
  imports: [
    PromoComponent,
    HeaderComponent,
    FooterComponent,
    RouterModule,
    BreadcrumbComponent
],
  templateUrl: './public.component.html',
  styleUrl: './public.component.css',
})
export class PublicComponent {

}
