using AceRental.Application.Packs.Command;
using AceRental.Application.Packs.Dtos;
using AceRental.Application.Packs.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class PacksController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<PackDetailsDto>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllPacksQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PackDetailsDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetPackQuery(id));
            return Ok(result);
        }

        [HttpGet("GetAvailability/{id:guid}/{startDate:datetime}/{endDate:datetime}")]
        public async Task<ActionResult<int>> GetAvailability(Guid id, DateTime startDate, DateTime endDate)
        {
            var result = await Mediator.Send(new GetPackAvailabilityQuery(id, startDate, endDate));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePackCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        
    }
}
