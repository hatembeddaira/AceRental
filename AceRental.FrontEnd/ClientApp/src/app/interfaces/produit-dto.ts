export interface ProduitDto {
    id: number;
    reference: string;
    libelle : string;
    description? : string;
    presentation? : string;
    caracteristiques? : string[];
    prix : number;
    images: string[];
}
