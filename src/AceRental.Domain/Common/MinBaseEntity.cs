namespace AceRental.Domain.Common
{
    public abstract class MinBaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? CreatedBy { get; private set; }

        public void SetCreatedBy(string userId) => CreatedBy = userId;
    }
}