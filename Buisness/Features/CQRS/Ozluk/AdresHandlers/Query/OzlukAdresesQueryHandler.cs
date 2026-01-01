using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Entities.Concrete.OzlukEntities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Features.CQRS._Generic.Secured;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;

namespace Business.Features.CQRS.Ozluk.AdresHandlers.Query
{
    public class OzlukAdresesQuery : ISecuredQuery<BaseResponse<OzlukAdresesQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
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
                
                IDataResult<IEnumerable<Adres>> adreses = await _adresService.GetUserAddresesAsync(kullanici.KullaniciUuid);
                
                if (!adreses.Success)
                {
                    return BaseResponse<OzlukAdresesQueryResponseDTO>.Failure("Adres bulunamadı", statusCode: 404);
                }

                var profileResponse = new OzlukAdresesQueryResponseDTO { 
                    Addreses =  adreses.Data.Select(adres => new OzlukAdresQueryResponseDTO
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
