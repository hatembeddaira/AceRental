import { ServiceType } from "../Enum/service-type";

export interface ServiceDto {
    Id: string;
    Reference: string;
    Name: string;
    Type: ServiceType;
    PriceHT: number;
    IsDailyPrice: boolean;
}
