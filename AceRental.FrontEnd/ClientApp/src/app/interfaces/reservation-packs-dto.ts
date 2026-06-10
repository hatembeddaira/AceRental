import { PackDetailsDto } from "./pack-details-dto";
import { ReservationDetailsDto } from "./reservation-details-dto";

export interface ReservationPacksDto {
    ReservationId?: string;
    PackId: string;
    Quantity: number;
    UnitPriceAtTimeOfBooking: number;
    Reservation?: ReservationDetailsDto;
    Pack?: PackDetailsDto;
}
