using AceRental.Application.Equipments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;

namespace AceRental.Api.Controllers.v1
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
            return Ok(await _mediator.Send(new GetAllEquipmentsQuery()));
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(EquipmentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllEquipmentsQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Availability([FromODataUri] Guid id, [FromODataUri] DateTime startDate, [FromODataUri] DateTime endDate)
        {
            return Ok(await _mediator.Send(new GetEquipmentAvailabilityQuery(id, startDate, endDate)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(EquipmentDetailsDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreateEquipmentCommand command)
        {
            return Created(await _mediator.Send(command));
        }
    }
}
