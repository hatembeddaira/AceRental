import { OptionComprisesDto } from "./option-comprises-dto";
import { EquipmentsDto } from "./equipment-dto";

export interface PackDto {
    id: number;
    reference: number;
    libelle : string;
    description : string;
    prix : number;
    image: string;
    produits : EquipmentsDto[];
    optionComprises : OptionComprisesDto[];
}
