using AceRental.Application.Clients.Dtos;
using AceRental.Application.Equipments.Command;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;

namespace AceRental.Application.Clients.Command;

public class CreateClientHandler : IRequestHandler<CreateClientCommand, ClientDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateClientHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var Client = new ClientDto
        {
            Id = Guid.NewGuid(),
            ClientNumber = 0,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            RaisonSociale = request.RaisonSociale,
            PhoneNumber = request.PhoneNumber,
            TelNumber = request.TelNumber,
            Address = request.Address,
            ComplementAdresse = request.ComplementAdresse,
            City = request.City,
            PostalCode = int.Parse(request.PostalCode)
        };
        
        _context.Clients.Add(_mapper.Map<Client>(Client));
        await _context.SaveChangesAsync(cancellationToken);
        return Client;
    }
}

 
