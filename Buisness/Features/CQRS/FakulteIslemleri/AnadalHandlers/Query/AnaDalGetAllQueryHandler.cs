using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Query
{
    public class AnaDalGetAllQuery : ISecuredQuery<BaseResponse<AnaDalGetAllResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string? AnaDalAdi { get; set; } = string.Empty;
        public Guid? FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
        public Pager? Pager { get; set; } = null;
    }

    public class AnaDalGetAllQueryHandler : IQueryHandler<AnaDalGetAllQuery, BaseResponse<AnaDalGetAllResponseDTO>>
    {
        private readonly IAnaDalService _anaDalService;
        private readonly IUserContext _userContext;

        public AnaDalGetAllQueryHandler(IAnaDalService anaDalService, IUserContext userContext)
        {
            _anaDalService = anaDalService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<AnaDalGetAllResponseDTO>> Handle(AnaDalGetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                Pager? pager = request.Pager ?? new Pager { Page = 1, PageSize = 10 };

                IDataResult<List<AnaDal>> result = await _anaDalService.GetAllByPaged(
                    request.AnaDalAdi,
                    request.FakulteUuid,
                    request.KurulusTarihi,
                    request.OlusturmaTarihi,
                    request.GuncellemeTarihi,
                    pager);

                if (!result.Success)
                    return BaseResponse<AnaDalGetAllResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<AnaDalGetAllResponseDTO>.Success(new AnaDalGetAllResponseDTO
                {
                    AnaDallar = result.Data.Select(a => new AnaDalQueryResponseDTO
                    {
                        AnaDalUuid = a.AnaDalUuid,
                        AnaDalAdi = a.AnaDalAdi,
                        FakulteUuid = a.FakulteUuid,
                        KurulusTarihi = a.KurulusTarihi,
                        OlusturmaTarihi = a.OlusturmaTarihi,
                        GuncellemeTarihi = a.GuncellemeTarihi
                    }).ToList(),
                    //Pager = pager
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<AnaDalGetAllResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}