using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AceRental.Api.Configuration.Swagger;

public class ODataRouteFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // On cherche les routes qui ont à la fois une version parenthèses () et une version slash {}
        var routesToRemove = swaggerDoc.Paths
            .Where(path => path.Key.Contains("/{key}") && !path.Key.Contains("("))
            .ToList();

        foreach (var route in routesToRemove)
        {
            swaggerDoc.Paths.Remove(route.Key);
        }
    }
}