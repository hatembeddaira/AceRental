using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Equipments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class EquipmentsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<EquipmentDetailsDto>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllEquipmentsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EquipmentDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetEquipmentQuery(id));
            return Ok(result);
        }

        [HttpGet("GetAvailability/{id:guid}/{startDate:datetime}/{endDate:datetime}")]
        public async Task<ActionResult<int>> GetAvailability(Guid id, DateTime startDate, DateTime endDate)
        {
            var result = await Mediator.Send(new GetEquipmentAvailabilityQuery(id, startDate, endDate));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateEquipmentCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
