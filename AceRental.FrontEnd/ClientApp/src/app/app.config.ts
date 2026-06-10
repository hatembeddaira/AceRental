import { ApplicationConfig, provideBrowserGlobalErrorListeners, importProvidersFrom, LOCALE_ID  } from '@angular/core';
import { provideRouter } from '@angular/router';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import localeFr from '@angular/common/locales/fr';
import { registerLocaleData } from '@angular/common';

registerLocaleData(localeFr);

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(NgxExtendedPdfViewerModule),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    {
      provide : MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue:{appearance : 'outline', subscriptSizing : 'dynamic'}
    },
    { provide: LOCALE_ID, useValue: 'fr-FR' }
  ]
};
