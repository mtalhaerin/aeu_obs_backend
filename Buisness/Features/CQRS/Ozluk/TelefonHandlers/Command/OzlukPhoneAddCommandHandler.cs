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
    public class OzlukPhoneAddCommand : ISecuredCommand<BaseResponse<OzlukPhoneAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string UlkeKodu { get; set; } = string.Empty;
        public string TelefonNo { get; set; } = string.Empty;
        public TelefonTipi TelefonTipi { get; set; } = TelefonTipi.CEP;
        public bool Oncelikli { get; set; } = false;

    }
    public class OzlukPhoneAddCommandHandler : ICommandHandler<OzlukPhoneAddCommand, BaseResponse<OzlukPhoneAddCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITelefonService _telefonService;

        public OzlukPhoneAddCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            ITelefonService telefonService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _telefonService = telefonService;
        }

        public async Task<BaseResponse<OzlukPhoneAddCommandResponseDTO>> Handle(OzlukPhoneAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.KullaniciUuid == Guid.Empty)
                {
                    return BaseResponse<OzlukPhoneAddCommandResponseDTO>.Failure("Kullanici  UUID'si belirtilmemiş", statusCode: 400);
                }

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL && kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukPhoneAddCommandResponseDTO>.Failure("Başka kullancıya ait Telefon bilgisni yazma izniniz bulunamamkta.", statusCode: 401);

                Telefon newTelefon = new Telefon
                {
                    TelefonUuid = Guid.NewGuid(),
                    KullaniciUuid = request.KullaniciUuid,
                    UlkeKodu = request.UlkeKodu,
                    TelefonNo = request.TelefonNo,
                    TelefonTipi = request.TelefonTipi,
                    Oncelikli = request.Oncelikli,
                    OlusturmaTarihi = DateTime.UtcNow,
                    GuncellemeTarihi = DateTime.UtcNow
                };

                IDataResult<Telefon> result = await _telefonService.AddUserPhoneAsync(newTelefon);
                if (!result.Success)
                {
                    return BaseResponse<OzlukPhoneAddCommandResponseDTO>.Failure("Telefon ekleme işlemi başarısız oldu", statusCode: 500);
                }

                return BaseResponse<OzlukPhoneAddCommandResponseDTO>.Success(new OzlukPhoneAddCommandResponseDTO
                {
                    KullaniciUuid = request.KullaniciUuid,
                    TelefonUuid = result.Data.TelefonUuid,
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
