using Business.Concrete;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs;
using Business.Features.CQRS.Ozluk.AdresHandlers.Query;
using Business.Features.CQRS.Ozluk.EmailHandlers.Query;
using Business.Features.CQRS.Ozluk.TelefonHandlers.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.Ozluk
{


    [ApiController]
    [Route("api/[controller]")]
    public class OzlukController : GenericController
    {
        public OzlukController(
            IMediator mediator,
            ILogger<OzlukController> logger) : base(mediator, logger)
        {
        }

        // Single Addres
        [HttpGet("addres")]
        [Authorize]
        public async Task<IActionResult> GetAddres([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresQueryRequestDTO query)
        {
            return await SendQuery(new OzlukAdresQuery
            {
                Authorization = token,
                AddressUuid = query.AddressUuid
            });
        }

        // Multiple Addreses
        [HttpGet("addreses")]
        [Authorize]
        public async Task<IActionResult> GetAddreses([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresesQueryRequestDTO query)
        {
            return await SendQuery(new OzlukAdresesQuery
            {
                Authorization = token,
            });
        }


        // Emails
        [HttpGet("email")]
        [Authorize]
        public async Task<IActionResult> GetEmail([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukEmailQueryRequestDTO query)
        {
            return await SendQuery(new OzlukEmailQuery
            {
                Authorization = token,
                EmailUuid = query.EmailUuid
            });
        }

        [HttpGet("emails")]
        [Authorize]
        public async Task<IActionResult> GetEmails([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukEmailsQueryRequestDTO query)
        {
            return await SendQuery(new OzlukEmailsQuery
            {
                Authorization = token,
            });
        }

        [HttpGet("phone")]
        [Authorize]
        public async Task<IActionResult> GetPhone([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukPhoneQueryRequestDTO command)
        {
            return await SendQuery(new OzlukPhoneQuery
            {
                Authorization = token,
                TelefonUuid = command.TelefonUuid
            });
        }

        [HttpGet("phones")]
        [Authorize]
        public async Task<IActionResult> GetPhones([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukPhonesQueryRequestDTO command)
        {
            return await SendQuery(new OzlukPhonesQuery
            {
                Authorization = token,
            });
        }

    }
}
