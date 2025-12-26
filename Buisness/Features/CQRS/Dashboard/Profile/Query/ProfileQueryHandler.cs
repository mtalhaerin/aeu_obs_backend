using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.YetkiEntities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Dashboard.Profile.Query
{
    public class ProfileQuery : ISecuredQuery<BaseResponse<ProfileQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class ProfileQueryHandler : IQueryHandler<ProfileQuery, BaseResponse<ProfileQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;

        public ProfileQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager,
            IKullaniciService kullaniciService,
            IYetkiService yetkiService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
        }

        public async Task<BaseResponse<ProfileQueryResponseDTO>> Handle(ProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                var profileResponse = new ProfileQueryResponseDTO
                {
                    KullaniciUuid = kullanici.KullaniciUuid,
                    KullaniciTipi = kullanici.KullaniciTipi,
                    Ad = kullanici.Ad,
                    OrtaAd = kullanici.OrtaAd,
                    Soyad = kullanici.Soyad,
                    KurumEposta = kullanici.KurumEposta,
                    KurumSicilNo = kullanici.KurumSicilNo,
                };

                return BaseResponse<ProfileQueryResponseDTO>.Success(profileResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
