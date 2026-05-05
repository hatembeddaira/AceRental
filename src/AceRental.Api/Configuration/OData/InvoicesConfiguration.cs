using AceRental.Application.Invoices.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;

namespace AceRental.Api.Configuration.OData
{
    public class InvoicesConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<InvoiceDto>("Invoices").EntityType;
                dto.HasKey(e => e.Id);
                // dto.HasRequired(i => i.Reservation);
                // dto.HasMany(i => i.Payment);
            }
        }
    }
}