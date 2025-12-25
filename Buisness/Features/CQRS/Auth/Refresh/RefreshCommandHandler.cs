using Business.Concrete;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.AuthDTOs.Command;
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
    public class RefreshCommand : ISecuredCommand<BaseResponse<RefreshCommandResponseDTO>>
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
            IUserContext userContext,
            IKullaniciService kullaniciService,
            IYetkiService yetkiService,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;
        }

        public async Task<BaseResponse<RefreshCommandResponseDTO>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici= _userContext.CurrentUser;


                _tokenCacheManager.BlacklistToken(token);

                //IDataResult<IEnumerable<KullaniciIslemYetkisi>>? islemYetkileri = await _yetkiService.GetKullaniciYetkileriAsync(kullanici.Data.KullaniciUuid);
                AccessToken newAccessToken = _tokenHelper.CreateToken(kullanici, null);

                _tokenCacheManager.RegisterToken(newAccessToken.Token, kullanici.KullaniciUuid, newAccessToken.ExpireInMinutes);

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
