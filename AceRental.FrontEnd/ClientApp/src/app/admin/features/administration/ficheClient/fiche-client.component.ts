import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from '../../../../services/client.service';
import { ClientDto } from '../../../../interfaces/client-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-fiche-client.component',
  imports: [MatFormFieldModule, 
    MatDatepickerModule,
    FormsModule, 
    MatInputModule,
    ReactiveFormsModule,CommonModule
    ],
  templateUrl: './fiche-client.component.html',
  styleUrl: './fiche-client.component.css',
})
export class FicheClientComponent implements OnInit {
  CiviliteH:boolean = true;
  id!: number;
  client!: ClientDto;
  clientForm!: FormGroup;

  constructor(route: ActivatedRoute, private fb: FormBuilder){
    this.id = Number(route.snapshot.paramMap.get('id'));
  }
  ngOnInit(): void {
    this.client  = ClientService.getClientById(this.id);
    this.clientForm = this.fb.group({
      email: [this.client.email, [Validators.required, Validators.email]],
      password: [this.client.password, Validators.required],
      civilite: [this.client.civilite, Validators.required],
      nomClient: [this.client.nomClient, Validators.required],
      prenomClient: [this.client.prenomClient, Validators.required],
      raisonSociale: [this.client.raisonSociale],
      tel: [this.client.tel, Validators.pattern(/^(0[1-9]\d{8}|\+33[1-9]\d{8})$/)],
      portable: [this.client.portable, Validators.pattern(/^(0[67]\d{8}|\+33[67]\d{8})$/)],
      adresse: [this.client.adresse],
      complementAdresse: [this.client.complementAdresse],
      codepostale: [this.client.codepostale, [Validators.pattern(/^\d{5}$/)]],
      ville: [this.client.ville]
    });
  }
  submit(): void {
    if (this.clientForm.valid) {
      console.log('clientForm.value', this.clientForm.value);
    }
  }
  reset(): void {
    this.clientForm.reset(this.client);
  }
  hasError(controlName: string, error: string): boolean {
    const control = this.clientForm.get(controlName);
    return !!(
      control &&
      control.hasError(error) &&
      (control.dirty || control.touched)
    );
  }
}
