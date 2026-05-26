export interface ClientReservationDto {
    ClientNumber: string;
    RaisonSociale?: string;
    FirstName : string;
    LastName : string;
    Email : string;
    Password: string;
    Civilite: string;
    TelNumber?: string;
    PhoneNumber?: string;
    Address?: string;
    ComplementAdresse?: string;
    PostalCode: number;
    City?: string;
    CreateAt: Date;
}
