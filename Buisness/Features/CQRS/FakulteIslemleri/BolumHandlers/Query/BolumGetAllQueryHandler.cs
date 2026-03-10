using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Buisness.DTOs.ResponseDTOs.FakulteDTOs;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Query
{
    public class BolumGetAllQuery : ISecuredQuery<BaseResponse<BolumGetAllResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string? BolumAdi { get; set; } = string.Empty;
        public Guid? FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
        public Pager? Pager { get; set; } = null;
    }

    public class BolumGetAllQueryHandler : IQueryHandler<BolumGetAllQuery, BaseResponse<BolumGetAllResponseDTO>>
    {
        private readonly IBolumService _bolumService;
        private readonly IUserContext _userContext;

        public BolumGetAllQueryHandler(IBolumService bolumService, IUserContext userContext)
        {
            _bolumService = bolumService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<BolumGetAllResponseDTO>> Handle(BolumGetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                Pager? pager = request.Pager ?? new Pager { Page = 1, PageSize = 10 };

                IDataResult<List<Bolum>> result = await _bolumService.GetAllByPaged(
                    request.BolumAdi,
                    request.FakulteUuid,
                    request.KurulusTarihi,
                    request.OlusturmaTarihi,
                    request.GuncellemeTarihi,
                    pager);

                if (!result.Success)
                    return BaseResponse<BolumGetAllResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<BolumGetAllResponseDTO>.Success(new BolumGetAllResponseDTO
                {
                    Bolumler = result.Data.Select(b => new BolumQueryResponseDTO
                    {
                        BolumUuid = b.BolumUuid,
                        BolumAdi = b.BolumAdi,
                        FakulteUuid = b.FakulteUuid,
                        KurulusTarihi = b.KurulusTarihi,
                        OlusturmaTarihi = b.OlusturmaTarihi,
                        GuncellemeTarihi = b.GuncellemeTarihi
                    }).ToList(),
                    //Pager = pager
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<BolumGetAllResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}