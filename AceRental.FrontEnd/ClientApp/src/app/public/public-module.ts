import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PublicRoutingModule } from './public-routing-module';
import { FooterComponent } from './@theme/components/footer/footer.component';
import { HeaderComponent } from './@theme/components/header/header.component';
import { PromoComponent } from './@theme/components/promo/promo.component';
import {MatDatepickerModule } from '@angular/material/datepicker';
import {MatFormFieldModule} from '@angular/material/form-field';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [],
  imports: [
    RouterModule,
    CommonModule,
    PublicRoutingModule,
    PromoComponent,
    HeaderComponent,
    FooterComponent,
    MatFormFieldModule, MatDatepickerModule
  ]
})
export class PublicModule { }
