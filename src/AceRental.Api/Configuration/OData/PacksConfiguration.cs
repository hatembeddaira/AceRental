using AceRental.Application.Packs.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;

namespace AceRental.Api.Configuration.OData
{
    public class PacksConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<PackDetailsDto>("Packs").EntityType;
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