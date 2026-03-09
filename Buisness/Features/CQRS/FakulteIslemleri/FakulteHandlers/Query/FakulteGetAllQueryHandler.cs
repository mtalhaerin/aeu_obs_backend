using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Paging;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.FakulteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Query
{
    public class FakulteGetAllQuery : ISecuredQuery<BaseResponse<FakulteGetAllResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string? FakulteAdi { get; set; } = string.Empty;
        public string? WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
        public Pager? Pager { get; set; } = null;
    }

    public class FakulteGetAllQueryHandler : IQueryHandler<FakulteGetAllQuery, BaseResponse<FakulteGetAllResponseDTO>>
    {
        private readonly IFakulteService _fakulteService;
        private readonly IUserContext _userContext;

        public FakulteGetAllQueryHandler(IFakulteService fakulteService, IUserContext userContext)
        {
            _fakulteService = fakulteService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<FakulteGetAllResponseDTO>> Handle(FakulteGetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                Pager? pager = request.Pager ?? new Pager { Page = 1, PageSize = 10 };

                IDataResult<List<Fakulte>> result = await _fakulteService.GetAllByPaged(
                    request.FakulteAdi,
                    request.WebAdres,
                    request.KurulusTarihi,
                    request.OlusturmaTarihi,
                    request.GuncellemeTarihi,
                    pager);

                if (!result.Success)
                    return BaseResponse<FakulteGetAllResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<FakulteGetAllResponseDTO>.Success(new FakulteGetAllResponseDTO
                {
                    Fakulteler = result.Data.Select(f => new FakulteResponseDTO
                    {
                        FakulteUuid = f.FakulteUuid,
                        FakulteAdi = f.FakulteAdi,
                        WebAdres = f.WebAdres,
                        KurulusTarihi = f.KurulusTarihi,
                        OlusturmaTarihi = f.OlusturmaTarihi,
                        GuncellemeTarihi = f.GuncellemeTarihi
                    }).ToList(),
                    //Pager = pager
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<FakulteGetAllResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}