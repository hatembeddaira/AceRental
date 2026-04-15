using AceRental.Application.Payments.Command;
using AceRental.Application.Payments.Dtos;
using AceRental.Application.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class PaymentsController : ApiControllerBase
    {
        [HttpGet("reservation/{reservationId:guid}")]
        [ProducesResponseType(typeof(List<PaymentDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PaymentDetailsDto>>> GetAll(Guid reservationId)
        {
            var result = await Mediator.Send(new GetAllPaymentsQuery(reservationId));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PaymentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDetailsDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetPaymentDetailsQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> Create(CreatePaymentCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
    }
}