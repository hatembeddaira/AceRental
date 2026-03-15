using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Equipments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class EquipmentsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<EquipmentDetailsDto>>> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllEquipmentsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EquipmentDto>> Get(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetEquipmentQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAvailability/{id:guid}/{startDate:datetime}/{endDate:datetime}")]
        public async Task<ActionResult<int>> GetAvailability(Guid id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await Mediator.Send(new GetEquipmentAvailabilityQuery(id, startDate, endDate));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateEquipmentCommand command)
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
