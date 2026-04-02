using AceRental.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AceRental.Api.Middleware;

/// <summary>
/// Middleware global de gestion des erreurs.
/// Intercepte toutes les exceptions non gérées et retourne une réponse
/// RFC 7807 (ProblemDetails) avec le bon code HTTP.
///
/// Mapping des exceptions :
///   NotFoundException        → 404 Not Found
///   ValidationException      → 422 Unprocessable Entity
///   BusinessRuleException    → 409 Conflict
///   UnauthorizedAccessException → 401 Unauthorized
///   Exception (tout le reste) → 500 Internal Server Error (sans leaker les détails en prod)
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, problemDetails) = exception switch
        {
            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                CreateProblem(
                    title: "Ressource introuvable",
                    detail: notFound.Message,
                    status: StatusCodes.Status404NotFound,
                    instance: context.Request.Path
                )
            ),

            ValidationException validation => (
                StatusCodes.Status422UnprocessableEntity,
                CreateValidationProblem(
                    errors: validation.Errors,
                    instance: context.Request.Path
                )
            ),

            BusinessRuleException businessRule => (
                StatusCodes.Status409Conflict,
                CreateProblem(
                    title: "Règle métier violée",
                    detail: businessRule.Message,
                    status: StatusCodes.Status409Conflict,
                    instance: context.Request.Path
                )
            ),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                CreateProblem(
                    title: "Non autorisé",
                    detail: "Vous n'êtes pas autorisé à effectuer cette action.",
                    status: StatusCodes.Status401Unauthorized,
                    instance: context.Request.Path
                )
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                CreateProblem(
                    title: "Erreur interne du serveur",
                    // En prod, on ne retourne pas les détails de l'exception
                    detail: _env.IsDevelopment()
                        ? exception.ToString()
                        : "Une erreur inattendue s'est produite. Veuillez réessayer plus tard.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: context.Request.Path
                )
            )
        };

        // Log toujours les erreurs inattendues, avec moins de verbosité pour les erreurs connues
        if (statusCode >= 500)
            _logger.LogError(exception, "Erreur non gérée sur {Method} {Path}", context.Request.Method, context.Request.Path);
        else
            _logger.LogWarning(exception, "Erreur {StatusCode} sur {Method} {Path}", statusCode, context.Request.Method, context.Request.Path);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions)
        );
    }

    private static ProblemDetails CreateProblem(string title, string detail, int status, string instance)
        => new()
        {
            Title = title,
            Detail = detail,
            Status = status,
            Instance = instance,
            Type = $"https://httpstatuses.com/{status}"
        };

    private static ValidationProblemDetails CreateValidationProblem(
        IDictionary<string, string[]> errors,
        string instance)
        => new(errors)
        {
            Title = "Erreurs de validation",
            Detail = "Un ou plusieurs champs sont invalides.",
            Status = StatusCodes.Status422UnprocessableEntity,
            Instance = instance,
            Type = "https://httpstatuses.com/422"
        };
}
