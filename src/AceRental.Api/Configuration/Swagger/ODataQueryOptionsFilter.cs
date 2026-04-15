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
        var relativePath = context.ApiDescription.RelativePath;
        if (string.IsNullOrEmpty(relativePath)) return;

        // 1. DÉTECTION DES PARAMÈTRES DANS LES PARENTHÈSES {param}
        var matches = Regex.Matches(relativePath, @"\{([a-zA-Z0-9_]+)\}");

        if (matches.Any())
        {
            
            // var pathParamNames = pathVariableMatches
            // .Cast<Match>() // Nécessaire pour LINQ sur MatchCollection
            // .Select(m => m.Groups["pName"].Value)
            // .ToList();
            

            foreach (Match match in matches)
            {
                string pName = match.Groups["pName"].Value;
                // On cherche l'ancien paramètre (souvent en Query par défaut)
                var oldParam = operation.Parameters.FirstOrDefault(p =>
                    p.Name.Equals(pName, StringComparison.OrdinalIgnoreCase));

                if (oldParam != null)
                {
                    // On le supprime car ses propriétés 'In' et 'Required' sont read-only
                    operation.Parameters.Remove(oldParam);

                    // On en crée un nouveau identique mais positionné dans le PATH
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = oldParam.Name,
                        Description = oldParam.Description,
                        Required = true,
                        In = ParameterLocation.Path, // Enfin assignable ici !
                        Schema = oldParam.Schema
                    });
                }
            }
        }

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
    }

}


