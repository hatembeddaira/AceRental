import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { filter, map, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ReservationDetailsDto } from '../interfaces/reservation-details-dto';


@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Reservations';

  get(): Observable<ReservationDetailsDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: ReservationDetailsDto[] }>(`${this.apiUrl}`, {
            params: new HttpParams()
                .set('$expand', 'Client')
                .set('$orderby', 'ReservationNumber desc')
        }).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<ReservationDetailsDto | null> {
    
    return this.http
      .get<ReservationDetailsDto | { '@odata.context'?: string; value: ReservationDetailsDto }>(`${this.apiUrl}(${id})`, {
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
          return response as ReservationDetailsDto;
        })
      );
  }
  patch(reservationId: string, reservation: ReservationDetailsDto): Observable<boolean> {
    return this.http
      .patch<boolean>(`${this.apiUrl}(${reservationId})`, reservation)
      .pipe(
        map(response => response || false)
      );
  }
  post(reservation: ReservationDetailsDto): Observable<ReservationDetailsDto> {
    return this.http
      .post<ReservationDetailsDto>(`${this.apiUrl}`, reservation)
      .pipe(
        map(response => response)
      );
  }
}
