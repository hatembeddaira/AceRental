using AceRental.Api.Middleware;

namespace AceRental.Api.Extensions;

public static class MiddlewareExtensions
{
    /// <summary>
    /// Enregistre le middleware global de gestion des erreurs.
    /// Doit être appelé en PREMIER dans le pipeline (avant UseRouting, UseAuthorization, etc.)
    /// pour intercepter toutes les exceptions.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
