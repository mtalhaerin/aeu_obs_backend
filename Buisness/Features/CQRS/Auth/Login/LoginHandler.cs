using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.ValidationRules.FluentValidation.FieldValidators;
using Core.Buisiness.Features.CQRS;
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

        // 1. ADIM: Token servisini buraya tanımlıyoruz
        private readonly ITokenHelper _tokenHelper;

        // 2. ADIM: Constructor'da servisi inject ediyoruz
        public LoginHandler(
            IKullaniciService kullaniciService,
            IYetkiService yetkiService,
            ITokenHelper tokenHelper)
        {
            _kullaniciService = kullaniciService;
            _yetkiService = yetkiService;
            _tokenHelper = tokenHelper;
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

                // Debug logları (Production'da kaldırılmalı)
                // Console.WriteLine($"result.Data.ParolaHash: {kullanici.Data!.ParolaHash}");

                if (!HashingHelper.VerifyPasswordHash(request.Password!, kullanici.Data!.ParolaHash, kullanici.Data!.ParolaTuz))
                {
                    return BaseResponse<LoginResponseDTO>.Failure("Parola hatalı", statusCode: 401);
                }

                // Yetkileri çekmek istersen burayı açabilirsin
                // var islemYetkileri = await _yetkiService.GetKullaniciYetkileriAsync(kullanici.Data.KullaniciUuid);

                // 3. ADIM: Artık static sınıf adıyla değil, inject ettiğimiz nesneyle çağırıyoruz
                AccessToken token = _tokenHelper.CreateToken(kullanici.Data, null);

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