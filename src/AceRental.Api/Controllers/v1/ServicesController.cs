using AceRental.Application.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AceRental.Application.Services.Command;
using AceRental.Application.Services.Dtos;

namespace AceRental.Api.Controllers.v1
{
    public class ServicesController : ODataController
    {
        private readonly IMediator _mediator;

        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllServicesQuery()));
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllServicesQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreateServiceCommand command)
        {
            return Created(await _mediator.Send(command));
        }

        [HttpPatch]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Patch([FromODataUri] Guid key, [FromBody] UpdateServiceCommand command)
        {
            command.Id = key;
            return Ok(await _mediator.Send(command));
        }
    }
}
