import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import { AuthService } from '../../../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [RouterModule, MatMenuModule, MatButtonModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  constructor(public authService: AuthService) {
  }

  get userName(): string | null {
    // On récupère les informations de l'utilisateur depuis le ID token, pas l'access token.
    // L'ID token est destiné au client (Angular), l'access token est pour l'API.
    const claims = this.authService.getClaimsFromAccessToken() as any;
    return claims ? claims.name : null;
  }
  
  onLoginClicked()
  {
    this.authService.login();
  }

  onLogoutClicked() {
    this.authService.logout();
  }
}
