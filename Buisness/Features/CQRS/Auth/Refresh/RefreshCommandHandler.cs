using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs.Command;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Core.Buisiness.Features.CQRS;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.YetkiEntities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Auth.Refresh
{
    public class RefreshCommand : ICommand<BaseResponse<RefreshCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class RefreshCommandHandler : ICommandHandler<RefreshCommand, BaseResponse<RefreshCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;

        public RefreshCommandHandler(
            IGenericHelper genericHelper,
            IKullaniciService kullaniciService,
            IYetkiService yetkiService,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager)
        {
            _genericHelper = genericHelper;
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;
        }

        public async Task<BaseResponse<RefreshCommandResponseDTO>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var token = _genericHelper.GetAccessTokenFromHeader();
                if (string.IsNullOrEmpty(token))
                {
                    return BaseResponse<RefreshCommandResponseDTO>.Failure("Token bulunamadı", statusCode: 400);
                }

                Guid userUuid = _tokenCacheManager.ValidateToken(token);
                if (userUuid == Guid.Empty)
                {
                    return BaseResponse<RefreshCommandResponseDTO>.Failure("Token geçersiz veya süresi dolmuş", statusCode: 401);
                }

                IDataResult<Kullanici?> kullanici = await _kullaniciService.GetByUuidAsync(userUuid);

                if (!kullanici.Success)
                {
                    return BaseResponse<RefreshCommandResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }

                _tokenCacheManager.BlacklistToken(token);

                IDataResult<IEnumerable<KullaniciIslemYetkisi>>? islemYetkileri = await _yetkiService.GetKullaniciYetkileriAsync(kullanici.Data.KullaniciUuid);
                AccessToken newAccessToken = _tokenHelper.CreateToken(kullanici.Data, islemYetkileri.Data);

                _tokenCacheManager.RegisterToken(newAccessToken.Token, kullanici.Data.KullaniciUuid, newAccessToken.ExpireInMinutes);

                RefreshCommandResponseDTO refreshResponse = new()
                {
                    AccessToken = newAccessToken.Token,
                };

                return BaseResponse<RefreshCommandResponseDTO>.Success(refreshResponse, "Giriş başarılı", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
