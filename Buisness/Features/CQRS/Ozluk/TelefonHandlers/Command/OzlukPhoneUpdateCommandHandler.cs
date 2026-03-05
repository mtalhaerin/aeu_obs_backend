using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.OzlukDTOs.PhoneDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.OzlukEntities;

namespace Business.Features.CQRS.Ozluk.TelefonHandlers.Command
{
    public class OzlukPhoneUpdateCommand : ISecuredCommand<BaseResponse<OzlukPhoneUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid TelefonUuid { get; set; } = Guid.Empty;
        public string UlkeKodu { get; set; } = string.Empty;
        public string TelefonNo { get; set; } = string.Empty;
        public TelefonTipi TelefonTipi { get; set; } = TelefonTipi.CEP;
        public bool Oncelikli { get; set; } = false;

    }
    public class OzlukPhoneUpdateCommandHandler : ICommandHandler<OzlukPhoneUpdateCommand, BaseResponse<OzlukPhoneUpdateCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITelefonService _telefonService;

        public OzlukPhoneUpdateCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            ITelefonService telefonService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _telefonService = telefonService;
        }

        public async Task<BaseResponse<OzlukPhoneUpdateCommandResponseDTO>> Handle(OzlukPhoneUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.KullaniciUuid == Guid.Empty || request.TelefonUuid == Guid.Empty)
                {
                    return BaseResponse<OzlukPhoneUpdateCommandResponseDTO>.Failure("Kullanici  UUID'si belirtilmemiş", statusCode: 400);
                }

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL && kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukPhoneUpdateCommandResponseDTO>.Failure("Başka kullancıya ait Telefon bilgisni yazma izniniz bulunamamkta.", statusCode: 401);

                IDataResult<Telefon> existingTelefonResult = await _telefonService.GetUserTelefonByUuidAsync(request.KullaniciUuid, request.TelefonUuid);

                if (!existingTelefonResult.Success || existingTelefonResult == null)
                {
                    return BaseResponse<OzlukPhoneUpdateCommandResponseDTO>.Failure("Telefon bulunamadı", statusCode: 404);
                }


                existingTelefonResult.Data.UlkeKodu = request.UlkeKodu;
                existingTelefonResult.Data.TelefonNo = request.TelefonNo;
                existingTelefonResult.Data.TelefonTipi = request.TelefonTipi;
                existingTelefonResult.Data.Oncelikli = request.Oncelikli;
                existingTelefonResult.Data.OlusturmaTarihi = DateTime.UtcNow;
                existingTelefonResult.Data.GuncellemeTarihi = DateTime.UtcNow;

                IDataResult<Telefon?> result = await _telefonService.UpdateUserPhoneAsync(existingTelefonResult.Data);
                if (!result.Success)
                {
                    return BaseResponse<OzlukPhoneUpdateCommandResponseDTO>.Failure("Telefon ekleme işlemi başarısız oldu", statusCode: 500);
                }

                return BaseResponse<OzlukPhoneUpdateCommandResponseDTO>.Success(new OzlukPhoneUpdateCommandResponseDTO
                {
                    KullaniciUuid = request.KullaniciUuid,
                    TelefonUuid = result.Data!.TelefonUuid,
                    UlkeKodu = result.Data.UlkeKodu,
                    TelefonNo = result.Data.TelefonNo,
                    TelefonTipi = result.Data.TelefonTipi,
                    Oncelikli = result.Data.Oncelikli
                }, "Telefon başarıyla silindi", statusCode: 200);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
