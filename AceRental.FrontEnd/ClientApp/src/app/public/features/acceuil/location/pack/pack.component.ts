import { Component, OnInit  } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatDatepickerModule } from "@angular/material/datepicker";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import { JsonPipe } from '@angular/common';
import {provideNativeDateAdapter, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatTabsModule} from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatBadgeModule} from '@angular/material/badge';
@Component({
  selector: 'app-pack.component',
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'fr-FR'}, provideNativeDateAdapter()],
  imports: [
    MatFormFieldModule, 
    MatDatepickerModule, 
    FormsModule, 
    ReactiveFormsModule, 
    RouterModule,
    // JsonPipe, 
    MatInputModule,
    MatTabsModule,MatBadgeModule, MatButtonModule, MatIconModule],
  templateUrl: './pack.component.html',
  styleUrl: './pack.component.css',
})
export class PackComponent implements OnInit{  
  id!: number;
  num = new FormControl(1);
  readonly range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });
  constructor(route: ActivatedRoute){
    this.id = Number(route.snapshot.paramMap.get('id'));
  }
  
  ngOnInit( ): void {
    
    // console.log('ID du pack =', this.id);
  }

  readonly adresse = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });
}
