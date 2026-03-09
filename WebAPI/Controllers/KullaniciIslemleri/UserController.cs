using Business.Concrete;
using Business.DTOs.RequestDTOs.KullaniciDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.KullaniciDTOs.QueryDTOs;
using Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Command;
using Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Query;
using Core.Entities.Enums;
using Core.Utilities.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.KullaniciIslemleri
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : GenericController
    {
        public UserController(
            IMediator mediator,
            ILogger<UserController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("users")]
        [Authorize]
        [Tags("User: General")]
        public async Task<IActionResult> GetUsers([FromHeader(Name = "Authorization")] string token, [FromQuery] UserGetAllQueryRequestDTO query, int page, int pageSize)
        {

            return await SendQuery(new UsersQuery
            {
                Authorization = token,
                KullaniciTipi = query.KullaniciTipi,
                Ad = query.Ad,
                OrtaAd = query.OrtaAd,
                Soyad = query.Soyad,
                KurumEposta = query.KurumEposta,
                KurumSicilNo = query.KurumSicilNo,
                OlusturmaTarihi = query.OlusturmaTarihi,
                GuncellemeTarihi = query.GuncellemeTarihi,
                Pager = new Pager { Page = page, PageSize = pageSize }
            });
        }

        [HttpGet("user")]
        [Tags("User: General")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromHeader(Name = "Authorization")] string token, [FromQuery] UserQueryRequestDTO body)
        {
            return await SendQuery(new UserQuery
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid
            });

        }

        [HttpPost("user")]
        [Tags("User: General")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromHeader(Name = "Authorization")] string token, [FromBody] UserAddCommandRequestDTO body)
        {
            return await SendCommand(new UserAddCommand
            {
                Authorization = token,
                KullaniciTipi = body.KullaniciTipi,
                Ad = body.Ad,
                OrtaAd = body.OrtaAd,
                Soyad = body.Soyad,
                KurumEposta = body.KurumEposta,
                KurumSicilNo = body.KurumSicilNo
            });
        }

        [HttpPut("user")]
        [Tags("User: General")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromHeader(Name = "Authorization")] string token, [FromBody] UserUpdateCommandRequestDTO body)
        {
            return await SendCommand(new UserUpdateCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                KullaniciTipi = body.KullaniciTipi,
                Ad = body.Ad,
                OrtaAd = body.OrtaAd,
                Soyad = body.Soyad,
                KurumEposta = body.KurumEposta,
                KurumSicilNo = body.KurumSicilNo
            });
        }

        [HttpDelete("user")]
        [Tags("User: General")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromHeader(Name = "Authorization")] string token, [FromBody] UserDeleteCommandRequestDTO body)
        {
            return await SendCommand(new UserDeleteCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid
            });
        }
    }
}
