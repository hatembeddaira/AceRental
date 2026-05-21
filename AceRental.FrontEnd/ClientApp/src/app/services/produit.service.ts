import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { EquipmentsDto } from '../interfaces/produit-dto';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root',
})
export class ProduitService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Equipments';

  get(): Observable<EquipmentsDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: EquipmentsDto[] }>(this.apiUrl).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<EquipmentsDto | null> {
    
    return this.http
      .get<EquipmentsDto | { '@odata.context'?: string; value: EquipmentsDto }>(`${this.apiUrl}(${id})`)
      .pipe(
        map(response => {
          if (!response) {
            return null;
          }
          // if ('value' in response) {
          //   return response.value || null;
          // }
          return response as EquipmentsDto;
        })
      );
  }

 

  insertOrUpdate(produit : EquipmentsDto)
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
