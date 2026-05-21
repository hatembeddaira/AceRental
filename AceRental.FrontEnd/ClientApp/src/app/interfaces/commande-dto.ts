import { PackCommandeDto } from "./pack-commande-dto";
import { ProduitCommandeDto } from "./produit-commande-dto";
import { EquipmentsDto } from "./produit-dto";

export interface CommandeDto {
    id: number;
    reference: string;
    nomClient: string;
    prenomClient: string;
    raisonSociale: string;
    dateCreation: string;
    dateDebutReservation: string;
    dateFinReservation: string;
    derniereModification: string;
    etat: string;
    totale: number;
    totaleExpedition: number;    
    livraison: boolean;
    multipleLivraison: boolean;
    adresseLivraison: string;
    produits : ProduitCommandeDto[];
    packs : PackCommandeDto[];
}
