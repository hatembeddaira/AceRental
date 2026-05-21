import { Component, OnInit } from '@angular/core';
import { ProduitService } from '../../../../services/produit.service';

@Component({
  selector: 'app-produits',
  imports: [],
  templateUrl: './produits.component.html',
  styleUrl: './produits.component.css',
})
export class ProduitsComponent implements OnInit {
  equipmentService: ProduitService;
constructor(private _equipmentService: ProduitService){
  this.equipmentService = _equipmentService;
}
  ngOnInit(): void {
    this.equipmentService.get().subscribe({
      next: equipments => {
        // this.dataSource.data = equipments;
        console.log(equipments);
      },
      error: err => console.error(err)
    });
  }
}
