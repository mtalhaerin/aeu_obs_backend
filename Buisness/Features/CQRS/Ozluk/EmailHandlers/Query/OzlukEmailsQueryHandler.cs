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

namespace Business.Features.CQRS.Ozluk.EmailHandlers.Query
{
    public class OzlukEmailsQuery : ISecuredQuery<BaseResponse<OzlukEmailsQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class OzlukEmailsQueryHandler : IQueryHandler<OzlukEmailsQuery, BaseResponse<OzlukEmailsQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IEpostaService _epostaService;

        public OzlukEmailsQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IEpostaService epostaService)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _epostaService = epostaService;
        }

        public async Task<BaseResponse<OzlukEmailsQueryResponseDTO>> Handle(OzlukEmailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;
                
                IDataResult<IEnumerable<Eposta>> Emails = await _epostaService.GetUserEmailsAsync(kullanici.KullaniciUuid);
                
                if (!Emails.Success)
                {
                    return BaseResponse<OzlukEmailsQueryResponseDTO>.Failure("Email bulunamadı", statusCode: 404);
                }

                var epostalarResponse = new OzlukEmailsQueryResponseDTO { 
                    Epostalar =  Emails.Data.Select(email => new OzlukEmailQueryResponseDTO
                    { 
                        EpostaUuid = email.EpostaUuid,
                        KullaniciUuid = email.KullaniciUuid,
                        EpostaAdresi = email.EpostaAdresi,
                        EpostaTipi = email.EpostaTipi,
                        Oncelikli = email.Oncelikli
                    }).ToList()
                };


                return BaseResponse<OzlukEmailsQueryResponseDTO>.Success(epostalarResponse, "Kullanıcı profili başarıyla getirildi", 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
