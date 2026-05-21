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
      {
        produit: {
          id: 2,
          reference: "DSR112",
          libelle : "Yamaha DSR112",
          description : "Enceinte de sonorisation active 1300 Watts HP 1'', une qualité sonore remarquable, idéale pour les professionnels du son",
          presentation:"La Yamaha DSR112 est une enceinte 2 voies compacte amplifie de haute puissance, quipe d'un boomer de 32 cm. quipe d'un DSP hautes performances travaillant en 48 bits et d'amplificateurs numriques (Classe D) haut de gamme d'une puissante totale de 1300 W (850 W pour le boomer, 450 W pour le tweeter), elle assure une restitution sonore d'un degr de rsolution sans concurrence dans sa catgorie, et de superbes performances en termes de niveau de pression sonore. Elle peut gnrer un niveau maximal de pression sonore de 136 dB SPL crte. La DSR112 convient parfaitement pour les applications de sonorisation et d'installation o on demande un grave 'punchy' et profond ainsi qu'une rponse prcise dans les aigus.",
          caracteristiques:["Enceinte de Sonorisation amplifiée Yamaha DSR 112", "Son design vous permet d'utiliser la DSR 112 en retour de scène", "Hp basses 12''", "Moteur d'aiguës 2''"],
          prix : 300,
          images: ['/assets/img/PackSol.jpg'],
        },
        quantite : 2,
        fraisExpedition : 150,
        adresseLivraison : '',
        dateDebutReservation : '',
        dateFinReservation : ''
      },
      {
        produit: {
          id: 1,
            reference: "DSR115",
            libelle : "Yamaha DSR115",
            description : "Enceinte de sonorisation active 1300 Watts HP 1'', une qualité sonore remarquable, idéale pour les professionnels du son",
            presentation:"La Yamaha DSR115 est une enceinte 2 voies compacte amplifie de haute puissance, quipe d'un boomer de 38 cm. quipe d'un DSP hautes performances travaillant en 48 bits et d'amplificateurs numriques (Classe D) haut de gamme d'une puissante totale de 1300 W (850 W pour le boomer, 450 W pour le tweeter), elle assure une restitution sonore d'un degr de rsolution sans concurrence dans sa catgorie, et de superbes performances en termes de niveau de pression sonore. Elle peut gnrer un niveau maximal de pression sonore de 136 dB SPL crte. La DSR115 convient parfaitement pour les applications de sonorisation et d'installation o on demande un grave 'punchy' et profond ainsi qu'une rponse prcise dans les aigus.",
            caracteristiques:["Enceinte de Sonorisation amplifiée Yamaha DSR 115", "Son design vous permet d'utiliser la DSR 115 en retour de scène", "Hp basses 15''", "Moteur d'aiguës 2''"],
            prix : 300,
          images: ['/assets/img/PackSol.jpg'],
        },
        quantite : 2,
        fraisExpedition : 200,
        adresseLivraison : '',
        dateDebutReservation : '',
        dateFinReservation : ''
      }
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
