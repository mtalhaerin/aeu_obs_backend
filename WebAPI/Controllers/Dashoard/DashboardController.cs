using Business.Concrete;
using Business.Features.CQRS.Dashboard.Profile.Command;
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
        private readonly IKullaniciService _kullaniciService;

        public DashboardController(
            IMediator mediator,
            ILogger<DashboardController> logger,
            IKullaniciService kullaniciService) : base(mediator, logger)
        {
            _kullaniciService = kullaniciService;
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile([FromHeader(Name = "Authorization")] ProfileQuery query)
        {
            return await SendQuery(query);
        }

        //Kullanıcı profili Güncelleme
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromHeader(Name = "Authorization")] ProfileUpdateCommand command)
        {
            return await SendCommand(command);
        }
    }
}
