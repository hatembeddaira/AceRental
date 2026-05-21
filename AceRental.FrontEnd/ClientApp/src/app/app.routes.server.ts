import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Pages publiques statiques pré-rendues
  { path: '', renderMode: RenderMode.Prerender },
  { path: 'services', renderMode: RenderMode.Prerender },
  { path: 'installation', renderMode: RenderMode.Prerender },
  { path: 'sonorisation', renderMode: RenderMode.Prerender },
  { path: 'lumiere', renderMode: RenderMode.Prerender },

  // Routes dynamiques et admin rendues côté serveur
  { path: 'admin/**', renderMode: RenderMode.Server },
  { path: '**', renderMode: RenderMode.Server }
];
