using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AceRental.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;
        // Utilise l'injection paresseuse pour MediatR
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}