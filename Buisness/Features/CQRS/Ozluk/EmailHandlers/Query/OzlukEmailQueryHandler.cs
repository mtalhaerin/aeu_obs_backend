using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.Ozluk.EmailHandlers.Query
{
    public class OzlukEmailQuery : ISecuredQuery<BaseResponse<OzlukEmailQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid? KullaniciUuid { get; set; } = Guid.Empty;
        public Guid? EmailUuid { get; set; } = Guid.Empty;
    }

    public class OzlukEmailQueryHandler : IQueryHandler<OzlukEmailQuery, BaseResponse<OzlukEmailQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IEpostaService _epostaService;

        public OzlukEmailQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IEpostaService epostaService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _epostaService = epostaService;
        }

        public async Task<BaseResponse<OzlukEmailQueryResponseDTO>> Handle(OzlukEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (request.EmailUuid == Guid.Empty || request.EmailUuid == null || request.KullaniciUuid == Guid.Empty || request.KullaniciUuid == null)
                    return BaseResponse<OzlukEmailQueryResponseDTO>.Failure("Kullanici veya Email UUID'si belirtilmemiş", statusCode: 400);

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL &&
                    kullanici.KullaniciUuid != request.KullaniciUuid)
                    return BaseResponse<OzlukEmailQueryResponseDTO>.Failure("Başka kullancıya ait Email bilgisni okuma izniniz bulunamamkta.", statusCode: 401);

                IDataResult<Eposta> Email = await _epostaService.GetUserEmailByUuidAsync(kullanici.KullaniciUuid, request.EmailUuid);
                
                if (!Email.Success)
                {
                    return BaseResponse<OzlukEmailQueryResponseDTO>.Failure("Email bulunamadı", statusCode: 404);
                }

                var emailResponse = new OzlukEmailQueryResponseDTO
                {
                    EpostaUuid = Email.Data.EpostaUuid,
                    KullaniciUuid = Email.Data.KullaniciUuid,
                    EpostaAdresi = Email.Data.EpostaAdresi,
                    EpostaTipi = Email.Data.EpostaTipi,
                    Oncelikli = Email.Data.Oncelikli
                };


                return BaseResponse<OzlukEmailQueryResponseDTO>.Success(emailResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
