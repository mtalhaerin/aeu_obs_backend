using Business.Concrete.DersManagers;
using Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.DersEntities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.DersHandlers.Query
{
    public class DersQuery : ISecuredQuery<BaseResponse<DersResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid DersUuid { get; set; } = Guid.Empty;
    }

    public class DersQueryHandler : IQueryHandler<DersQuery, BaseResponse<DersResponseDTO>>
    {
        private readonly IDersService _dersService;

        public DersQueryHandler(IDersService dersService)
        {
            _dersService = dersService;
        }

        public async Task<BaseResponse<DersResponseDTO>> Handle(DersQuery request, CancellationToken cancellationToken)
        {
                try
            {
                IDataResult<Ders> result = await _dersService.GetByUuidAsync(request.DersUuid);
                if (!result.Success)
                    return BaseResponse<DersResponseDTO>.Failure(result.Message, statusCode: 404);

                return BaseResponse<DersResponseDTO>.Success(new DersResponseDTO
                {
                    DersUuid = result.Data.DersUuid,
                    DersKodu = result.Data.DersKodu,
                    DersAdi = result.Data.DersAdi,
                    Aciklama = result.Data.Aciklama,
                    HaftalikDersSaati = result.Data.HaftalikDersSaati,
                    Kredi = result.Data.Kredi,
                    Akts = result.Data.Akts,
                    OlusturmaTarihi = result.Data.OlusturmaTarihi,
                    GuncellemeTarihi = result.Data.GuncellemeTarihi
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<DersResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}