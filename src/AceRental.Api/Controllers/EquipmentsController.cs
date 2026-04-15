using AceRental.Application.Equipments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;

namespace AceRental.Api.Controllers
{
    public class EquipmentsController : ODataController
    {
        private readonly IMediator _mediator;

        public EquipmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<EquipmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllEquipmentsQuery());
            return Ok(result);
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(EquipmentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllEquipmentsQuery());
            var equipment = result.FirstOrDefault(e => e.Id == key);
            return Ok(equipment);
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Availability([FromODataUri] Guid key, [FromODataUri] DateTime startDate, [FromODataUri] DateTime endDate)
        {
            var result = await _mediator.Send(new GetEquipmentAvailabilityQuery(key, startDate, endDate));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreateEquipmentCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(command);
            return Created(result);
        }
    }
}
