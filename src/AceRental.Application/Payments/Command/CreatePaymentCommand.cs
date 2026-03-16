using AceRental.Application.Payments.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Payments.Command;

public record CreatePaymentCommand(
    Guid ReservationId,
    decimal Amount,
    DateTime Date,
    PaymentMethod Method,
    PaymentType Type,
    string? TransactionId
    ) : IRequest<Guid>;

//public record PaymentItemDto(Guid? EquipmentId, Guid? PackId, int Quentity);