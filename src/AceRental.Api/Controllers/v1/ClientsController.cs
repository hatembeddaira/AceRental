using AceRental.Application.Clients.Dtos;
using AceRental.Application.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AceRental.Api.Controllers.v1
{
    // [ODataAttributeRouting]
    public class ClientsController  : ODataController
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<ClientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllClientsQuery()));
        }


        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllClientsQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }
        // [HttpPost]
        // [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]  //  201 Created
        // [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        // [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        // public async Task<ActionResult<Guid>> Create(CreateClientCommand command)
        // {
        //     var id = await Mediator.Send(command);
        //     return CreatedAtAction(nameof(Get), new { id }, id);
        // }
    }
}