using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AceRental.Api.Middleware;

/// <summary>
/// Filtre global qui convertit automatiquement les réponses POST 200 en 201 Created.
///
/// Comportement :
///   - S'applique uniquement aux requêtes HTTP POST
///   - Si la réponse est un ObjectResult avec statut 200, le remplace par 201
///   - Tente de construire le header Location automatiquement :
///       POST /api/v1/equipments → Location: /api/v1/equipments/{id}
///   - Si le body contient un Guid (l'id créé), il est utilisé dans Location
///   - Sinon, Location pointe simplement vers la collection
///
/// Enregistrement dans Program.cs :
///   builder.Services.AddControllers(options =>
///       options.Filters.Add<CreatedResponseFilter>());
/// </summary>
public class CreatedResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // On ne traite que les POST qui ont réussi avec un 200
        if (context.HttpContext.Request.Method != HttpMethods.Post)
            return;

        if (context.Result is not ObjectResult objectResult)
            return;

        if (objectResult.StatusCode is not (null or StatusCodes.Status200OK))
            return;

        // Construire le header Location
        var location = BuildLocationHeader(context, objectResult.Value);

        // Remplacer le 200 par un 201 Created avec le header Location
        context.Result = new CreatedResult(location, objectResult.Value);
    }

    private static string BuildLocationHeader(ActionExecutedContext context, object? value)
    {
        var request = context.HttpContext.Request;
        var basePath = $"{request.Scheme}://{request.Host}{request.Path}";

        // Si le body est un Guid (pattern classique : return Ok(newId))
        if (value is Guid id && id != Guid.Empty)
            return $"{basePath}/{id}";

        // Si le body est un string qui ressemble à un Guid
        if (value is string str && Guid.TryParse(str, out var parsedId))
            return $"{basePath}/{parsedId}";

        // Fallback : pointe vers la collection
        return basePath;
    }
}
