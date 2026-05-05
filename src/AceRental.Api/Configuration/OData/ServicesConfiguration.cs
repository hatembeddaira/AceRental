using AceRental.Application.Services.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;
using AceRental.Api.Controllers;

namespace AceRental.Api.Configuration.OData
{
    public class ServicesConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<ServiceDto>("Services").EntityType;
                dto.HasKey(e => e.Id);       
            }
        }
    }
}