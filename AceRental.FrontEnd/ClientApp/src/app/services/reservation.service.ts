import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ReservationDetailsDto } from '../interfaces/reservation-details-dto';


@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Reservations';

  get(): Observable<ReservationDetailsDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: ReservationDetailsDto[] }>(`${this.apiUrl}?$expand=Client`).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<ReservationDetailsDto | null> {
    
    return this.http
      .get<ReservationDetailsDto | { '@odata.context'?: string; value: ReservationDetailsDto }>(`${this.apiUrl}(${id})?$expand=Client,Payments,Quotes,Invoices,Services,Packs,Equipments`)
      .pipe(
        map(response => {
          if (!response) {
            return null;
          }
          // if ('value' in response) {
          //   return response.value || null;
          // }
          return response as ReservationDetailsDto;
        })
      );
  }
}
