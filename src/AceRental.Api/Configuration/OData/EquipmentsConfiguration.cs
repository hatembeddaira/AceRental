using AceRental.Application.Equipments.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;
using AceRental.Api.Controllers;

namespace AceRental.Api.Configuration.OData
{
    public class EquipmentsConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<EquipmentDetailsDto>("Equipments").EntityType;
                dto.HasKey(e => e.Id);

                var getAvailability = dto.Collection.Function("Availability");
                getAvailability.Parameter<Guid>("id");
                getAvailability.Parameter<DateTime>("startDate");
                getAvailability.Parameter<DateTime>("endDate");
                getAvailability.Returns<int>();          
            }

            
        }
    }
}