import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { ProduitService } from '../../../../services/produit.service';
import { CommonModule } from '@angular/common';
import { ProduitDto } from '../../../../interfaces/produit-dto';
import { MatIcon, MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-fiche-produit.component',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatIconModule,
  ],
  templateUrl: './fiche-produit.component.html',
  styleUrl: './fiche-produit.component.css',
})
export class FicheProduitComponent {
  CiviliteH:boolean = true;
  id!: number;
  produit!: ProduitDto;
  produitForm!: FormGroup;
  fileName?:string;
  readonly maxSize = 104857600;
  file_store!: any;
  file_list: Array<string> = [];
display: FormControl = new FormControl("", Validators.required);
  constructor(
    route: ActivatedRoute, 
    private fb: FormBuilder,
    private produitService: ProduitService
  ){
    this.id = Number(route.snapshot.paramMap.get('id'));
  }
  ngOnInit(): void {
   this.produitService.getById(this.id).subscribe(res => {
      this.produit = res;
      this.initForm();
    });
    
  }

  initForm(): void {
    this.produitForm = this.fb.group({
      reference: [this.produit.reference, [Validators.required]],
      libelle: [this.produit.libelle, Validators.required],
      description: [this.produit.description],
      presentation: [this.produit.presentation],
      caracteristiques: [this.produit.caracteristiques],
      prix: [this.produit.prix,[Validators.required, Validators.min(1)]],
      images: [this.produit.images || [], [Validators.required]]
    });
  }
  
  submit(): void {
    if (this.produitForm.valid) {
      console.log('produitForm.value', this.produitForm.value);
      this.produitService.insertOrUpdate(this.produitForm.value).subscribe(res => {
        console.log(res.url);
      });
    }
  }
  reset(): void {
    this.produitForm.reset(this.produit);
  }
  hasError(controlName: string, error: string): boolean {
    const control = this.produitForm.get(controlName);
    return !!(
      control &&
      control.hasError(error) &&
      (control.dirty || control.touched)
    );
  }
  handleFileInputChange(event : any): void {
    this.produit.images = [];
    // this.produitForm.patchValue({images: []});
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
        this.produit.images.push(base64);
        this.produitForm.patchValue({
          images: [...this.produitForm.value.images, base64]
        });
        console.log("this.produitForm.value.images", this.produitForm.value.images);
      };
    });
    let input = event.target as HTMLInputElement;
    input.blur();
  }
 
}
