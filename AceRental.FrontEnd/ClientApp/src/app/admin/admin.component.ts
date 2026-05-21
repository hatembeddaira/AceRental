import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './@theme/components/header/header.component';
import { FooterComponent } from './@theme/components/footer/footer.component';
import { BreadcrumbComponent } from "../public/features/acceuil/breadcrumb/breadcrumb.component";

@Component({
  selector: 'app-admin.component',
  standalone: true,
  imports: [HeaderComponent,
    FooterComponent,
    RouterModule, BreadcrumbComponent
  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css',
})
export class AdminComponent {

}
