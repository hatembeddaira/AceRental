using AceRental.Application.Quotes.Dtos;
using MediatR;

namespace AceRental.Application.Quotes.Queries
{
    public record GetAllQuotesQuery() : IRequest<IQueryable<QuoteDto>>; 
}