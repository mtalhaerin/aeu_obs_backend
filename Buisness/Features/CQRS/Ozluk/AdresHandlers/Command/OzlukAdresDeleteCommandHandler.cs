using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.OzlukEntities;

namespace Business.Features.CQRS.Ozluk.AdresHandlers.Command
{
    public class OzlukAdresesDeleteCommand : ISecuredCommand<BaseResponse<OzlukAdresDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid OzlukAdresUuid { get; set; } =  Guid.Empty;
    }
    public class OzlukAdresDeleteCommandHandler : ICommandHandler<OzlukAdresesDeleteCommand, BaseResponse<OzlukAdresDeleteCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IAdresService _adresService;
        public OzlukAdresDeleteCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IAdresService adresService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _adresService = adresService;
        }
        public async Task<BaseResponse<OzlukAdresDeleteCommandResponseDTO>> Handle(OzlukAdresesDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if ((kullanici.KullaniciTipi != KullaniciTipi.PERSONEL) &&
                    (kullanici.KullaniciUuid != request.KullaniciUuid))
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<IEnumerable<Adres>> addreses = await _adresService.GetUserAddresesAsync(kullanici.KullaniciUuid);

                bool haveOncelikli = addreses.Data.Any(adres => adres.Oncelikli);

                if (addreses.Data.Count() == 0)
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("Herhangi bir adres bulunamadı.", statusCode: 404);

                if(addreses.Data.Count() == 1)
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("En az bir adresinizin olması gerekmekte. Adresinizi silme işlemi gerçekleştirmeden  önce bir adres ekleyiniz.", statusCode: 409);

                Adres? addresToDelete = addreses.Data.FirstOrDefault(a => a.AdresUuid == request.OzlukAdresUuid);
                Adres? newPrimaryAddres = addreses.Data
                    .Where(a => !a.Oncelikli && a.AdresUuid != request.OzlukAdresUuid)
                    .OrderByDescending(a => a.OlusturmaTarihi)
                    .FirstOrDefault();
                if (addresToDelete == null)
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("Silinmek istenen adres bulunamadı.", statusCode: 404);

                IResult setPrimaryResult = await _adresService.SetPrimaryAddres(newPrimaryAddres);
                if (!setPrimaryResult.Success)
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("Yeni öncelikli adres ayarlanamadı.", statusCode: 500);

                IResult deleteResult = await _adresService.DeleteByAddresUuidAsync(addresToDelete.AdresUuid);
                if (!deleteResult.Success)
                    return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Failure("Adres silinemedi", statusCode: 500);

                var addressDeleteResponse = new OzlukAdresDeleteCommandResponseDTO
                {
                    AdresUuid = addresToDelete.AdresUuid,
                    KullaniciUuid = addresToDelete.KullaniciUuid
                };
                
                return BaseResponse<OzlukAdresDeleteCommandResponseDTO>.Success(addressDeleteResponse, "Adres başarıyla silindi", 201);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
