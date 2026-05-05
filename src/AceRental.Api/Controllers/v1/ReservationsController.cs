using AceRental.Application.Equipments.Command;
using AceRental.Application.Reservations.Command;
using AceRental.Application.Reservations.Dtos;
using AceRental.Application.Reservations.Queries;
using AceRental.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
namespace AceRental.Api.Controllers.v1
{
    public class ReservationsController : ODataController
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ProducesResponseType(typeof(IQueryable<ReservationDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllReservationsQuery()));
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 8, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ProducesResponseType(typeof(ReservationDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid key)
        {
            var result = await _mediator.Send(new GetAllReservationsQuery());
            var obj = result.FirstOrDefault(e => e.Id == key);
            if (obj == null)
            {
                return NoContent();
            }
            return Ok(obj);
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<ReservationTimelineDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Timeline(Guid key)
        {
            return Ok(await _mediator.Send(new GetReservationTimelineQuery(key)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReservationDetailsDto), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Post([FromBody] CreateReservationCommand command)
        {
            return Created(await _mediator.Send(command));
        }

        [HttpPatch]
        [ProducesResponseType(typeof(ReservationDetailsDto), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> Patch([FromODataUri] Guid key, [FromBody] UpdateReservationCommand command)
        {
            command.ReservationId = key; 
            return Ok(await _mediator.Send(command));
        }
        
            
        [HttpPatch("api/v1/Reservations({key})/LogisticStatus")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> LogisticStatus([FromODataUri] Guid key, [FromBody] ChangeLogisticStatusCommand command)
        {
            command.ReservationId = key;
            return Ok(await _mediator.Send(command));
        }

        [HttpPatch("api/v1/Reservations({key})/FinancialStatus")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<IActionResult> FinancialStatus([FromODataUri] Guid key, [FromBody] ChangeFinancialStatusCommand command)
        {
            command.ReservationId = key;
            return Ok(await _mediator.Send(command));
        }            
    }
}