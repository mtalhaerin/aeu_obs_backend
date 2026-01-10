using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.OzlukDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Business.Features.CQRS.Ozluk.AdresHandlers.Query;
using Core.Entities.Concrete.OzlukEntities;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Ozluk.AdresHandlers.Command
{
    public class OzlukAdresesAddCommand : ISecuredCommand<BaseResponse<OzlukAdresAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string Sokak { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string Ilce { get; set; } = string.Empty;
        public string PostaKodu { get; set; } = string.Empty;
        public string Ulke { get; set; } = string.Empty;
        public bool Oncelikli { get; set; } = false;
    }
    public class OzlukAdresAddCommandHandler : ICommandHandler<OzlukAdresesAddCommand, BaseResponse<OzlukAdresAddCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IAdresService _adresService;
        public OzlukAdresAddCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IAdresService adresService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _adresService = adresService;
        }

        public async Task<BaseResponse<OzlukAdresAddCommandResponseDTO>> Handle(OzlukAdresesAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                IDataResult<IEnumerable<Adres>> adreses = await _adresService.GetUserAddresesAsync(kullanici.KullaniciUuid);

                bool haveOncelikli = adreses.Data.Any(adres => adres.Oncelikli);

                if (haveOncelikli && request.Oncelikli)
                    return BaseResponse<OzlukAdresAddCommandResponseDTO>.Failure("Zaten öncelikli bir adresininz bulunmakta", statusCode: 409);


                Adres newAdres = new Adres
                {
                    AdresUuid = Guid.NewGuid(),
                    KullaniciUuid = kullanici.KullaniciUuid,
                    Sokak = request.Sokak,
                    Sehir = request.Sehir,
                    Ilce = request.Ilce,
                    PostaKodu = request.PostaKodu,
                    Ulke = request.Ulke,
                    Oncelikli = request.Oncelikli
                };
                
                var addedAdres = await _adresService.AddAdresAsync(newAdres);
                if (!addedAdres.Success)
                {
                    return BaseResponse<OzlukAdresAddCommandResponseDTO>.Failure("Adres eklenemedi", statusCode: 500);
                }
                var adresResponse = new OzlukAdresAddCommandResponseDTO
                {
                    AdresUuid = addedAdres.Data.AdresUuid,
                    KullaniciUuid = addedAdres.Data.KullaniciUuid,
                    Sokak = addedAdres.Data.Sokak,
                    Sehir = addedAdres.Data.Sehir,
                    Ilce = addedAdres.Data.Ilce,
                    PostaKodu = addedAdres.Data.PostaKodu,
                    Ulke = addedAdres.Data.Ulke,
                    Oncelikli = addedAdres.Data.Oncelikli
                };
                return BaseResponse<OzlukAdresAddCommandResponseDTO>.Success(adresResponse, "Adres başarıyla eklendi", 201);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}



