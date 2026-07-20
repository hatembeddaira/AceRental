import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { OAuthService } from "angular-oauth2-oidc";

export const authGuard: CanActivateFn = (route, state) => {
  const oauthService = inject(OAuthService);
  const router = inject(Router);

  if (oauthService.hasValidIdToken() && oauthService.hasValidAccessToken()) {
    const claims = oauthService.getIdentityClaims();
    console.log('claims', claims);

    if (!claims) {
      oauthService.initCodeFlow(state.url);
      return false;
    }

    if (state.url.startsWith('/admin')) {
      const roles = claims['role'] as string | string[];
console.log('roles', roles);
      if (Array.isArray(roles) ? !roles.includes('Admin') : roles !== 'Admin') {
        return router.createUrlTree(['/']);
      }
    }

    // 5. Si toutes les conditions sont remplies, autoriser l'accès.
    return true;
  }

  // Si pas connecté, on init le flux en lui disant où revenir
  oauthService.initCodeFlow(state.url); 
  return false;
};