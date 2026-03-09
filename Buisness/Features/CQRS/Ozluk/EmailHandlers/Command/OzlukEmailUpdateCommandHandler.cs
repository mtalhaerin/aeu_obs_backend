using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
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
    public class OzlukEmailUpdateCommand : ISecuredCommand<BaseResponse<OzlukEmailUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid EpostaUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
    }
    public class OzlukEmailUpdateCommandHandler : ICommandHandler<OzlukEmailUpdateCommand, BaseResponse<OzlukEmailUpdateCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IEpostaService _emailService;
        public OzlukEmailUpdateCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IEpostaService EmailService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _emailService = EmailService;
        }

        public async Task<BaseResponse<OzlukEmailUpdateCommandResponseDTO>> Handle(OzlukEmailUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL &&
                    kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukEmailUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<IEnumerable<Eposta>> emails = await _emailService.GetUserEmailsAsync(request.KullaniciUuid);

                Eposta? emailToUpdate = emails.Data.FirstOrDefault(a => a.EpostaUuid == request.EpostaUuid);

                if (emailToUpdate == null)
                    return BaseResponse<OzlukEmailUpdateCommandResponseDTO>.Failure("Güncellenecek email bulunamadı.", statusCode: 404);

                emailToUpdate.EpostaAdresi = request.EpostaAdresi;
                emailToUpdate.EpostaTipi = request.EpostaTipi;
                emailToUpdate.Oncelikli = request.Oncelikli;

                IDataResult<Eposta>? updatedEmailResult = null;
                if (request.Oncelikli)
                    updatedEmailResult = await _emailService.ResetPrimaryEmail(emailToUpdate);
                else
                    updatedEmailResult = await _emailService.ResetNonPrimaryEmail(emailToUpdate);

                if (!updatedEmailResult.Success)
                    return BaseResponse<OzlukEmailUpdateCommandResponseDTO>.Failure("Email güncelleme işlemi başarısız.", statusCode: 500);

                var emailUpdateResponse = new OzlukEmailUpdateCommandResponseDTO
                {
                    EpostaUuid = emailToUpdate.EpostaUuid,
                    KullaniciUuid = emailToUpdate.KullaniciUuid,
                    EpostaAdresi = emailToUpdate.EpostaAdresi,
                    EpostaTipi = emailToUpdate.EpostaTipi,
                    Oncelikli = emailToUpdate.Oncelikli
                };
                return BaseResponse<OzlukEmailUpdateCommandResponseDTO>.Success(emailUpdateResponse, "Email başarıyla güncellendi", 200);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
