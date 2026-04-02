namespace AceRental.Application.Exceptions;

/// <summary>
/// Exception levée quand une règle métier est violée (→ 409 Conflict)
/// Ex : équipement non disponible pour les dates demandées
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
