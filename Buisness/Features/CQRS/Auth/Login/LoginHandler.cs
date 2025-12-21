using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Core.Buisiness.Features.CQRS;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Auth.Login
{
    public class LoginCommand : ICommand<BaseResponse<LoginResponseDTO>>
    {
        public string? email { get; set; } = null;
        public string? password { get; set; } = null;
    }
    public class LoginHandler : ICommandHandler<LoginCommand, BaseResponse<LoginResponseDTO>>
    {

        private readonly IKullaniciService _kullaniciService;
        public LoginHandler(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        public async Task<BaseResponse<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Implementation of login logic goes here.
            // This is a placeholder implementation.
            try
            {
                IDataResult<Kullanici?> result = await _kullaniciService.GetByEmailAsync(request.email!);
                if (!result.Success)
                {
                    return BaseResponse<LoginResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);
                }
                else
                {
                    // Here you would typically verify the password and generate a token.
                    // This is a simplified example.
                    var loginResponse = new LoginResponseDTO
                    {
                        AccessToken = "dummy_token" // Replace with actual token generation logic
                    };
                    return BaseResponse<LoginResponseDTO>.Success(loginResponse, "Giriş başarılı", 200);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
