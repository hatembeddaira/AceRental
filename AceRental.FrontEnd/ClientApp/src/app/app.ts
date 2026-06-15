import { Component, OnInit, signal, PLATFORM_ID, Inject, inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
// import { OAuthService } from 'angular-oauth2-oidc';
// import { authCodeFlowConfig } from './auth.config';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  // private oauthService = inject(OAuthService);
  protected readonly title = signal('ace.sound.fr');
  constructor(private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }
  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) return;
    // this.configureAuth();
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
  // private configureAuth() {
  //   this.oauthService.configure(authCodeFlowConfig);
    
  //   // Charge les configurations du serveur et tente de récupérer le token
  //   this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
  //     console.log('Authentification initialisée avec succès');
  //   });

  //   // Optionnel : Active le rafraîchissement automatique du token
  //   this.oauthService.setupAutomaticSilentRefresh();
  // }
}
