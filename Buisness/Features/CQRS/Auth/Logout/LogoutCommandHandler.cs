using Business.Concrete;
using Business.DTOs.ResponseDTOs.AuthDTOs.Command;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.ValidationRules.FluentValidation.FieldValidators;
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

namespace Business.Features.CQRS.Auth.Logout
{
    public class LogoutCommand : ICommand<BaseResponse<LogoutCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, BaseResponse<LogoutCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(
            IGenericHelper genericHelper,
            ITokenHelper tokenHelper,
            ITokenCacheManager tokenCacheManager)
        {
            _genericHelper = genericHelper;
            _tokenHelper = tokenHelper;
            _tokenCacheManager = tokenCacheManager;
        }

        public async Task<BaseResponse<LogoutCommandResponseDTO>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var token = _genericHelper.GetAccessTokenFromHeader();
                if (string.IsNullOrEmpty(token))
                {
                    return BaseResponse<LogoutCommandResponseDTO>.Failure("Token bulunamadı", statusCode: 400);
                }

                Guid userUuid = _tokenCacheManager.ValidateToken(token);
                if (userUuid == Guid.Empty)
                {
                    return BaseResponse<LogoutCommandResponseDTO>.Failure("Token geçersiz veya süresi dolmuş", statusCode: 401);
                }

                _tokenCacheManager.RemoveToken(token);

                var LogoutResponse = new LogoutCommandResponseDTO
                {
                    UserUuid = userUuid,
                };

                return BaseResponse<LogoutCommandResponseDTO>.Success(LogoutResponse, "Başarıyla çıkış yapıldı", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}