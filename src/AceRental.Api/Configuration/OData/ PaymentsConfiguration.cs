using AceRental.Application.Payments.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;
using AceRental.Api.Controllers;

namespace AceRental.Api.Configuration.OData
{
    public class PaymentsConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<PaymentDetailsDto>("Payments").EntityType;
                dto.HasKey(e => e.Id);

                var getPaymentsReservation = dto.Collection.Function("Reservation");
                getPaymentsReservation.Parameter<Guid>("reservationId");
                getPaymentsReservation.ReturnsCollection<PaymentDetailsDto>();          
            }

            
        }
    }
}