using AceRental.Application.Quotes.Dtos;
using AceRental.Application.Reservations.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;

namespace AceRental.Api.Configuration.OData
{
    public class QuotesConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var quotes = builder.EntitySet<QuoteDto>("Quotes");
                quotes.EntityType.HasKey(e => e.Id);
                // quotes.HasRequiredBinding(e => e.Reservation, builder.EntitySet<ReservationDto>("Reservations"));
            }
        }
    }
}