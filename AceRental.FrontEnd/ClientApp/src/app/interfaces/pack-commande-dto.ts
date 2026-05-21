import { PackDto } from "./pack-dto";

export interface PackCommandeDto {
    dateDebutReservation: string;
    dateFinReservation: string;
    adresseLivraison: string;
    quantite : number;
    fraisExpedition: number;
    pack : PackDto
}
