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
        [ProducesResponseType(typeof(List<PackDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PackDetailsDto>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllPacksQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PackDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PackDetailsDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetPackQuery(id));
            return Ok(result);
        }

        [HttpGet("GetAvailability/{id:guid}/{startDate:datetime}/{endDate:datetime}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetAvailability(Guid id, DateTime startDate, DateTime endDate)
        {
            var result = await Mediator.Send(new GetPackAvailabilityQuery(id, startDate, endDate));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]  //  201 Created
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]    //  validation
        [ProducesResponseType(StatusCodes.Status409Conflict)]               //  règle métier
        public async Task<ActionResult<Guid>> Create(CreatePackCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

    }
}
