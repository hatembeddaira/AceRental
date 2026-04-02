namespace AceRental.Application.Exceptions;

/// <summary>
/// Exception levée quand une ressource demandée est introuvable (→ 404)
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string resourceName, object key)
        : base($"La ressource '{resourceName}' avec l'identifiant '{key}' est introuvable.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}
