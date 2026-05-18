using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceRental.Domain.Common
{
    public abstract class ArchivedEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? ArchivedBy { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        public bool IsArchived { get; private set; } = false;


        public void SetCreatedBy(string userId)
        {
            CreatedBy = userId;
            CreatedAt = DateTime.UtcNow;
        }
        public void SetArchivedBy(string userId)
        {
            ArchivedBy = userId;
            ArchivedAt = DateTime.UtcNow;
            IsArchived = true;
        }
    }
}