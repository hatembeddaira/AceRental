import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { filter, map, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ServiceDto } from '../interfaces/service-dto';

@Injectable({
  providedIn: 'root',
})
export class ServiceService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Services';

  get(): Observable<ServiceDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: ServiceDto[] }>(`${this.apiUrl}`).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<ServiceDto | null> {
    
    return this.http
      .get<ServiceDto | { '@odata.context'?: string; value: ServiceDto }>(`${this.apiUrl}(${id})`, {
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
          return response as ServiceDto;
        })
      );
  }
}
