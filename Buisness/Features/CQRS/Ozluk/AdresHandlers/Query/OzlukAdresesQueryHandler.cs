using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Concrete.OzlukEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Ozluk.AdresHandlers.Query
{
    public class OzlukAdresesQuery : ISecuredQuery<BaseResponse<OzlukAdresesQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid? KullaniciUuid { get; set; } = Guid.Empty;
    }

    public class OzlukAdresesQueryHandler : IQueryHandler<OzlukAdresesQuery, BaseResponse<OzlukAdresesQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IAdresService _adresService;

        public OzlukAdresesQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IAdresService adresService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _adresService = adresService;
        }

        public async Task<BaseResponse<OzlukAdresesQueryResponseDTO>> Handle(OzlukAdresesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.KullaniciUuid == null || request.KullaniciUuid == Guid.Empty)
                    return BaseResponse<OzlukAdresesQueryResponseDTO>.Failure("Kullanıcı UUID'si belirtilmemiş", statusCode: 400);

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL &&
    kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukAdresesQueryResponseDTO>.Failure("Başka kullancıya ait adres bilgisni okuma izniniz bulunamamkta.", statusCode: 401);

                if (!(request.KullaniciUuid != null && request.KullaniciUuid != Guid.Empty))
                {
                    return BaseResponse<OzlukAdresesQueryResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }
                IDataResult<IEnumerable<Adres>> adreses = await _adresService.GetUserAddresesAsync(request.KullaniciUuid.Value);

                if (!adreses.Success)
                {
                    return BaseResponse<OzlukAdresesQueryResponseDTO>.Failure("Adres bulunamadı", statusCode: 404);
                }

                var profileResponse = new OzlukAdresesQueryResponseDTO
                {
                    Addreses = adreses.Data.Select(adres => new OzlukAdresQueryResponseDTO
                    {
                        AdresUuid = adres.AdresUuid,
                        KullaniciUuid = adres.KullaniciUuid,
                        Sokak = adres.Sokak,
                        Sehir = adres.Sehir,
                        Ilce = adres.Ilce,
                        PostaKodu = adres.PostaKodu,
                        Ulke = adres.Ulke,
                        Oncelikli = adres.Oncelikli,
                    }).ToList()
                };


                return BaseResponse<OzlukAdresesQueryResponseDTO>.Success(profileResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
