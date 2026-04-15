using AceRental.Application.Clients.Dtos;
using AceRental.Application.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AceRental.Api.Controllers
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
            var result = await _mediator.Send(new GetAllClientsQuery());
            return Ok(result);
        }


        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllClientsQuery());
            var client = result.FirstOrDefault(c => c.Id == key);
            return Ok(client);
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