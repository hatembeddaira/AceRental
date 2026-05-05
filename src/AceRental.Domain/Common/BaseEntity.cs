namespace AceRental.Domain.Common
{
    public abstract class BaseEntity : TraceEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        
    }

}