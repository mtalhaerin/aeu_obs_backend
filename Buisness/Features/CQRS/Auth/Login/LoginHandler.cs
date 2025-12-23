using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.ValidationRules.FluentValidation.FieldValidators;
using Core.Buisiness.Features.CQRS;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.YetkiEntities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Auth.Login
{
    public class LoginCommand : ICommand<BaseResponse<LoginResponseDTO>>,
        IEmailFieldValidator,
        IPasswordFieldValidator
    {
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
    }

    public class LoginHandler : ICommandHandler<LoginCommand, BaseResponse<LoginResponseDTO>>
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;

        public LoginHandler(
            IKullaniciService kullaniciService,
            IYetkiService yetkiService,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager)
        {
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;

        }

        public async Task<BaseResponse<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                IDataResult<Kullanici?> kullanici = await _kullaniciService.GetByEmailAsync(request.Email!);
                if (!kullanici.Success)
                {
                    return BaseResponse<LoginResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }

                if (!HashingHelper.VerifyPasswordHash(request.Password!, kullanici.Data!.ParolaHash, kullanici.Data!.ParolaTuz))
                {
                    return BaseResponse<LoginResponseDTO>.Failure("Parola hatalı", statusCode: 401);
                }

                IDataResult<IEnumerable<KullaniciIslemYetkisi>>? islemYetkileri = await _yetkiService.GetKullaniciYetkileriAsync(kullanici.Data.KullaniciUuid);

                AccessToken token = _tokenHelper.CreateToken(kullanici.Data, islemYetkileri.Data);
                _tokenCacheManager.RegisterToken(token.Token, kullanici.Data.KullaniciUuid, token.ExpireInMinutes);

                var loginResponse = new LoginResponseDTO
                {
                    AccessToken = token.Token
                };

                return BaseResponse<LoginResponseDTO>.Success(loginResponse, "Giriş başarılı", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}