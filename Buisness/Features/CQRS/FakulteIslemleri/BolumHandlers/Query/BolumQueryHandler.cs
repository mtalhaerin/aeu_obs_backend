using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.FakulteEntities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Query
{
    public class BolumQuery : ISecuredQuery<BaseResponse<BolumQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid BolumUuid { get; set; } = Guid.Empty;
    }

    public class BolumQueryHandler : IQueryHandler<BolumQuery, BaseResponse<BolumQueryResponseDTO>>
    {
        private readonly IBolumService _bolumService;
        private readonly IUserContext _userContext;

        public BolumQueryHandler(IBolumService bolumService, IUserContext userContext)
        {
            _bolumService = bolumService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<BolumQueryResponseDTO>> Handle(BolumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                IDataResult<Bolum> result = await _bolumService.GetByUuidAsync(request.BolumUuid);
                if (!result.Success || result.Data == null)
                    return BaseResponse<BolumQueryResponseDTO>.Failure("Bölüm bulunamadı.", statusCode: 404);

                return BaseResponse<BolumQueryResponseDTO>.Success(new BolumQueryResponseDTO
                {
                    BolumUuid = result.Data.BolumUuid,
                    BolumAdi = result.Data.BolumAdi,
                    AnaDalUuid = result.Data.AnaDalUuid,
                    KurulusTarihi = result.Data.KurulusTarihi,
                    OlusturmaTarihi = result.Data.OlusturmaTarihi,
                    GuncellemeTarihi = result.Data.GuncellemeTarihi
                }, "Bölüm başarıyla getirildi.", 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<BolumQueryResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}