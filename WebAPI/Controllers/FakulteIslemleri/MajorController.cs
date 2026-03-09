using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs;
using Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Command;
using Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Query;
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
        public async Task<IActionResult> GetAnaDallar([FromHeader(Name = "Authorization")] string token, [FromQuery] AnaDalGetAllQueryRequestDTO query, int page, int pageSize)
        {
            return await SendQuery(new AnaDalGetAllQuery
            {
                Authorization = token,
                AnaDalAdi = query.AnaDalAdi,
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
        public async Task<IActionResult> GetAnaDal([FromHeader(Name = "Authorization")] string token, [FromQuery] AnaDalQueryRequestDTO body)
        {
            return await SendQuery(new AnaDalQuery
            {
                Authorization = token,
                AnaDalUuid = body.AnaDalUuid
            });
        }

        [HttpPost("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> CreateAnaDal([FromHeader(Name = "Authorization")] string token, [FromBody] AnaDalAddCommandRequestDTO body)
        {
            return await SendCommand(new AnaDalAddCommand
            {
                Authorization = token,
                AnaDalAdi = body.AnaDalAdi,
                FakulteUuid = body.FakulteUuid,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpPut("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> UpdateAnaDal([FromHeader(Name = "Authorization")] string token, [FromBody] AnaDalUpdateCommandRequestDTO body)
        {
            return await SendCommand(new AnaDalUpdateCommand
            {
                Authorization = token,
                AnaDalUuid = body.AnaDalUuid,
                AnaDalAdi = body.AnaDalAdi,
                FakulteUuid = body.FakulteUuid,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpDelete("major")]
        [Tags("Major: General")]
        [Authorize]
        public async Task<IActionResult> DeleteAnaDal([FromHeader(Name = "Authorization")] string token, [FromBody] AnaDalDeleteCommandRequestDTO body)
        {
            return await SendCommand(new AnaDalDeleteCommand
            {
                Authorization = token,
                AnaDalUuid = body.AnaDalUuid
            });
        }
    }
}