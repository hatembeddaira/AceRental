import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing-module';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AdminRoutingModule,   
  ]
})
export class AdminModule { }

// platformBrowserDynamic().bootstrapModule(AdminModule);