import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { ClientDto } from '../interfaces/client-dto';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root',
})
export class ClientService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.aceRentalApiUrl + '/v1/Clients';

  get(): Observable<ClientDto[]> {
    return this.http.get<{ '@odata.context'?: string; value: ClientDto[] }>(this.apiUrl).pipe(
      map(response => response.value || []),
      tap(() => null)
    );
  }
  getById(id: string): Observable<ClientDto | null> {
    
    return this.http
      .get<ClientDto | { '@odata.context'?: string; value: ClientDto }>(`${this.apiUrl}(${id})`)
      .pipe(
        map(response => {
          if (!response) {
            return null;
          }
          // if ('value' in response) {
          //   return response.value || null;
          // }
          return response as ClientDto;
        })
      );
  }
}
