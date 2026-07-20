import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { ClientDto } from '../../../interfaces/client-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client',
  // standalone: true,
  imports: [MatFormFieldModule, 
    MatDatepickerModule,
    FormsModule, 
    MatInputModule,
    ReactiveFormsModule,CommonModule
    ],
  templateUrl: './client.component.html',
  styleUrl: './client.component.css',
})
export class ClientComponent implements OnInit {
  CiviliteH:boolean = true;
  id!: string;
  client = signal<any>(null);
  clientForm!: FormGroup;
  clientService: ClientService;
  constructor(route: ActivatedRoute, private fb: FormBuilder,private _clientService: ClientService){
    this.id = route.snapshot.paramMap.get('id') || '';
    this.clientService = _clientService;
    // Initialize form controls before async data arrives.
    this.clientForm = this.fb.group({
      id: [''],
      clientNumber: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      civilite: ['H', Validators.required],
      nomClient: ['', Validators.required],
      prenomClient: ['', Validators.required],
      raisonSociale: [''],
      tel: ['', Validators.pattern(/^(0[1-9]\d{8}|\+33[1-9]\d{8})$/)],
      portable: ['', Validators.pattern(/^(0[67]\d{8}|\+33[67]\d{8})$/)],
      adresse: [''],
      complementAdresse: [''],
      codepostale: ['', [Validators.pattern(/^\d{5}$/)]],
      ville: ['']
    });
  }
  ngOnInit(): void {
    if (!this.id) {
      throw new Error('Identifiant client manquant.');
    }

    this.clientService.getById(this.id).subscribe({
      next: client => {
        if (!client) {
          throw new Error('Client introuvable.');
        }

        console.log('Response from API:', client);
        this.client.set(client);
        this.patchValue(this.client());
      },
      error: err => console.error(err)
    });
  }
  patchValue(client: ClientDto): void {
    this.clientForm.patchValue({
          id: client.Id,
          clientNumber: client.ClientNumber,
          email: client.Email,
          password: '',
          civilite: 'H',
          nomClient: client.LastName,
          prenomClient: client.FirstName,
          raisonSociale: client.RaisonSociale,
          tel: client.TelNumber,
          portable: client.PhoneNumber,
          adresse: client.Address,
          complementAdresse: client.ComplementAdresse,
          codepostale: client.PostalCode,
          ville: client.City
        });
  }
  submit(): void {
    if (this.clientForm.valid) {
      console.log('clientForm.value', this.clientForm.value);
    }
  }
  reset(): void {
    this.patchValue(this.client());
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
