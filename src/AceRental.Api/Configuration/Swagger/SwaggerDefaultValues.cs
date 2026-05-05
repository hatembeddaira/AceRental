using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi; // Tout est ici
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models; 
using System.Linq;
using System.Text.Json;
using Microsoft.OpenApi.Any;

namespace AceRental.Api.Configuration.Swagger;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        // On gère la dépréciation
        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters)
        {
            // Sécurité : FirstOrDefault pour éviter le crash "Sequence contains no matching element"
            var description = apiDescription.ParameterDescriptions
                .FirstOrDefault(p => p.Name == parameter.Name);

            if (description == null) continue;

            if (parameter.Description == null)
            {
                parameter.Description = description.ModelMetadata?.Description;
            }

            // Correction Read-Only : Cast vers la classe concrète OpenApiSchema
            if (parameter.Schema is OpenApiSchema concreteSchema)
            {
                if (concreteSchema.Default == null && description.DefaultValue != null)
                {
                    // En v2.x sans Models, on instancie directement l'objet de base
                    concreteSchema.Default = new OpenApiString(description.DefaultValue.ToString());
                }
            }

            parameter.Required |= description.IsRequired;
        }
    }
}