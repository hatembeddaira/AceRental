import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {provideNativeDateAdapter, MAT_DATE_LOCALE} from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ReservationService } from '../../../../services/reservation.service';
import { CommonModule } from '@angular/common';
import { ReservationDetailsDto } from '../../../../interfaces/reservation-details-dto';
import { ServiceDto } from '../../../../interfaces/service-dto';
import { ServiceType } from '../../../../Enum/service-type';
import { ReservationServicesDto } from '../../../../interfaces/reservation-services-dto';
import { ClientDto } from '../../../../interfaces/client-dto';

@Component({
  selector: 'app-reservation.component',
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'fr-FR'}, provideNativeDateAdapter()],
  imports: [
    MatFormFieldModule, 
    MatDatepickerModule,
    FormsModule, 
    MatInputModule,
    ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css',
})
export class BasketComponent implements OnInit {
  id!: string;
  basket = signal<any>(null);
  reservationService: ReservationService
  transportInclus : boolean = false;
  // multiplePeriodeChecked : boolean = false;
  // multipleAdresseChecked : boolean = false;
  // livraisonInclusChecked : boolean = false;
  num = new FormControl(1);
  readonly range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });
  constructor(
    private cdRef: ChangeDetectorRef,
    route: ActivatedRoute,
    private _reservationService: ReservationService){
    this.id = route.snapshot.paramMap.get('id') || 'create';
    this.reservationService = _reservationService;
  }
    
  ngOnInit( ): void {
    if(!this.id || this.id === 'create')
    {
      let client :  ClientDto ={        
          ClientNumber: '',
          RaisonSociale: 'test',
          FirstName: 'hatem',
          LastName: 'beddaira',
          Email: '',
          Password: '',
          Civilite: 'H',
          TelNumber: '',
          PhoneNumber: '',
          Address: '',
          ComplementAdresse: '',
          PostalCode: 0,
          City: '',
          CreateAt: new Date()
         };
      let res : ReservationDetailsDto = {
        Id: '',
        ReservationNumber: 0,
        StartDate: new Date(),
        EndDate: new Date(),
        FinancialStatus: 0,
        LogisticStatus: 0,
        Workflow: 0,
        TotalHT: 0,
        TVA: 0,
        TotalTTC: 0,
        ClientId: '',
        Client: client,
         CurrentVersion : 1,
         Equipments : [],
         Packs : [],
         Services : [],
         CreatedAt : new Date(),
         UpdatedAt : new Date()
      }
      this.basket.set(res);
    }
    else
    {
      this.reservationService.getById(this.id).subscribe({
        next: obj => {
          if (!obj) {
            throw new Error('Panier vide.');
          }

          this.basket.set(obj); 
          this.transportInclus = obj?.Services.some((s: ReservationServicesDto) => s.Service!.Type.toString() === ServiceType[ServiceType.Transport]) || false;
          console.log('Response from API:', obj);
        },
        error: err => console.error(err)
      });
    }
  }
  plus(item: any) {
    item.Quantity = Number(item.Quantity) + 1;
    this.calculeMontantTotale();
  }
  moins(item: any) {
    item.Quantity = Math.max(1, Number(item.Quantity) - 1);
    this.calculeMontantTotale();
  }
  onLivraisonInclusChecked(event:Event)
  {
    this.transportInclus = (event.target as HTMLInputElement).checked;
    
    console.log(' livraisonInclusChecked =', this.transportInclus );
  }
  // onMultipleAdresseChecked(event:Event)
  // {
  //   this.reservation.multipleLivraison = (event.target as HTMLInputElement).checked;
  //   console.log(' multipleAdresseChecked =', this.reservation.multipleLivraison );
  // }
  // // onMultiplePeriodeChecked(event:Event)
  // {
  //   this.multiplePeriodeChecked = (event.target as HTMLInputElement).checked;
  //   console.log(' multiplePeriodeChecked =', this.multiplePeriodeChecked );
  // }

  calculeMontantTotale()
  {
    // const totalPacks = this.reservation.Packs.reduce((total, packReservation) => {
    //   return total + packReservation.pack.prix * packReservation.quantite;
    // }, 0);
    // const totalProduits = this.reservation.Equipments.reduce((total, produitReservation) => {
    //   return total + produitReservation.produit.DailyPriceHT * produitReservation.quantite;
    // }, 0);
    // this.reservation.totale = totalPacks + totalProduits;
  }
}
