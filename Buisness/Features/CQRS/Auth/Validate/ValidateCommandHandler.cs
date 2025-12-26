using Business.Concrete;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.AuthDTOs.Command;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Auth.Validate
{
    public class ValidateCommand : ISecuredCommand<BaseResponse<ValidateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class ValidateCommandHandler : ICommandHandler<ValidateCommand, BaseResponse<ValidateCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciService;
        private readonly IYetkiService _yetkiService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenCacheManager _tokenCacheManager;

        public ValidateCommandHandler(
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

        public async Task<BaseResponse<ValidateCommandResponseDTO>> Handle(ValidateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;


                ValidateCommandResponseDTO refreshResponse = new()
                {
                    IsValid = true,
                };

                return BaseResponse<ValidateCommandResponseDTO>.Success(refreshResponse, "Token geçerli", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
