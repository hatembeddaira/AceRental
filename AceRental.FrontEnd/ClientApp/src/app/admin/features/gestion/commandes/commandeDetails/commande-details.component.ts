import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {provideNativeDateAdapter, MAT_DATE_LOCALE} from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommandesService } from '../../../../../services/commandes.service';
import { CommandeDto } from '../../../../../interfaces/commande-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-commande-details.component',
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'fr-FR'}, provideNativeDateAdapter()],
  imports: [
    MatFormFieldModule, 
    MatDatepickerModule,
    FormsModule, 
    MatInputModule,
    ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './commande-details.component.html',
  styleUrl: './commande-details.component.css',
})
export class CommandeDetailsComponent implements OnInit {
  commande! : CommandeDto ;
  // multiplePeriodeChecked : boolean = false;
  // multipleAdresseChecked : boolean = false;
  // livraisonInclusChecked : boolean = false;
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
    
    // console.log('ID du commande =', this.id);
    this.commande  = CommandesService.getCommandeById(this.id);
    // console.log(' commande =', this.commande );
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
    this.commande.livraison = (event.target as HTMLInputElement).checked;
    if(!this.commande.livraison)
    {
      this.commande.multipleLivraison = this.commande.livraison;
    }
    console.log(' livraisonInclusChecked =', this.commande.livraison );
  }
  onMultipleAdresseChecked(event:Event)
  {
    this.commande.multipleLivraison = (event.target as HTMLInputElement).checked;
    console.log(' multipleAdresseChecked =', this.commande.multipleLivraison );
  }
  // onMultiplePeriodeChecked(event:Event)
  // {
  //   this.multiplePeriodeChecked = (event.target as HTMLInputElement).checked;
  //   console.log(' multiplePeriodeChecked =', this.multiplePeriodeChecked );
  // }

  calculeMontantTotale()
  {
    const totalPacks = this.commande.packs.reduce((total, packCommande) => {
      return total + packCommande.pack.prix * packCommande.quantite;
    }, 0);
    const totalProduits = this.commande.produits.reduce((total, produitCommande) => {
      return total + produitCommande.produit.prix * produitCommande.quantite;
    }, 0);
    this.commande.totale = totalPacks;
  }
}
