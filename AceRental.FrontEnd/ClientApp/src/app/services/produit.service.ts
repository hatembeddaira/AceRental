import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProduitDto } from '../interfaces/produit-dto';
import { Observable, of } from 'rxjs';

const ELEMENT_DATA: ProduitDto[] = [
{
  id: 1,
  reference: "DSR115",
  libelle : "Yamaha DSR115",
  description : "Enceinte de sonorisation active 1300 Watts HP 1'', une qualité sonore remarquable, idéale pour les professionnels du son",
  presentation:"La Yamaha DSR115 est une enceinte 2 voies compacte amplifie de haute puissance, quipe d'un boomer de 38 cm. quipe d'un DSP hautes performances travaillant en 48 bits et d'amplificateurs numriques (Classe D) haut de gamme d'une puissante totale de 1300 W (850 W pour le boomer, 450 W pour le tweeter), elle assure une restitution sonore d'un degr de rsolution sans concurrence dans sa catgorie, et de superbes performances en termes de niveau de pression sonore. Elle peut gnrer un niveau maximal de pression sonore de 136 dB SPL crte. La DSR115 convient parfaitement pour les applications de sonorisation et d'installation o on demande un grave 'punchy' et profond ainsi qu'une rponse prcise dans les aigus.",
  caracteristiques:["Enceinte de Sonorisation amplifiée Yamaha DSR 115", "Son design vous permet d'utiliser la DSR 115 en retour de scène", "Hp basses 15''", "Moteur d'aiguës 2''"],
  prix : 300,
  images: [],
}, 
{
  id: 2,
  reference: "DSR112",
  libelle : "Yamaha DSR112",
  description : "Enceinte de sonorisation active 1300 Watts HP 1'', une qualité sonore remarquable, idéale pour les professionnels du son",
  presentation:"La Yamaha DSR112 est une enceinte 2 voies compacte amplifie de haute puissance, quipe d'un boomer de 32 cm. quipe d'un DSP hautes performances travaillant en 48 bits et d'amplificateurs numriques (Classe D) haut de gamme d'une puissante totale de 1300 W (850 W pour le boomer, 450 W pour le tweeter), elle assure une restitution sonore d'un degr de rsolution sans concurrence dans sa catgorie, et de superbes performances en termes de niveau de pression sonore. Elle peut gnrer un niveau maximal de pression sonore de 136 dB SPL crte. La DSR112 convient parfaitement pour les applications de sonorisation et d'installation o on demande un grave 'punchy' et profond ainsi qu'une rponse prcise dans les aigus.",
  caracteristiques:["Enceinte de Sonorisation amplifiée Yamaha DSR 112", "Son design vous permet d'utiliser la DSR 112 en retour de scène", "Hp basses 12''", "Moteur d'aiguës 2''"],
  prix : 300,
  images: [],
}
];

@Injectable({
  providedIn: 'root',
})
export class ProduitService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<ProduitDto[]>  
  {
    // return this.http.get<ProduitDto[]>('https://api.monsite.com/api/files/list');
    return of(ELEMENT_DATA);
  }
  
  getById(id : number): Observable<ProduitDto>
  {
    // return this.http.get<ProduitDto>(`https://api.monsite.com/api/files/list${id}`);
    const item =  ELEMENT_DATA.find(x => x.id === id) ;
    if (!item) {
      throw new Error("Élément non trouvé");
    }
    return of(item);
  }

  insertOrUpdate(produit : ProduitDto)
  {
    return this.http.post<{ url: string }>('https://api.monsite.com/api/files/upload', produit);
  }
  
//   uploadProduitFiles(files: File[]) {
//     const formData = new FormData();
//     files.forEach(file => {
//       formData.append('file', file);
//     })
//     return this.http.post<{ url: string }>('https://api.monsite.com/api/files/upload', formData);
//   }
}
