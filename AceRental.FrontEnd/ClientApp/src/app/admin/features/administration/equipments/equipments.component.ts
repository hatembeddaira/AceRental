import { Component, OnInit } from '@angular/core';
import { EquipmentService } from '../../../../services/equipment.service';

@Component({
  selector: 'app-equipments',
  imports: [],
  templateUrl: './equipments.component.html',
  styleUrl: './equipments.component.css',
})
export class EquipmentsComponent implements OnInit {
  equipmentService: EquipmentService;
constructor(private _equipmentService: EquipmentService){
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
