using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Ozluk.EmailHandlers.Command
{
    public class OzlukEmailAddCommand : ISecuredCommand<BaseResponse<OzlukEmailAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
    }
    public class OzlukEmailAddCommandHandler : ICommandHandler<OzlukEmailAddCommand, BaseResponse<OzlukEmailAddCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IEpostaService _emailService;
        public OzlukEmailAddCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IEpostaService EmailService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _emailService = EmailService;
        }

        public async Task<BaseResponse<OzlukEmailAddCommandResponseDTO>> Handle(OzlukEmailAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL &&
                    kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukEmailAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<IEnumerable<Eposta>> Emailes = await _emailService.GetUserEmailsAsync(kullanici.KullaniciUuid);

                bool haveOncelikli = Emailes.Data.Any(Email => Email.Oncelikli);

                if (haveOncelikli && request.Oncelikli)
                    return BaseResponse<OzlukEmailAddCommandResponseDTO>.Failure("Zaten öncelikli bir Emailiniz bulunmakta", statusCode: 409);


                Eposta newEmail = new Eposta
                {
                    EpostaUuid = Guid.NewGuid(),
                    KullaniciUuid = request.KullaniciUuid,
                    EpostaAdresi = request.EpostaAdresi,
                    EpostaTipi = request.EpostaTipi,
                    Oncelikli = request.Oncelikli
                };

                if (!haveOncelikli || Emailes.Data.Count() == 0)
                    newEmail.Oncelikli = true;

                var addedEmail = await _emailService.AddEmailAsync(newEmail);
                if (!addedEmail.Success)
                    return BaseResponse<OzlukEmailAddCommandResponseDTO>.Failure("Email eklenemedi", statusCode: 500);
                
                
                var emailResponse = new OzlukEmailAddCommandResponseDTO
                {
                    EpostaUuid = addedEmail.Data.EpostaUuid,
                    KullaniciUuid = addedEmail.Data.KullaniciUuid,
                    EpostaAdresi = addedEmail.Data.EpostaAdresi,
                    EpostaTipi = addedEmail.Data.EpostaTipi,
                    Oncelikli = addedEmail.Data.Oncelikli
                };
                return BaseResponse<OzlukEmailAddCommandResponseDTO>.Success(emailResponse, "Email başarıyla eklendi", 201);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}