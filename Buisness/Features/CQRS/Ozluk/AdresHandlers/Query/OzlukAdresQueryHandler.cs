using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.DTOs.ResponseDTOs.OzlukDTOs.OzlukQueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
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
    public class OzlukAdresQuery : ISecuredQuery<BaseResponse<OzlukAdresQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid? AddressUuid { get; set; } = null;
    }

    public class OzlukAdresQueryHandler : IQueryHandler<OzlukAdresQuery, BaseResponse<OzlukAdresQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IAdresService _adresService;

        public OzlukAdresQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IAdresService adresService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _adresService = adresService;
        }

        public async Task<BaseResponse<OzlukAdresQueryResponseDTO>> Handle(OzlukAdresQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.AddressUuid == null || request.AddressUuid == Guid.Empty)
                {
                    return BaseResponse<OzlukAdresQueryResponseDTO>.Failure("Adres UUID'si belirtilmemiş", statusCode: 400);
                }
                
                IDataResult<Adres> adres = await _adresService.GetUserAddresByUuidAsync(kullanici.KullaniciUuid, request.AddressUuid);
                
                if (!adres.Success)
                {
                    return BaseResponse<OzlukAdresQueryResponseDTO>.Failure("Adres bulunamadı", statusCode: 404);
                }

                var profileResponse = new OzlukAdresQueryResponseDTO
                {
                    AdresUuid = adres.Data.AdresUuid,
                    KullaniciUuid = adres.Data.KullaniciUuid,
                    Sokak = adres.Data.Sokak,
                    Sehir = adres.Data.Sehir,
                    Ilce = adres.Data.Ilce,
                    PostaKodu = adres.Data.PostaKodu,
                    Ulke = adres.Data.Ulke,
                    Oncelikli = adres.Data.Oncelikli,
                };


                return BaseResponse<OzlukAdresQueryResponseDTO>.Success(profileResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
