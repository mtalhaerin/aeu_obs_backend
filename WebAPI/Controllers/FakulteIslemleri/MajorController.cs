using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs;
using Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Command;
using Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Query;
using Core.Utilities.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.FakulteIslemleri
{
    [ApiController]
    [Route("api/[controller]")]
    public class MajorController : GenericController
    {
        public MajorController(
            IMediator mediator,
            ILogger<MajorController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("majors")]
        [Authorize]
        [Tags("Major: General")]
        public async Task<IActionResult> GetBolumler([FromHeader(Name = "Authorization")] string token, [FromQuery] BolumGetAllQueryRequestDTO query, int page, int pageSize)
        {
            return await SendQuery(new BolumGetAllQuery
            {
                Authorization = token,
                BolumAdi = query.BolumAdi,
                FakulteUuid = query.FakulteUuid,
                KurulusTarihi = query.KurulusTarihi,
                OlusturmaTarihi = query.OlusturmaTarihi,
                GuncellemeTarihi = query.GuncellemeTarihi,
                Pager = new Pager { Page = page, PageSize = pageSize }
            });
        }

        [HttpGet("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> GetBolum([FromHeader(Name = "Authorization")] string token, [FromQuery] BolumQueryRequestDTO body)
        {
            return await SendQuery(new BolumQuery
            {
                Authorization = token,
                BolumUuid = body.BolumUuid
            });
        }

        [HttpPost("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> CreateBolum([FromHeader(Name = "Authorization")] string token, [FromBody] BolumAddCommandRequestDTO body)
        {
            return await SendCommand(new BolumAddCommand
            {
                Authorization = token,
                BolumAdi = body.BolumAdi,
                FakulteUuid = body.FakulteUuid,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpPut("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> UpdateBolum([FromHeader(Name = "Authorization")] string token, [FromBody] BolumUpdateCommandRequestDTO body)
        {
            return await SendCommand(new BolumUpdateCommand
            {
                Authorization = token,
                BolumUuid = body.BolumUuid,
                BolumAdi = body.BolumAdi,
                FakulteUuid = body.FakulteUuid,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpDelete("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> DeleteBolum([FromHeader(Name = "Authorization")] string token, [FromBody] BolumDeleteCommandRequestDTO body)
        {
            return await SendCommand(new BolumDeleteCommand
            {
                Authorization = token,
                BolumUuid = body.BolumUuid
            });
        }
    }
}