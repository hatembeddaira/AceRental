import { EquipmentDetailsDto } from "./equipment-details-dto";
import { ReservationDetailsDto } from "./reservation-details-dto";

export interface ReservationEquipmentsDto {
    ReservationId?: string;
    EquipmentId: string;
    Quantity: number;
    UnitPriceAtTimeOfBooking: number;
    Reservation?: ReservationDetailsDto;
    Equipment?: EquipmentDetailsDto;
}
