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
                var dto = builder.EntitySet<QuoteDto>("Quotes");
                dto.EntityType.HasKey(e => e.Id);
            }
        }
    }
}