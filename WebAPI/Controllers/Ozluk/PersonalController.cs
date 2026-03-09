using Business.Concrete;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.CommandDTOs;
using Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs;
using Business.Features.CQRS.Ozluk.AdresHandlers.Command;
using Business.Features.CQRS.Ozluk.AdresHandlers.Query;
using Business.Features.CQRS.Ozluk.EmailHandlers.Command;
using Business.Features.CQRS.Ozluk.EmailHandlers.Query;
using Business.Features.CQRS.Ozluk.TelefonHandlers.Command;
using Business.Features.CQRS.Ozluk.TelefonHandlers.Query;
using Core.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.Ozluk
{


    [ApiController]
    [Route("api/[controller]")]
    public class PersonalController : GenericController
    {
        public PersonalController(
            IMediator mediator,
            ILogger<PersonalController> logger) : base(mediator, logger)
        {
        }

        #region Addres CRUD Operations
        // Multiple Addreses
        [HttpGet("addreses")]
        [Authorize]
        [Tags("Personal | Address")]
        public async Task<IActionResult> GetAddreses([FromHeader(Name = "Authorization")] string token, [FromQuery] Guid? kullaniciUuid)
        {
            return await SendQuery(new OzlukAdresesQuery
            {
                Authorization = token,
                KullaniciUuid = kullaniciUuid
            });
        }

        // Single Addres
        [HttpGet("addres")]
        [Authorize]
        [Tags("Personal | Address")]
        public async Task<IActionResult> GetAddres([FromHeader(Name = "Authorization")] string token, [FromQuery] OzlukAddresQueryRequestDTO body)
        {
            return await SendQuery(new OzlukAdresQuery
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                AddressUuid = body.AddressUuid
            });
        }

        [HttpPost("addres")]
        [Authorize]
        [Tags("Personal | Address")]
        public async Task<IActionResult> CreateAddres([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukAddresAddCommandRequestDTO body)
        {
            return await SendCommand(new OzlukAdresesAddCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                Sokak = body.Sokak,
                Sehir = body.Sehir,
                Ilce = body.Ilce,
                PostaKodu = body.PostaKodu,
                Ulke = body.Ulke,
                Oncelikli = body.Oncelikli
            });
        }

        [HttpPut("addres")]
        [Authorize]
        [Tags("Personal | Address")]
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
        [Tags("Personal | Address")]
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
        [HttpGet("emails")]
        [Authorize]
        [Tags("Personal | Email")]
        public async Task<IActionResult> GetEmails([FromHeader(Name = "Authorization")] string token, [FromQuery] Guid? kullaniciUuid)
        {
            return await SendQuery(new OzlukEmailsQuery
            {
                Authorization = token,
                KullaniciUuid = kullaniciUuid
            });
        }

        // Emails
        [HttpGet("email")]
        [Authorize]
        [Tags("Personal | Email")]
        public async Task<IActionResult> GetEmail([FromHeader(Name = "Authorization")] string token, [FromQuery] OzlukEmailQueryRequestDTO body)
        {
            return await SendQuery(new OzlukEmailQuery
            {
                Authorization = token,
                EmailUuid = body.EmailUuid,
                KullaniciUuid = body.KullaniciUuid
            });
        }

        [HttpPost("email")]
        [Authorize]
        [Tags("Personal | Email")]
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

        [HttpPut("email")]
        [Authorize]
        [Tags("Personal | Email")]
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
        [Tags("Personal | Email")]
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
        [HttpGet("phones")]
        [Authorize]
        [Tags("Personal | Phone")]
        public async Task<IActionResult> GetPhones([FromHeader(Name = "Authorization")] string token, [FromQuery] Guid? kullaniciUuid)
        {
            return await SendQuery(new OzlukPhonesQuery
            {
                Authorization = token,
                KullaniciUuid = kullaniciUuid
            });
        }

        [HttpGet("phone")]
        [Authorize]
        [Tags("Personal | Phone")]
        public async Task<IActionResult> GetPhone([FromHeader(Name = "Authorization")] string token, [FromQuery] Guid? kullaniciUuid, Guid? telefonUuid)
        {
            return await SendQuery(new OzlukPhoneQuery
            {
                Authorization = token,
                KullaniciUuid = kullaniciUuid,
                TelefonUuid = telefonUuid
            });
        }

        [HttpPost("phone")]
        [Authorize]
        [Tags("Personal | Phone")]
        public async Task<IActionResult> CreatePhone([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukPhoneAddCommandRequestDTO body)
        {
            return await SendCommand(new OzlukPhoneAddCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                UlkeKodu = body.UlkeKodu,
                TelefonNo = body.TelefonNo,
                TelefonTipi = body.TelefonTipi,
                Oncelikli = body.Oncelikli
            });
        }

        [HttpPut("phone")]
        [Authorize]
        [Tags("Personal | Phone")]
        public async Task<IActionResult> UpdatePhone([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukPhoneUpdateCommandRequestDTO body)
        {
            return await SendCommand(new OzlukPhoneUpdateCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                TelefonUuid = body.TelefonUuid,
                UlkeKodu = body.UlkeKodu,
                TelefonNo = body.TelefonNo,
                TelefonTipi = body.TelefonTipi,
                Oncelikli = body.Oncelikli
            });
        }
        
        [HttpDelete("phone")]
        [Authorize]
        [Tags("Personal | Phone")]
        public async Task<IActionResult> DeletePhone([FromHeader(Name = "Authorization")] string token, [FromBody] OzlukPhoneDeleteCommandRequestDTO body)
        {
            return await SendCommand(new OzlukPhoneDeleteCommand
            {
                Authorization = token,
                KullaniciUuid = body.KullaniciUuid,
                TelefonUuid = body.TelefonUuid
            });
        }
        #endregion
    }
}
