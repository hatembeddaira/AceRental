import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { filter, map, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { PackDetailsDto } from '../interfaces/pack-details-dto';

@Injectable({
  providedIn: 'root',
})
export class PackService {
private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Packs';

  get(): Observable<PackDetailsDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: PackDetailsDto[] }>(`${this.apiUrl}`).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<PackDetailsDto | null> {
    
    return this.http
      .get<PackDetailsDto | { '@odata.context'?: string; value: PackDetailsDto }>(`${this.apiUrl}(${id})`, {
            params: new HttpParams()
                .set('$expand', 'Client,Payments,Quotes,Invoices,Services($expand=Service),Packs($expand=Pack),Equipments($expand=Equipment)')
        })
      .pipe(
        map(response => {
          if (!response) {
            return null;
          }
          // if ('value' in response) {
          //   return response.value || null;
          // }
          return response as PackDetailsDto;
        })
      );
  }
}
