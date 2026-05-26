import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { EquipmentService } from '../../../../services/equipment.service';
import { CommonModule } from '@angular/common';
import { EquipmentsDto } from '../../../../interfaces/equipment-dto';
import { MatIcon, MatIconModule } from "@angular/material/icon";
import e from 'express';
import { EquipmentCategory } from '../../../../Enum/equipment-category';

@Component({
  selector: 'app-equipment.component',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatIconModule,
    MatSelectModule
  ],
  templateUrl: './equipment.component.html',
  styleUrl: './equipment.component.css',
})
export class EquipmentComponent {
  id!: string;
  equipment!: EquipmentsDto;
  equipmentForm!: FormGroup;
  equipmentService: EquipmentService
  fileName?:string;
  categoryList: string[] = Object.values(EquipmentCategory).filter(v => typeof v === 'string') as string[];
  readonly maxSize = 104857600;
  file_store!: any;
  file_list: Array<string> = [];
  display: FormControl = new FormControl("", Validators.required);
  constructor(
    route: ActivatedRoute,
    private fb: FormBuilder,
    private _equipmentService: EquipmentService
  ){
    this.id = route.snapshot.paramMap.get('id')|| '';
    this.equipmentService = _equipmentService;
    this.equipmentForm = this.fb.group({
      reference: [''],
      name: ['', [Validators.required, Validators.email]],
      description: ['', Validators.required],
      presentation: ['', Validators.required],
      caracteristiques: ['', Validators.required],
      dailyPriceHT: [0, Validators.required],
      purchasePriceTTC: [0, Validators.required],
      newPurchasePriceTTC: [0, Validators.required],
      totalStock: [0, Validators.required],
      category: ['', Validators.required],
      images: [[]]
    });
  }
  ngOnInit(): void {
    if (!this.id) {
      throw new Error('Identifiant equippement manquant.');
    }
    this.equipmentService.getById(this.id).subscribe({
      next: e => {
        if (!e) {
          throw new Error('Equippement introuvable.');
        }

        this.equipment = e;
        this.equipment.images = this.equipment.images || [];
        console.log('Response from API:', this.equipment);
        this.patchValue(this.equipment);
      },
      error: err => console.error(err)
    });
  }

  patchValue(equipment: EquipmentsDto): void {
    this.equipmentForm.patchValue({
      reference: equipment.Reference,
      name: equipment.Name,
      description: equipment.Description,
      presentation: 'test presentation',
      caracteristiques: 'test caracteristiques',
      dailyPriceHT: equipment.DailyPriceHT,
      purchasePriceTTC: equipment.PurchasePriceTTC,
      newPurchasePriceTTC: equipment.NewPurchasePriceTTC,
      totalStock: equipment.TotalStock,
      category: equipment.Category,
      images: equipment.images || []
    });
  }

  submit(): void {
      console.log('equipmentForm.value', this.equipmentForm.value);
    if (this.equipmentForm.valid) {
      this.equipmentService.insertOrUpdate(this.equipmentForm.value).subscribe(res => {
        console.log(res.url);
      });
    }
  }
  reset(): void {
    this.patchValue(this.equipment);
  }
  hasError(controlName: string, error: string): boolean {
    const control = this.equipmentForm.get(controlName);
    return !!(
      control &&
      control.hasError(error) &&
      (control.dirty || control.touched)
    );
  }
  handleFileInputChange(event : any): void {
    this.equipment.images = [];
    const files = event.target.files as FileList;
    if (!files || files.length === 0) return;
    Array.from(files).forEach(file => {
      if (!file.type.startsWith('image/')) {
          console.log("Only images are supported.");
          return;
      }
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        const base64 = reader.result as string;
        this.equipment.images.push(base64);
        console.log("this.equipment.images", this.equipment.images);
        this.equipmentForm.patchValue({
          images: [...this.equipmentForm.value.images, base64]
        });
        console.log("this.equipmentForm.value.images", this.equipmentForm.value.images);
      };
    });
    let input = event.target as HTMLInputElement;
    input.blur();
  }

}
