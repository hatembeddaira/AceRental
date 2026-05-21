import { Component } from '@angular/core';
import { AcceuilRoutingModule } from "../../../features/acceuil/acceuil-routing-module";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [AcceuilRoutingModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {

}
