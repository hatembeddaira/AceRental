import { ReservationDetailsDto } from "./reservation-details-dto";
import { ServiceDto } from "./service-dto";

export interface ReservationServicesDto {
    ReservationId?: string;
    ServiceId: string;
    Quantity: number;
    UnitPriceAtTimeOfBooking: number;
    Reservation?: ReservationDetailsDto;
    Service?: ServiceDto;
}
