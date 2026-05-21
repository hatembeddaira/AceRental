import { Injectable } from '@angular/core';
import { CommandeDto } from '../interfaces/commande-dto';

const ELEMENT_DATA: CommandeDto[] = [
  {
    id: 1, 
    reference : 'CMD202501001',
    nomClient: 'Tarek', 
    prenomClient: 'Beddaira', 
    raisonSociale:'Ace Sound', 
    dateCreation : '01/01/2025', 
    derniereModification : '01/01/2025', 
    totale : 5500.35, 
    totaleExpedition : 500,
    etat: 'Expédiée', 
    livraison:true, 
    multipleLivraison : true,
    adresseLivraison:'5 rue max linder, 91700,  Saint genviève des boix',
    dateDebutReservation : '',
    dateFinReservation : '',
    packs : [
      {
        pack: {
          id: 1,
          reference: 20,
          libelle: 'Pack DO',
          description: 'Description Pack DO',
          prix: 500,
          image: '/assets/img/PackDo.jpg',
          produits: [],
          optionComprises: []
        },
        quantite : 3,
        fraisExpedition : 150,
        adresseLivraison : '6 rue max linder, 91700,  Saint genviève des boix',
        dateDebutReservation : '01/01/2025',
        dateFinReservation : '02/01/2025'
      },
      {
        pack: {
          id: 2,
          reference: 30,
          libelle: 'Pack Sol',
          description: 'Description Pack Sol',
          prix: 400,
          image: '/assets/img/PackSol.jpg',
          produits: [],
          optionComprises: []
        },
        quantite : 2,
        fraisExpedition : 350,
        adresseLivraison : '4 rue max linder, 91700,  Saint genviève des boix',
        dateDebutReservation : '01/01/2025',
        dateFinReservation : '03/01/2025'
      }
    ],
    produits : [
      
    ]
  }, 
  {
    id: 2, 
    reference : 'CMD202501002',
    nomClient: 'hatem', 
    prenomClient: 'Daira', 
    raisonSociale:'Ace Sound', 
    dateCreation : '01/01/2025', 
    derniereModification : '01/01/2025', 
    totale : 5500.35, 
    totaleExpedition : 0,
    etat: 'Expédiée', 
    livraison:false, 
    multipleLivraison : false,
    adresseLivraison:'',
    dateDebutReservation : '01/01/2025',
    dateFinReservation : '02/01/2025',
    packs : [],
    produits : []
  },
  {
    id: 3, 
    reference : 'CMD202501003',
    nomClient: 'Cyrine', 
    prenomClient: 'Daira', 
    raisonSociale:'Ace Sound', 
    dateCreation : '01/01/2025', 
    derniereModification : '01/01/2025', 
    totale : 5500.35, 
    totaleExpedition : 0,
    etat: 'En Attente', 
    livraison:true, 
    multipleLivraison : false,
    adresseLivraison:'6 rue max linder, 91700,  Saint genviève des boix',
    dateDebutReservation : '02/01/2025',
    dateFinReservation : '03/01/2025',
    packs : [],
    produits : []
  }
];

@Injectable({
  providedIn: 'root',
})
export class CommandesService {
  static getAllCommandes(): CommandeDto[]  {
    return ELEMENT_DATA;
  }
  
  static getCommandeById(id : number): CommandeDto
  {
    const item =  ELEMENT_DATA.find(x => x.id === id) ;
    if (!item) {
      throw new Error("Élément non trouvé");
    }
    return item;
  }
}
