using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AceRental.Application.Quotes.Command;
using AceRental.Application.Quotes.Dtos;
using AceRental.Application.Quotes.Queries;

namespace AceRental.Api.Controllers.v1
{
    public class QuotesController : ODataController
    {
        private readonly IMediator _mediator;

        public QuotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<QuoteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllQuotesQuery()));
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(QuoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllQuotesQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(typeof(QuoteDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] GenerateQuoteCommand command)
        {
            return Created(await _mediator.Send(command));
        }
    }
}
