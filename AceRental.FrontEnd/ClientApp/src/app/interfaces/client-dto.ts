export interface ClientDto {
    id: number;
    raisonSociale?: string;
    nomClient : string;
    prenomClient : string;
    email : string;
    password: string;
    civilite: string;
    tel?: string;
    portable?: string;
    adresse?: string;
    complementAdresse?: string;
    codepostale?: number;
    ville?: string;
    dateCreation: Date;
}
