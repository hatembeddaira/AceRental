using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Equipments.Dtos;

namespace AceRental.Application.Packs.Dtos
{
    public class PackItemDto
    {
        public Guid Id { get; set; }
        public Guid PackId { get; set; }
        public required Guid EquipmentId { get; set; }
        public EquipmentDto? Equipment { get; set; } 
        public required int Quantity { get; set; }
    }
}