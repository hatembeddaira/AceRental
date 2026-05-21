import { ProduitDto } from "./produit-dto";

export interface ProduitCommandeDto {
    dateDebutReservation: string;
    dateFinReservation: string;
    adresseLivraison: string;
    quantite : number;
    fraisExpedition: number;
    produit : ProduitDto
}
