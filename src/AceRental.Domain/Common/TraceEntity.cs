using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceRental.Domain.Common
{
    public abstract class TraceEntity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }

        public void SetCreatedBy(string userId) => CreatedBy = userId;

        public void SetUpdatedBy(string userId)
        {
            UpdatedBy = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete(string userId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedBy = userId;
        }
    }
}