using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class ReservationHistory : MinBaseEntity
    {
        public Guid ReservationId { get; set; }
        public required HistoryType HistoryType {get; set;}
        public int VersionNumber { get; set; }
        public string? ChangeReason { get; set; }
        public required string DataSnapshotJson { get;  set; }
    }
}