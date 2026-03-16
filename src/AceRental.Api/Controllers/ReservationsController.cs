using AceRental.Application.Equipments.Command;
using AceRental.Application.Reservations.Command;
using AceRental.Application.Reservations.Queries;
using AceRental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
namespace AceRental.Api.Controllers
{
    public class ReservationsController : ApiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllReservationsQuery());            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetReservationQuery(id));            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReservationTimeline/{id:guid}")]
        public async Task<ActionResult> GetReservationTimeline(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetReservationTimelineQuery(id));            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReservationTimelineString/{id:guid}")]
        public async Task<ActionResult> GetReservationTimelineString(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetReservationTimelineStringQuery(id));            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateReservationCommand command)
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
        
        [HttpPut]
        public async Task<ActionResult<Guid>> Update(UpdateReservationCommand command)
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
        [HttpPatch("logistic-status")]
        public async Task<ActionResult<Guid>> LogisticStatus(ChangeLogisticStatusCommand command)
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
        
        [HttpPatch("financial-status")]
        public async Task<ActionResult<Guid>> FinancialStatus(ChangeFinancialStatusCommand command)
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


        [HttpPost("GenerateDevis")]
        public async Task<ActionResult<Guid>> GenerateDevis(GenerateQuoteCommand command)
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