import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { OAuthService } from "angular-oauth2-oidc";
import { jwtDecode } from "jwt-decode";

export const authGuard: CanActivateFn = (route, state) => {
  const oauthService = inject(OAuthService);
  const router = inject(Router);

  if (oauthService.hasValidAccessToken()) {
    // Vérification optionnelle du rôle
    const claims = oauthService.getAccessToken() ? jwtDecode(oauthService.getAccessToken()) : null;
    
    // Si la route est '/admin' et que l'utilisateur n'est pas Admin, on redirige
    //&& claims?.['role'] !== 'Admin'
    if (state.url.includes('admin') ) {
      return router.createUrlTree(['/']);
    }
    return true;
  }

  // Si pas connecté, on init le flux en lui disant où revenir
  oauthService.initCodeFlow(state.url); 
  return false;
};