using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Invoices.Command;
using AceRental.Application.Invoices.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace AceRental.Api.Controllers.v1
{
    [Route("[controller]")]
    public class InvoicesController : ODataController
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        

        [HttpPost("Partial")]
        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> PartiallyInvoice([FromBody] GeneratePartiallyInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(PartiallyInvoice), new { id = result }, result);
        }

        [HttpPost("Rental")]
        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> RentalInvoice([FromBody] GenerateRentalInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(RentalInvoice), new { id = result }, result);
        }
    }
}