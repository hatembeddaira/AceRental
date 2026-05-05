using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AceRental.Api.Configuration.Swagger;

public class ODataRouteFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // On vérifie si la route contient des paramètres entre accolades {}
        var pathParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.Source.Id == "Path")
            .Select(p => p.Name)
            .ToList();

        if (!pathParameters.Any()) return;

        // On cherche les paramètres que Swagger a mis en 'Query' mais qui sont déjà dans le 'Path'
        var parametersToRemove = operation.Parameters
            .Where(p => p.In == ParameterLocation.Query && 
                        pathParameters.Any(pp => pp.Equals(p.Name, System.StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var parameter in parametersToRemove)
        {
            operation.Parameters.Remove(parameter);
        }
    }
}