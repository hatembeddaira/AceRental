using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AceRental.Api.Configuration.OData
{
    public class ODataMainConfiguration
    {
        public static IEdmModel GetMasterEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            // On appelle chaque configuration d'entité pour construire le modèle global
            // ClientsConfiguration.Apply(builder, ApiVersions.V1, null);
            // EquipmentsConfiguration.Apply(builder, ApiVersions.V1, null);
            // ... ajoute les autres ici

            return builder.GetEdmModel();
        }
    }
}