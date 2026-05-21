import { ApplicationConfig, provideBrowserGlobalErrorListeners, importProvidersFrom  } from '@angular/core';
import { provideRouter } from '@angular/router';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(NgxExtendedPdfViewerModule),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    {
      provide : MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue:{appearance : 'outline', subscriptSizing : 'dynamic'}
    }
  ]
};
