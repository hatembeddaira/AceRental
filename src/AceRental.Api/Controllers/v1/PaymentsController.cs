using AceRental.Application.Payments.Command;
using AceRental.Application.Payments.Dtos;
using AceRental.Application.Payments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AceRental.Api.Controllers.v1
{
    public class PaymentsController : ODataController
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllPaymentsQuery()));
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }
        
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<PaymentDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reservation([FromODataUri] Guid reservationId)
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            var obj = result.Where(e => e.ReservationId == reservationId);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreatePaymentCommand command)
        {
            return Created(await _mediator.Send(command));
        }
    }
}