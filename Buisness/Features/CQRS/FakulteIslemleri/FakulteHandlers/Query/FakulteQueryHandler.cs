using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Query
{
    public class FakulteQuery : ISecuredQuery<BaseResponse<FakulteResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
    }

    public class FakulteQueryHandler : IQueryHandler<FakulteQuery, BaseResponse<FakulteResponseDTO>>
    {
        private readonly IFakulteService _fakulteService;
        private readonly IUserContext _userContext;

        public FakulteQueryHandler(IFakulteService fakulteService, IUserContext userContext)
        {
            _fakulteService = fakulteService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<FakulteResponseDTO>> Handle(FakulteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                IDataResult<Fakulte> result = await _fakulteService.GetByUuidAsync(request.FakulteUuid);
                if (!result.Success || result.Data == null)
                    return BaseResponse<FakulteResponseDTO>.Failure("Fakulte bulunamadı.", statusCode: 404);

                return BaseResponse<FakulteResponseDTO>.Success(new FakulteResponseDTO
                {
                    FakulteUuid = result.Data.FakulteUuid,
                    FakulteAdi = result.Data.FakulteAdi,
                    WebAdres = result.Data.WebAdres,
                    KurulusTarihi = result.Data.KurulusTarihi,
                    OlusturmaTarihi = result.Data.OlusturmaTarihi,
                    GuncellemeTarihi = result.Data.GuncellemeTarihi
                }, "Fakulte başarıyla getirildi.", 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<FakulteResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}