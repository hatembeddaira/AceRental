import { FinancialStatus } from "../Enum/financial-status";
import { LogisticStatus } from "../Enum/logistic-status";
import { Workflow } from "../Enum/workflow";
import { ClientReservationDto } from "./client-reservation-dto";
import { ReservationEquipmentsDto } from "./reservation-equipments-dto";
import { ReservationPacksDto } from "./reservation-packs-dto";
import { ReservationServicesDto } from "./reservation-services-dto";

export interface ReservationDetailsDto {
    Id: string;
    ReservationNumber: number;
    StartDate: Date;
    EndDate: Date;
    FinancialStatus: FinancialStatus;
    LogisticStatus: LogisticStatus;
    Workflow: Workflow;
    TotalHT: number;
    TVA: number;
    TotalTTC: number;
    ClientId: string;
    Client: ClientReservationDto;
    CurrentVersion: number;
    Equipments: ReservationEquipmentsDto[];
    Packs: ReservationPacksDto[];
    Services: ReservationServicesDto[];
    // Invoices: List<InvoiceDto>;
    // Quotes: List<QuoteDto>;
    // Payments: List<PaymentDto>;

    CreatedAt: Date;
    UpdatedAt: Date;
}