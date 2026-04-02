using FluentValidation.Results;

namespace AceRental.Application.Exceptions;

/// <summary>
/// Exception levée quand la validation métier échoue (→ 422)
/// </summary>
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("Des erreurs de validation ont été détectées.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Construit l'exception à partir des erreurs FluentValidation
    /// </summary>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}
