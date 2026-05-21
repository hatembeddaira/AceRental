import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { ClientDto } from '../interfaces/client-dto';

const ELEMENT_DATA: ClientDto[] = [
{
  id: 1, 
  raisonSociale: "Ace Sound",
  nomClient :  "Daira",
  prenomClient :  "Hatem",
  email :  "hatem.daira@gmail.com",
  password:  "12345",
  civilite:  "H",
  tel:  undefined,
  portable:  "0751340180",
  adresse:  "6 rue max linder",
  complementAdresse:  undefined,
  codepostale: 91700,
  ville: "saint genviève des bois",
  dateCreation: new Date('10/20/2025')
  }, 
  {
  id: 2, 
  raisonSociale: "Ace Sound",
  nomClient :  "Daira",
  prenomClient :  "Syrine",
  email :  "Syrine.daira@gmail.com",
  password:  "12345",
  civilite:  "F",
  tel:  undefined,
  portable:  "0751340180",
  adresse:  "6 rue max linder",
  complementAdresse:  undefined,
  codepostale: 91700,
  ville:  "saint genviève des bois",
  dateCreation: new Date('10/20/2025')
  }, 
  {
  id: 3, 
  raisonSociale: undefined,
  nomClient :  "Beddaira",
  prenomClient :  "Tarek",
  email :  "Tarek.beddaira@gmail.com",
  password:  "12345",
  civilite:  "H",
  tel:  undefined,
  portable:  "0751340180",
  adresse:  "6 rue max linder",
  complementAdresse:  undefined,
  codepostale: 91700,
  ville:  "saint genviève des bois",
  dateCreation: new Date('10/20/2025')
  }, 
];

@Injectable({
  providedIn: 'root',
})
export class ClientService {

  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  static getAllClients(): ClientDto[]  
  {
  return ELEMENT_DATA;
  }

  
  static getClientById(id : number): ClientDto
  {
  const item =  ELEMENT_DATA.find(x => x.id === id) ;
  if (!item) {
    throw new Error("Élément non trouvé");
  }
  return item;
  }
}
