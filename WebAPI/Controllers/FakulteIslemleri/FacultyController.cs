using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs;
using Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Command;
using Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Query;
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
    public class FacultyController : GenericController
    {
        public FacultyController(
            IMediator mediator,
            ILogger<FacultyController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("faculties")]
        [Authorize]
        [Tags("Faculty: General")]
        public async Task<IActionResult> GetFakulteler([FromHeader(Name = "Authorization")] string token, [FromQuery] FakulteGetAllQueryRequestDTO query, int page, int pageSize)
        {
            return await SendQuery(new FakulteGetAllQuery
            {
                Authorization = token,
                FakulteAdi = query.FakulteAdi,
                WebAdres = query.WebAdres,
                KurulusTarihi = query.KurulusTarihi,
                OlusturmaTarihi = query.OlusturmaTarihi,
                GuncellemeTarihi = query.GuncellemeTarihi,
                Pager = new Pager { Page = page, PageSize = pageSize }
            });
        }

        [HttpGet("faculty")]
        [Tags("Faculty: General")]
        [Authorize]
        public async Task<IActionResult> GetFakulte([FromHeader(Name = "Authorization")] string token, [FromQuery] FakulteQueryRequestDTO body)
        {
            return await SendQuery(new FakulteQuery
            {
                Authorization = token,
                FakulteUuid = body.FakulteUuid
            });
        }

        [HttpPost("faculty")]
        [Tags("Faculty: General")]
        [Authorize]
        public async Task<IActionResult> CreateFakulte([FromHeader(Name = "Authorization")] string token, [FromBody] FakulteAddCommandRequestDTO body)
        {
            return await SendCommand(new FakulteAddCommand
            {
                Authorization = token,
                FakulteAdi = body.FakulteAdi,
                WebAdres = body.WebAdres,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpPut("faculty")]
        [Tags("Faculty: General")]
        [Authorize]
        public async Task<IActionResult> UpdateFakulte([FromHeader(Name = "Authorization")] string token, [FromBody] FakulteUpdateCommandRequestDTO body)
        {
            return await SendCommand(new FakulteUpdateCommand
            {
                Authorization = token,
                FakulteUuid = body.FakulteUuid,
                FakulteAdi = body.FakulteAdi,
                WebAdres = body.WebAdres,
                KurulusTarihi = body.KurulusTarihi
            });
        }

        [HttpDelete("faculty")]
        [Tags("Faculty: General")]
        [Authorize]
        public async Task<IActionResult> DeleteFakulte([FromHeader(Name = "Authorization")] string token, [FromBody] FakulteDeleteCommandRequestDTO body)
        {
            return await SendCommand(new FakulteDeleteCommand
            {
                Authorization = token,
                FakulteUuid = body.FakulteUuid
            });
        }
    }
}