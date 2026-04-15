using AceRental.Application.Equipments.Command;
using AceRental.Application.Reservations.Command;
using AceRental.Application.Reservations.Dtos;
using AceRental.Application.Reservations.Queries;
using AceRental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
namespace AceRental.Api.Controllers
{
    public class ReservationsController : ApiControllerBase
    {

        [HttpGet]
        [ProducesResponseType(typeof(List<ReservationDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
            => Ok(await Mediator.Send(new GetAllReservationsQuery()));

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(List<ReservationDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(Guid id)
            => Ok(await Mediator.Send(new GetReservationQuery(id)));

        [HttpGet("GetReservationTimeline/{id:guid}")]
        [ProducesResponseType(typeof(List<ReservationTimelineDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetReservationTimeline(Guid id)
            => Ok(await Mediator.Send(new GetReservationTimelineQuery(id)));

        [HttpGet("GetReservationTimelineString/{id:guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetReservationTimelineString(Guid id)
            => Ok(await Mediator.Send(new GetReservationTimelineStringQuery(id)));

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> Create(CreateReservationCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPut]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> Update(UpdateReservationCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPatch("logistic-status")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<bool>> LogisticStatus(ChangeLogisticStatusCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPatch("financial-status")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]  //  200 OK
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<bool>> FinancialStatus(ChangeFinancialStatusCommand command)
            => Ok(await Mediator.Send(command));


        [HttpPost("quote")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> GenerateDevis(GenerateQuoteCommand command)
            => Ok(await Mediator.Send(command));
    }
}