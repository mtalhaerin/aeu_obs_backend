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
    public class OzlukAdresesUpdateCommand : ISecuredCommand<BaseResponse<OzlukAdresUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid AdresUuid { get; set; } = Guid.Empty;
        public string Sokak { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string Ilce { get; set; } = string.Empty;
        public string PostaKodu { get; set; } = string.Empty;
        public string Ulke { get; set; } = string.Empty;
        public bool Oncelikli { get; set; } = false;
    }
    public class OzlukAdresUpdateCommandHandler : ICommandHandler<OzlukAdresesUpdateCommand, BaseResponse<OzlukAdresUpdateCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IAdresService _adresService;
        public OzlukAdresUpdateCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IAdresService adresService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _adresService = adresService;
        }

        public async Task<BaseResponse<OzlukAdresUpdateCommandResponseDTO>> Handle(OzlukAdresesUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL &&
                    kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukAdresUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<IEnumerable<Adres>> addreses = await _adresService.GetUserAddresesAsync(kullanici.KullaniciUuid);

                Adres? addressToUpdate = addreses.Data.FirstOrDefault(a => a.AdresUuid == request.AdresUuid);

                if (addressToUpdate == null)
                    return BaseResponse<OzlukAdresUpdateCommandResponseDTO>.Failure("Güncellenecek adres bulunamadı.", statusCode: 404);

                addressToUpdate.Sokak = request.Sokak;
                addressToUpdate.Sehir = request.Sehir;
                addressToUpdate.Ilce = request.Ilce;
                addressToUpdate.PostaKodu = request.PostaKodu;
                addressToUpdate.Ulke = request.Ulke;
                addressToUpdate.Oncelikli = request.Oncelikli;

                IDataResult<Adres>? updatedAddressResult = null;
                if (request.Oncelikli)
                    updatedAddressResult = await _adresService.ResetPrimaryAddress(addressToUpdate);
                else
                    updatedAddressResult = await _adresService.ResetNonPrimaryAddress(addressToUpdate);

                if (!updatedAddressResult.Success)
                    return BaseResponse<OzlukAdresUpdateCommandResponseDTO>.Failure("Adres güncelleme işlemi başarısız.", statusCode: 500);

                var addressUpdateResponse = new OzlukAdresUpdateCommandResponseDTO
                {
                    AdresUuid = updatedAddressResult.Data.AdresUuid,
                    KullaniciUuid = updatedAddressResult.Data.KullaniciUuid,
                    Sokak = updatedAddressResult.Data.Sokak,
                    Sehir = updatedAddressResult.Data.Sehir,
                    Ilce = updatedAddressResult.Data.Ilce,
                    PostaKodu = updatedAddressResult.Data.PostaKodu,
                    Ulke = updatedAddressResult.Data.Ulke,
                    Oncelikli = updatedAddressResult.Data.Oncelikli
                };
                return BaseResponse<OzlukAdresUpdateCommandResponseDTO>.Success(addressUpdateResponse, "Adres başarıyla güncellendi", 200);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
