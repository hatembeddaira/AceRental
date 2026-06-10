export interface ReservationItemDto {
    Reference: string;
    Name: string;
    Type: 'Service' | 'Pack' | 'Equipment';
    UnitPrice: number;
    IsDailyPrice: boolean;
    Id: string;
}