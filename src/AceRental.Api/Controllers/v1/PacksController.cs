using AceRental.Application.Packs.Command;
using AceRental.Application.Packs.Dtos;
using AceRental.Application.Packs.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AceRental.Api.Controllers.v1
{
    public class PacksController : ODataController
    {
        private readonly IMediator _mediator;

        public PacksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<PackDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllPacksQuery()));
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(PackDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllPacksQuery());
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
            return Ok(await _mediator.Send(new GetPackAvailabilityQuery(id, startDate, endDate)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(PackDetailsDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreatePackCommand command)
        {
            return Created(await _mediator.Send(command));
        }

    }
}
