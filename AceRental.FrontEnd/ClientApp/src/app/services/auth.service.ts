import { inject, Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private oauthService = inject(OAuthService);

  login() {
    this.oauthService.initCodeFlow(); // Redirige vers le serveur d'authentification
  }

  logout() {
    this.oauthService.logOut();
  }

  get isLoggedIn(): boolean {
    return this.oauthService.hasValidAccessToken() && this.oauthService.hasValidIdToken();
  }

  get identityClaims() {
    return this.oauthService.getIdentityClaims(); // Contient le nom, rôles, email, etc.
  }
}