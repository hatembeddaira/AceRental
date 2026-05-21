import { Component, OnInit, signal, PLATFORM_ID, Inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('ace.sound.fr');
  constructor(private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}
  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) return;

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
}
