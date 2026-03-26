using AceRental.Application.Payments.Command;
using AceRental.Application.Payments.Dtos;
using AceRental.Application.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class PaymentsController : ApiControllerBase
    {
        [HttpGet("reservation/{reservationId:guid}")]
        public async Task<ActionResult<List<PaymentDetailsDto>>> GetAll(Guid reservationId)
        {
            try
            {
                var result = await Mediator.Send(new GetAllPaymentsQuery(reservationId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PaymentDetailsDto>> Get(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetPaymentDetailsQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePaymentCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}