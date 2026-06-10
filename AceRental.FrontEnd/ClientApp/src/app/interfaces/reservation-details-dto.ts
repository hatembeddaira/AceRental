import { FinancialStatus } from "../Enum/financial-status";
import { LogisticStatus } from "../Enum/logistic-status";
import { Workflow } from "../Enum/workflow";
import { ClientDto } from "./client-dto";
import { ReservationEquipmentsDto } from "./reservation-equipments-dto";
import { ReservationPacksDto } from "./reservation-packs-dto";
import { ReservationServicesDto } from "./reservation-services-dto";

export interface ReservationDetailsDto {
    Id?: string;
    ReservationNumber?: number;
    RaisonSociale?: string;
    StartDate: Date;
    EndDate: Date;
    FinancialStatus?: FinancialStatus;
    LogisticStatus?: LogisticStatus;
    Workflow: Workflow;
    TotalHT?: number;
    TVA?: number;
    TotalTTC?: number;
    ClientId: string;
    Client?: ClientDto;
    CurrentVersion?: number;
    Equipments: ReservationEquipmentsDto[];
    Packs: ReservationPacksDto[];
    Services: ReservationServicesDto[];
    // Invoices: List<InvoiceDto>;
    // Quotes: List<QuoteDto>;
    // Payments: List<PaymentDto>;

    CreatedAt?: Date;
    UpdatedAt?: Date;
}