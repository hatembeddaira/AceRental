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

@Component({
  selector: 'app-reservation.component',
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'fr-FR'}, provideNativeDateAdapter()],
  imports: [
    MatFormFieldModule, 
    MatDatepickerModule,
    FormsModule, 
    MatInputModule,
    ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css',
})
export class ReservationComponent implements OnInit {
  id!: string;
  reservation = signal<any>(null);
  reservationService: ReservationService
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
    this.id = route.snapshot.paramMap.get('id')|| '';
    this.reservationService = _reservationService;
  }
    
  ngOnInit( ): void {
    
    // console.log('ID du Reservation =', this.id);
    this.reservationService.getById(this.id).subscribe({
      next: obj => {
        if (!obj) {
          throw new Error('Reservation introuvable.');
        }

        this.reservation.set(obj);
        // this.cdRef.detectChanges();
        console.log('Response from API:', obj);
      },
      error: err => console.error(err)
    });
    // console.log(' Reservation =', this.Reservation );
  }
  plus(item: any) {
    item.quantite = Number(item.quantite) + 1;
    this.calculeMontantTotale();
  }
  moins(item: any) {
    item.quantite = Math.max(1, Number(item.quantite) - 1);
    this.calculeMontantTotale();
  }
  onLivraisonInclusChecked(event:Event)
  {
    // this.reservation.livraison = (event.target as HTMLInputElement).checked;
    // if(!this.reservation.livraison)
    // {
    //   this.reservation.multipleLivraison = this.reservation.livraison;
    // }
    // console.log(' livraisonInclusChecked =', this.reservation.livraison );
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
