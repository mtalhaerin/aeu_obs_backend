using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Concrete.OzlukEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.Ozluk.TelefonHandlers.Query
{
    public class OzlukPhoneQuery : ISecuredQuery<BaseResponse<OzlukPhoneQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid? KullaniciUuid { get; set; } = Guid.Empty;
        public Guid? TelefonUuid { get; set; } = Guid.Empty;
    }

    public class OzlukPhoneQueryHandler : IQueryHandler<OzlukPhoneQuery, BaseResponse<OzlukPhoneQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITelefonService _telefonService;

        public OzlukPhoneQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            ITelefonService telefonService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _telefonService = telefonService;
        }

        public async Task<BaseResponse<OzlukPhoneQueryResponseDTO>> Handle(OzlukPhoneQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.TelefonUuid == null || request.TelefonUuid == Guid.Empty || request.KullaniciUuid == null || request.KullaniciUuid == Guid.Empty)
                {
                    return BaseResponse<OzlukPhoneQueryResponseDTO>.Failure("Kullanici veya Telefon UUID'si belirtilmemiş", statusCode: 400);
                }

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL && kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukPhoneQueryResponseDTO>.Failure("Başka kullancıya ait Telefon bilgisni okuma izniniz bulunamamkta.", statusCode: 401);

                IDataResult<Telefon> Phone = await _telefonService.GetUserTelefonByUuidAsync(request.KullaniciUuid.Value, request.TelefonUuid);
                
                if (!Phone.Success)
                {
                    return BaseResponse<OzlukPhoneQueryResponseDTO>.Failure("Telefon bulunamadı", statusCode: 404);
                }

                var telefonResponse = new OzlukPhoneQueryResponseDTO
                {
                    TelefonUuid = Phone.Data.TelefonUuid,
                    KullaniciUuid = Phone.Data.KullaniciUuid,
                    UlkeKodu = Phone.Data.UlkeKodu,
                    TelefonNo = Phone.Data.TelefonNo,
                    TelefonTipi = Phone.Data.TelefonTipi,
                    Oncelikli = Phone.Data.Oncelikli
                };


                return BaseResponse<OzlukPhoneQueryResponseDTO>.Success(telefonResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
