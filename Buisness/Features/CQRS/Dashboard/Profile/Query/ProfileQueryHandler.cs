using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
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
    public class ProfileQuery : IQuery<BaseResponse<ProfileQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class ProfileQueryHandler : IQueryHandler<ProfileQuery, BaseResponse<ProfileQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;

        public ProfileQueryHandler(
            IGenericHelper genericHelper,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager,
            IKullaniciService kullaniciService,
            IYetkiService yetkiService)
        {
            _genericHelper = genericHelper;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
        }

        public async Task<BaseResponse<ProfileQueryResponseDTO>> Handle(ProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var token = _genericHelper.GetAccessTokenFromHeader();
                if (string.IsNullOrEmpty(token))
                {
                    return BaseResponse<ProfileQueryResponseDTO>.Failure("Token bulunamadı", statusCode: 400);
                }

                Guid userUuid = _tokenCacheManager.ValidateToken(token);
                if (userUuid == Guid.Empty)
                {
                    return BaseResponse<ProfileQueryResponseDTO>.Failure("Token geçersiz veya süresi dolmuş", statusCode: 401);
                }

                IDataResult<Kullanici?> kullanici = await _kullaniciService.GetByUuidAsync(userUuid);
                if (!kullanici.Success)
                {
                    return BaseResponse<ProfileQueryResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }

                var profileResponse = new ProfileQueryResponseDTO
                {
                    KullaniciUuid = kullanici.Data!.KullaniciUuid,
                    KullaniciTipi = kullanici.Data.KullaniciTipi,
                    Ad = kullanici.Data.Ad,
                    OrtaAd = kullanici.Data.OrtaAd,
                    Soyad = kullanici.Data.Soyad,
                    KurumEposta = kullanici.Data.KurumEposta,
                    KurumSicilNo = kullanici.Data.KurumSicilNo,
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
