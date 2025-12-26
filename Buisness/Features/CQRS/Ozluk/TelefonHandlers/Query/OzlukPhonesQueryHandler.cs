using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Query;
using Business.DTOs.ResponseDTOs.OzlukDTOs.OzlukQueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete.OzlukEntities;
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
    public class OzlukPhonesQuery : ISecuredQuery<BaseResponse<OzlukPhonesQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class OzlukPhonesQueryHandler : IQueryHandler<OzlukPhonesQuery, BaseResponse<OzlukPhonesQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly ITelefonService _telefonService;

        public OzlukPhonesQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            ITelefonService telefonService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _telefonService = telefonService;
        }

        public async Task<BaseResponse<OzlukPhonesQueryResponseDTO>> Handle(OzlukPhonesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;
                
                IDataResult<IEnumerable<Telefon>> telefonlar = await _telefonService.GetUserTelefonsAsync(kullanici.KullaniciUuid);
                
                if (!telefonlar.Success)
                {
                    return BaseResponse<OzlukPhonesQueryResponseDTO>.Failure("Telefon bulunamadı", statusCode: 404);
                }

                var telefonlarResponse = new OzlukPhonesQueryResponseDTO { 
                    Telefonlar = telefonlar.Data.Select(t => new OzlukPhoneQueryResponseDTO
                    {
                        TelefonUuid = t.TelefonUuid,
                        KullaniciUuid = t.KullaniciUuid,
                        UlkeKodu = t.UlkeKodu,
                        TelefonNo = t.TelefonNo,
                        TelefonTipi = t.TelefonTipi,
                        Oncelikli = t.Oncelikli
                    }).ToList()
                };


                return BaseResponse<OzlukPhonesQueryResponseDTO>.Success(telefonlarResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
