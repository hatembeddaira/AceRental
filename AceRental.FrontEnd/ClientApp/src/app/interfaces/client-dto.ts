export interface ClientDto {
    Id?: string;
    ClientNumber: string;
    RaisonSociale?: string;
    LastName : string;
    FirstName : string;
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
