export interface EquipmentsDto {
    Id: number;
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
