export interface EquipmentsDto {
    Id: string;
    Reference: string;
    Name : string;
    Description? : string;
    Presentation? : string;
    Caracteristiques? : string[];
    DailyPriceHT : number;
    PurchasePriceTTC : number;
    NewPurchasePriceTTC : number;
    TotalStock : number;
    Category : string;
    CreatedAt: Date;
    CreatedBy: string;
    images: string[];
}
