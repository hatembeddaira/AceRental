using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace AceRental.Api.Configuration.Swagger;

public class ODataQueryOptionsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 2. AJOUT DES OPTIONS ODATA ($filter, etc.)
        var hasEnableQuery = context.MethodInfo.GetCustomAttributes(typeof(EnableQueryAttribute), true).Any();
        if (hasEnableQuery)
        {
            var odataParams = new[] { "$filter", "$expand", "$select", "$orderby", "$top", "$count" };
            foreach (var name in odataParams)
            {
                if (!operation.Parameters.Any(p => p.Name == name))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = name,
                        In = ParameterLocation.Query,
                        Required = false,
                        Schema = new OpenApiSchema { Type = "string" }
                    });
                }
            }
        }
        
        var relativePath = context.ApiDescription.RelativePath;
        if (string.IsNullOrEmpty(relativePath) || !relativePath.Contains("(")) return;

        // 1. On identifie les paramètres qui sont déjà dans les parenthèses {id}, {startDate}, etc.
        foreach (var parameter in operation.Parameters.ToList())
        {
            // Si le paramètre est présent dans le template de route entre accolades
            if (relativePath.Contains($"{{{parameter.Name}}}"))
            {
                // On force Swagger à comprendre qu'il fait partie du PATH, pas de la QUERY
                parameter.In = ParameterLocation.Path;
                parameter.Required = true;
            }
        }

        // 2. Nettoyage de sécurité : supprimer les paramètres en Query qui ont le même nom
        // car Swashbuckle a tendance à les recréer par erreur pour les types simples.
        var pathParamNames = operation.Parameters
            .Where(p => p.In == ParameterLocation.Path)
            .Select(p => p.Name).ToList();

        var duplicates = operation.Parameters
            .Where(p => p.In == ParameterLocation.Query && pathParamNames.Contains(p.Name))
            .ToList();

        foreach (var dup in duplicates) operation.Parameters.Remove(dup);

        
    }
}


