using AceRental.Application.Services.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;
using AceRental.Api.Controllers;
using AceRental.Application.Reservations.Dtos;

namespace AceRental.Api.Configuration.OData
{
    public class ReservationEquipmentsConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<ReservationEquipmentsDto>("ReservationEquipments").EntityType;
                dto.HasKey(e => new { e.ReservationId, e.EquipmentId });       
            }
        }
    }
}