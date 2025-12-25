using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs.Command;
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
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Auth.Login
{
    public class LoginCommand : ICommand<BaseResponse<LoginCommandResponseDTO>>,
        IEmailFieldValidator,
        IPasswordFieldValidator
    {
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, BaseResponse<LoginCommandResponseDTO>>
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;

        public LoginCommandHandler(
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

        public async Task<BaseResponse<LoginCommandResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                IDataResult<Kullanici?> kullanici = await _kullaniciService.GetByEmailAsync(request.Email!);
                if (!kullanici.Success)
                {
                    return BaseResponse<LoginCommandResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }

                if (!HashingHelper.VerifyPasswordHash(request.Password!, kullanici.Data!.ParolaHash, kullanici.Data!.ParolaTuz))
                {
                    return BaseResponse<LoginCommandResponseDTO>.Failure("Parola hatalı", statusCode: 401);
                }

                //IDataResult<IEnumerable<KullaniciIslemYetkisi>>? islemYetkileri = await _yetkiService.GetKullaniciYetkileriAsync(kullanici.Data.KullaniciUuid);

                AccessToken token = _tokenHelper.CreateToken(kullanici.Data, null);
                _tokenCacheManager.RegisterToken(token.Token, kullanici.Data.KullaniciUuid, token.ExpireInMinutes);

                var loginResponse = new LoginCommandResponseDTO
                {
                    AccessToken = token.Token,
                    UserType = kullanici.Data.KullaniciTipi,
                    UserName = $"{kullanici.Data.KurumEposta.Substring(0, kullanici.Data.KurumEposta.IndexOf('@'))}",
                    GivenName = kullanici.Data.Ad,
                    MiddleName = kullanici.Data.OrtaAd,
                    Surname = kullanici.Data.Soyad,
                    InstitutionEmail = kullanici.Data.KurumEposta,
                    InstitutionRegistrationNumber = kullanici.Data.KurumSicilNo

                };

                return BaseResponse<LoginCommandResponseDTO>.Success(loginResponse, "Giriş başarılı", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}