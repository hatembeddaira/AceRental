// using AceRental.Application.Clients.Command;
using AceRental.Application.Clients.Dtos;
using AceRental.Application.Clients.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    public class ClientsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllClientsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClientDto>> Get(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new GetClientQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}