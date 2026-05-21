import { Component, OnInit } from '@angular/core';
import { ProduitService } from '../../../../services/produit.service';

@Component({
  selector: 'app-produits',
  imports: [],
  templateUrl: './produits.component.html',
  styleUrl: './produits.component.css',
})
export class ProduitsComponent implements OnInit {
constructor(private produitService: ProduitService){}
  ngOnInit(): void {
    this.produitService.getAll().subscribe(res => {
        console.log(res);
      });
  }
}
