using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Services.Dtos;

namespace AceRental.Application.Packs.Dtos
{
    public class PackServiceDto
    {
        public required ServiceDto Service { get; set; }
        public bool IsFree { get; set; } = true;
    }
}