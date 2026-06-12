using System.ComponentModel.DataAnnotations;
using AceRental.Application.Clients.Dtos;
using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Clients.Command;

public record CreateClientCommand(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    string RaisonSociale,
    string PhoneNumber,
    string TelNumber,
    string Address,
    string ComplementAdresse,
    string City,
    string PostalCode
    ) : IRequest<ClientDto>;
