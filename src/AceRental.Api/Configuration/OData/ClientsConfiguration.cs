using AceRental.Application.Clients.Dtos;
using AceRental.Domain.Entities;
using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AceRental.Api.Configuration.OData
{
    public class ClientsConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var clientEntity = builder.EntitySet<ClientDto>("Clients").EntityType;
                clientEntity.HasKey(c => c.Id);
        
            }
        }
    }
}