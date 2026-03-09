using Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.QueryDTOs;
using Business.Features.CQRS.DersHandlers.Command;
using Business.Features.CQRS.DersHandlers.Query;
using Core.Utilities.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.DersIslemleri
{
    [ApiController]
    [Route("api/[controller]")]
    public class LectureController : GenericController
    {
        public LectureController(
            IMediator mediator,
            ILogger<LectureController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("lectures")]
        [Authorize]
        [Tags("Lecture: General")]
        public async Task<IActionResult> GetDersler([FromHeader(Name = "Authorization")] string token, [FromQuery] DersGetAllQueryRequestDTO query, int page, int pageSize)
        {
            return await SendQuery(new DersGetAllQuery
            {
                Authorization = token,
                Pager = new Pager { Page = page, PageSize = pageSize }
            });
        }

        [HttpGet("lecture")]
        [Authorize]
        [Tags("Lecture: General")]
        public async Task<IActionResult> GetDers([FromHeader(Name = "Authorization")] string token, [FromQuery] DersQueryRequestDTO body)
        {
            return await SendQuery(new DersQuery
            {
                Authorization = token,
                DersUuid = body.DersUuid
            });
        }

        [HttpPost("lecture")]
        [Authorize]
        [Tags("Lecture: General")]
        public async Task<IActionResult> CreateDers([FromHeader(Name = "Authorization")] string token, [FromBody] DersAddCommandRequestDTO body)
        {
            return await SendCommand(new DersAddCommand
            {
                Authorization = token,
                Ders = body
            });
        }

        [HttpPut("lecture")]
        [Authorize]
        [Tags("Lecture: General")]
        public async Task<IActionResult> UpdateDers([FromHeader(Name = "Authorization")] string token, [FromBody] DersUpdateCommandRequestDTO body)
        {
            return await SendCommand(new DersUpdateCommand
            {
                Authorization = token,
                Ders = body
            });
        }

        [HttpDelete("lecture")]
        [Authorize]
        [Tags("Lecture: General")]
        public async Task<IActionResult> DeleteDers([FromHeader(Name = "Authorization")] string token, [FromBody] DersDeleteCommandRequestDTO body)
        {
            return await SendCommand(new DersDeleteCommand
            {
                Authorization = token,
                Ders = body
            });
        }
    }
}