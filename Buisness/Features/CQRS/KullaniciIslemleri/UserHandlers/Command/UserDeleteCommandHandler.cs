using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Command
{
    public class UserDeleteCommand : ISecuredCommand<BaseResponse<UserDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
    public class UserDeleteCommandHandler : ICommandHandler<UserDeleteCommand, BaseResponse<UserDeleteCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciServis;
        public UserDeleteCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IKullaniciService kullaniciServis)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciServis = kullaniciServis;
        }

        public async Task<BaseResponse<UserDeleteCommandResponseDTO>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<UserDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                if (kullanici.KullaniciUuid == request.KullaniciUuid)
                    return BaseResponse<UserDeleteCommandResponseDTO>.Failure("You can not delete yourself.", statusCode: 409);

                IResult deleteResult = await _kullaniciServis.DeleteKullaniciAsync(request.KullaniciUuid);
                if (!deleteResult.Success)
                    return BaseResponse<UserDeleteCommandResponseDTO>.Failure(deleteResult.Message, statusCode: 400);

                return BaseResponse<UserDeleteCommandResponseDTO>.Success(new UserDeleteCommandResponseDTO { KullaniciUuid = request.KullaniciUuid }, deleteResult.Message, 200);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
