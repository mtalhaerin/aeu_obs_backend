using Business.Concrete;
using Business.Features.CQRS.Dashboard.Profile.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.Dashoard
{

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : GenericController
    {
        public DashboardController(
            IMediator mediator,
            ILogger<DashboardController> logger): base(mediator, logger)
        {
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile([FromHeader(Name = "Authorization")] ProfileQuery command)
        {
            return await SendQuery(command);
        }
    }
}
