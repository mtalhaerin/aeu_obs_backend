using Business.Concrete;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs;
using Business.Features.CQRS.Ozluk.AdresHandlers.Command;
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

        #region Addres CRUD Operations
        [HttpPost("addres")]
        [Authorize]
        [Tags("Ozluk: Address")]
        public async Task<IActionResult> CreateAddres([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresAddCommandRequestDTO body)
        {
            return await SendCommand(new OzlukAdresesAddCommand
            {
                Authorization = token,
                Sokak =  body.Sokak,
                Sehir = body.Sehir,
                Ilce = body.Ilce,
                PostaKodu = body.PostaKodu,
                Ulke = body.Ulke,
                Oncelikli = body.Oncelikli
            });
        }

        // Single Addres
        [HttpGet("addres")]
        [Authorize]
        [Tags("Ozluk: Address")]
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
        [Tags("Ozluk: Address")]
        public async Task<IActionResult> GetAddreses([FromHeader(Name = "Authorization")] string token)
        {
            return await SendQuery(new OzlukAdresesQuery
            {
                Authorization = token,
            });
        }

        [HttpPut("addres")]
        [Authorize]
        [Tags("Ozluk: Address")]
        public async Task<IActionResult> UpdateAddres()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }

        [HttpDelete("addres")]
        [Authorize]
        [Tags("Ozluk: Address")]
        public async Task<IActionResult> DeleteAddres()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }
        #endregion

        #region Email CRUD Operations
        [HttpPost("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> CreateEmail()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }

        // Emails
        [HttpGet("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
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
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> GetEmails([FromHeader(Name = "Authorization")] string token)
        {
            return await SendQuery(new OzlukEmailsQuery
            {
                Authorization = token,
            });
        }

        [HttpPut("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> UpdateEmail()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }

        [HttpDelete("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> DeleteEmail()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }
        #endregion


        #region Phone CRUD Operations
        [HttpPost("phone")]
        [Authorize]
        [Tags("Ozluk: Phone")]
        public async Task<IActionResult> CreatePhone()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }

        [HttpGet("phone")]
        [Authorize]
        [Tags("Ozluk: Phone")]
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
        [Tags("Ozluk: Phone")]
        public async Task<IActionResult> GetPhones([FromHeader(Name = "Authorization")] string token)
        {
            return await SendQuery(new OzlukPhonesQuery
            {
                Authorization = token,
            });
        }

        [HttpPut("phone")]
        [Authorize]
        [Tags("Ozluk: Phone")]
        public async Task<IActionResult> UpdatePhone()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }
        
        [HttpDelete("phone")]
        [Authorize]
        [Tags("Ozluk: Phone")]
        public async Task<IActionResult> DeletePhone()
        {
            return await Task.FromResult(Ok("Not implemented yet"));
        }
        #endregion
    }
}
