using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.QueryDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Query
{
    public class UserQuery : ISecuredQuery<BaseResponse<UserQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }

    public class UserQueryHandler : IQueryHandler<UserQuery, BaseResponse<UserQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciServis;

        public UserQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IKullaniciService kullaniciServis)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciServis = kullaniciServis;
        }

        public async Task<BaseResponse<UserQueryResponseDTO>> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<UserQueryResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<Kullanici> result = await _kullaniciServis.GetByUuidAsync(request.KullaniciUuid);
                if (!result.Success)
                    return BaseResponse<UserQueryResponseDTO>.Failure(result.Message, statusCode: 404);

                return BaseResponse<UserQueryResponseDTO>.Success(new UserQueryResponseDTO
                {
                    KullaniciUuid = result.Data.KullaniciUuid,
                    KullaniciTipi = result.Data.KullaniciTipi,
                    Ad = result.Data.Ad,
                    OrtaAd = result.Data.OrtaAd,
                    Soyad = result.Data.Soyad,
                    KurumEposta = result.Data.KurumEposta,
                    KurumSicilNo = result.Data.KurumSicilNo,
                    OlusturmaTarihi = result.Data.OlusturmaTarihi,
                    GuncellemeTarihi = result.Data.GuncellemeTarihi
                }, result.Message, 200);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
