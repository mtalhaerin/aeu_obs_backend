using Business.Concrete;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs;
using Business.Features.CQRS.Ozluk.AdresHandlers.Command;
using Business.Features.CQRS.Ozluk.AdresHandlers.Query;
using Business.Features.CQRS.Ozluk.EmailHandlers.Command;
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
        public async Task<IActionResult> UpdateAddres([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresUpdateCommandRequestDTO body)
        {
            return await SendCommand(new OzlukAdresesUpdateCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                AdresUuid = body.AdresUuid,
                Sokak = body.Sokak,
                Sehir = body.Sehir,
                Ilce = body.Ilce,
                PostaKodu = body.PostaKodu,
                Ulke = body.Ulke,
                Oncelikli = body.Oncelikli
            });
        }

        [HttpDelete("addres")]
        [Authorize]
        [Tags("Ozluk: Address")]
        public async Task<IActionResult> DeleteAddres([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresDeleteCommandRequestDTO body)
        {
            return await SendCommand(new OzlukAdresesDeleteCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                AdresUuid = body.AdresUuid
            });
        }
        #endregion

        #region Email CRUD Operations
        [HttpPost("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> CreateEmail([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukEmailAddCommandRequestDTO body)
        {
            return await SendCommand(new OzlukEmailAddCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                EpostaAdresi = body.EpostaAdresi,
                EpostaTipi = body.EpostaTipi,
                Oncelikli = body.Oncelikli
            });
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
        public async Task<IActionResult> UpdateEmail([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukEmailUpdateCommandRequestDTO body)
        {
            return await SendCommand(new OzlukEmailUpdateCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                EpostaUuid = body.EpostaUuid,
                EpostaAdresi = body.EpostaAdresi,
                EpostaTipi = body.EpostaTipi,
                Oncelikli = body.Oncelikli
            });
        }

        [HttpDelete("email")]
        [Authorize]
        [Tags("Ozluk: Email")]
        public async Task<IActionResult> DeleteEmail([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukEmailDeleteCommandRequestDTO body)
        {
            return await SendCommand(new OzlukEmailDeleteCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                EpostaUuid = body.EpostaUuid
            });
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
