import { inject, Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private oauthService = inject(OAuthService);

  login() {
    this.oauthService.initCodeFlow(); // Redirige vers le serveur d'authentification
    // localStorage.setItem('token', this.oauthService.getAccessToken())
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
  get accessToken() {
    return this.oauthService.getAccessToken(); // Contient le nom, rôles, email, etc.
  }
  public getClaimsFromAccessToken() {
    const token = this.oauthService.getAccessToken();
    if (!token) return null;

    // Décodage manuel de la partie payload du JWT
    const payload = token.split('.')[1];
    return JSON.parse(window.atob(payload));
  }
}