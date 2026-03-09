using Business.Concrete.DersManagers;
using Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Utilities.Paging;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.DersEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.DersHandlers.Query
{
    public class DersGetAllQuery : ISecuredQuery<BaseResponse<DersGetAllResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string? DersKodu { get; set; } = string.Empty;
        public string? DersAdi { get; set; } = string.Empty;
        public int? Kredi { get; set; } = null;
        public int? Akts { get; set; } = null;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
        public Pager? Pager { get; set; } = null;
    }

    public class DersGetAllQueryHandler : IQueryHandler<DersGetAllQuery, BaseResponse<DersGetAllResponseDTO>>
    {
        private readonly IDersService _dersService;

        public DersGetAllQueryHandler(IDersService dersService)
        {
            _dersService = dersService;
        }

        public async Task<BaseResponse<DersGetAllResponseDTO>> Handle(DersGetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Pager pager = request.Pager ?? new Pager { Page = 1, PageSize = 10 };

                IDataResult<List<Ders>> result = await _dersService.GetAllByPaged(
                    request.DersKodu,
                    request.DersAdi,
                    request.Kredi,
                    request.Akts,
                    request.OlusturmaTarihi,
                    request.GuncellemeTarihi,
                    pager);

                if (!result.Success)
                    return BaseResponse<DersGetAllResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<DersGetAllResponseDTO>.Success(new DersGetAllResponseDTO
                {
                    Dersler = result.Data.Select(d => new DersResponseDTO
                    {
                        DersUuid = d.DersUuid,
                        DersKodu = d.DersKodu,
                        DersAdi = d.DersAdi,
                        Aciklama = d.Aciklama,
                        HaftalikDersSaati = d.HaftalikDersSaati,
                        Kredi = d.Kredi,
                        Akts = d.Akts,
                        OlusturmaTarihi = d.OlusturmaTarihi,
                        GuncellemeTarihi = d.GuncellemeTarihi
                    }).ToList(),
                    //Pager = pager
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<DersGetAllResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}