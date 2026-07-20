import { Component, OnInit, signal, PLATFORM_ID, Inject, inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { OAuthService } from 'angular-oauth2-oidc';
import { authCodeFlowConfig } from './auth.config';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private oauthService = inject(OAuthService);
  protected readonly title = signal('ace.sound.fr');
  constructor(private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }
  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) return;
    this.configureAuth();
    // GTM: dataLayer
    (window as any).dataLayer = (window as any).dataLayer || [];

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        (window as any).dataLayer.push({
          event: 'page_view',
          page_path: event.urlAfterRedirects
        });
      }
    });


  }
  private configureAuth() {
    this.oauthService.configure(authCodeFlowConfig);

    // Charge les configurations du serveur et tente de récupérer le token
    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
      // 1. Vérifie si le token est valide
      if (this.oauthService.hasValidAccessToken()) {
        console.log('Authentification réussie, jeton récupéré.');
        // console.log('Jeton complet :', this.oauthService.getAccessToken());

        // // Pour récupérer l'objet complet
        // const claims = this.getClaimsFromAccessToken() as any;
        // console.log('Utilisateur :', claims.name);
        // console.log('Role :', claims.role);
        // console.log('Email :', claims.email);
        // console.log('IdUser :', claims.sub);
        // if (claims.role === 'Admin') {
        //   this.router.navigate(['/admin']);
        // }
        // else { this.router.navigate(['/']); }
      } else {
        console.log('Utilisateur non connecté.');
      }
    });

    // Optionnel : Active le rafraîchissement automatique du token
    this.oauthService.setupAutomaticSilentRefresh();
  }

  getClaimsFromAccessToken() {
    const token = this.oauthService.getAccessToken();
    if (!token) return null;

    // Décodage manuel de la partie payload du JWT
    const payload = token.split('.')[1];
    return JSON.parse(window.atob(payload));
  }
}
