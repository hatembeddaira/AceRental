import { OptionComprisesDto } from "./option-comprises-dto";
import { ProduitDto } from "./produit-dto";

export interface PackDto {
    id: number;
    reference: number;
    libelle : string;
    description : string;
    prix : number;
    image: string;
    produits : ProduitDto[];
    optionComprises : OptionComprisesDto[];
}
