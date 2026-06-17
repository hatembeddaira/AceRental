import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-header',
  imports: [RouterModule, MatMenuModule, MatButtonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  authService: AuthService;
  constructor(private _authService: AuthService) {
    this.authService = _authService;
  }
  
  onLoginClicked()
  {
    this.authService.login();
    console.log('accessToken', this.authService.accessToken);
  }
}
