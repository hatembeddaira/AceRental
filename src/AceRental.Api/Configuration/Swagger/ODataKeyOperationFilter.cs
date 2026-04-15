using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AceRental.Api.Configuration.Swagger;

public class ODataKeyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Correction : On vérifie si c'est nul, si oui on initialise
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        // 1. On cherche tous les paramètres nommés "key"
        var existingKeys = operation.Parameters
            .Where(p => p.Name == "key")
            .ToList();

        if (existingKeys.Any())
        {
            // On supprime les anciennes versions
            foreach (var key in existingKeys)
            {
                operation.Parameters.Remove(key);
            }

            // 2. On ajoute le paramètre unique et propre
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "key",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema
                {
                    // Utilisation du type énuméré correct pour .NET 8/9
                    Type = "string",
                    Format = "uuid"
                },
                Description = "Identifiant unique (GUID)"
            });
        }
    }
}