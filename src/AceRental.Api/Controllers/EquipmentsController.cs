using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Equipments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class EquipmentsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<EquipmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EquipmentDetailsDto>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllEquipmentsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EquipmentDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetEquipmentQuery(id));
            return Ok(result);
        }

        [HttpGet("GetAvailability/{id:guid}/{startDate:datetime}/{endDate:datetime}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetAvailability(Guid id, DateTime startDate, DateTime endDate)
        {
            var result = await Mediator.Send(new GetEquipmentAvailabilityQuery(id, startDate, endDate));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> Create(CreateEquipmentCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
    }
}
