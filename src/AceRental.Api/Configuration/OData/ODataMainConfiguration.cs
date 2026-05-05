using AceRental.Infrastructure.Configurations;
using Asp.Versioning;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AceRental.Api.Configuration.OData
{
    public static class ODataMainConfiguration
    {
        public static IEdmModel GetMasterEdmModel(ApiVersion apiVersion)
        {
            var builder = new ODataConventionModelBuilder();

            // On appelle chaque configuration d'entité pour construire le modèle global
            new ClientsConfiguration().Apply(builder, apiVersion, null);
            new EquipmentsConfiguration().Apply(builder, apiVersion, null);
            new InvoicesConfiguration().Apply(builder, apiVersion, null);
            new PacksConfiguration().Apply(builder, apiVersion, null);
            new PaymentsConfiguration().Apply(builder, apiVersion, null);
            new ReservationsConfiguration().Apply(builder, apiVersion, null);
            new ServicesConfiguration().Apply(builder, apiVersion, null);
            new QuotesConfiguration().Apply(builder, apiVersion, null);
            new ReservationEquipmentsConfiguration().Apply(builder, apiVersion, null);
            new ReservationPacksConfiguration().Apply(builder, apiVersion, null);
            new ReservationServicesConfiguration().Apply(builder, apiVersion, null);
            return builder.GetEdmModel();
        }
    }
}