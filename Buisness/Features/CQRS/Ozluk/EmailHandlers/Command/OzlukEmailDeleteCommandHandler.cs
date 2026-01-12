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
    public class OzlukEmailDeleteCommand : ISecuredCommand<BaseResponse<OzlukEmailDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid EpostaUuid { get; set; } = Guid.Empty;
    }
    public class OzlukEmailDeleteCommandHandler : ICommandHandler<OzlukEmailDeleteCommand, BaseResponse<OzlukEmailDeleteCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IEpostaService _emailService;
        public OzlukEmailDeleteCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IEpostaService EmailService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _emailService = EmailService;
        }

        public async Task<BaseResponse<OzlukEmailDeleteCommandResponseDTO>> Handle(OzlukEmailDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if ((kullanici.KullaniciTipi != KullaniciTipi.PERSONEL) &&
                    (kullanici.KullaniciUuid != request.KullaniciUuid))
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<IEnumerable<Eposta>> emails = await _emailService.GetUserEmailsAsync(kullanici.KullaniciUuid);

                bool haveOncelikli = emails.Data.Any(Email => Email.Oncelikli);

                if (emails.Data.Count() == 0)
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("Herhangi bir email bulunamadı.", statusCode: 404);

                if (emails.Data.Count() == 1)
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("En az bir email adresinizin olması gerekmekte. Email adresinizi silme işlemi gerçekleştirmeden önce bir email adres ekleyiniz.", statusCode: 409);

                Eposta? emailToDelete = emails.Data.FirstOrDefault(a => a.EpostaUuid == request.EpostaUuid);
                Eposta? newPrimaryEmail = emails.Data
                    .Where(a => !a.Oncelikli && a.EpostaUuid != request.EpostaUuid)
                    .OrderByDescending(a => a.OlusturmaTarihi)
                    .FirstOrDefault();

                if (emailToDelete == null)
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("Silinmek istenen adres bulunamadı.", statusCode: 404);


                IResult setPrimaryResult = await _emailService.SetPrimaryEmail(newPrimaryEmail);
                if (!setPrimaryResult.Success)
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("Yeni öncelikli adres ayarlanamadı.", statusCode: 500);

                IResult deleteResult = await _emailService.DeleteByEmailUuidAsync(emailToDelete.EpostaUuid);
                if (!deleteResult.Success)
                    return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Failure("Adres silinemedi", statusCode: 500);

                var emialDeleteResponse = new OzlukEmailDeleteCommandResponseDTO
                {
                    KullaniciUuid = emailToDelete.KullaniciUuid,
                    EpostaUuid = emailToDelete.EpostaUuid
                };

                return BaseResponse<OzlukEmailDeleteCommandResponseDTO>.Success(emialDeleteResponse, "Adres başarıyla silindi", 201);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
