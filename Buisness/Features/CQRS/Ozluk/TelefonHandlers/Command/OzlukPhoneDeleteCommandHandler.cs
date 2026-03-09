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
    public class OzlukPhoneDeleteCommand : ISecuredCommand<BaseResponse<OzlukPhoneDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid TelefonUuid { get; set; } = Guid.Empty;
    }
    public class OzlukPhoneDeleteCommandHandler : ICommandHandler<OzlukPhoneDeleteCommand, BaseResponse<OzlukPhoneDeleteCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITelefonService _telefonService;

        public OzlukPhoneDeleteCommandHandler(
            IGenericHelper genericHelper, 
            IUserContext userContext, 
            ITelefonService telefonService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _telefonService = telefonService;
        }

        public async Task<BaseResponse<OzlukPhoneDeleteCommandResponseDTO>> Handle(OzlukPhoneDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.TelefonUuid == Guid.Empty || request.KullaniciUuid == Guid.Empty)
                {
                    return BaseResponse<OzlukPhoneDeleteCommandResponseDTO>.Failure("Kullanici veya Telefon UUID'si belirtilmemiş", statusCode: 400);
                }

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL && kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukPhoneDeleteCommandResponseDTO>.Failure("Başka kullancıya ait Telefon bilgisni okuma izniniz bulunamamkta.", statusCode: 401);

                IDataResult<Telefon> Phone = await _telefonService.GetUserTelefonByUuidAsync(request.KullaniciUuid, request.TelefonUuid);

                if (!Phone.Success)
                {
                    return BaseResponse<OzlukPhoneDeleteCommandResponseDTO>.Failure("Telefon bulunamadı", statusCode: 404);
                }

                IResult result = await _telefonService.DeleteUserPhoneAsync(request.KullaniciUuid, request.TelefonUuid);
                if (!result.Success)
                {
                    return BaseResponse<OzlukPhoneDeleteCommandResponseDTO>.Failure("Telefon silme işlemi başarısız oldu", statusCode: 500);
                }

                return BaseResponse<OzlukPhoneDeleteCommandResponseDTO>.Success(new OzlukPhoneDeleteCommandResponseDTO
                {
                    TelefonUuid = request.TelefonUuid,
                    KullaniciUuid = request.KullaniciUuid
                }, "Telefon başarıyla silindi", statusCode: 200);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
