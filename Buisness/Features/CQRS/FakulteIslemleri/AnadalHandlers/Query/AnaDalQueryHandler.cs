using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Query
{
    public class AnaDalQuery : ISecuredQuery<BaseResponse<AnaDalQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
    }

    public class AnaDalQueryHandler : IQueryHandler<AnaDalQuery, BaseResponse<AnaDalQueryResponseDTO>>
    {
        private readonly IAnaDalService _anaDalService;
        private readonly IUserContext _userContext;

        public AnaDalQueryHandler(IAnaDalService anaDalService, IUserContext userContext)
        {
            _anaDalService = anaDalService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<AnaDalQueryResponseDTO>> Handle(AnaDalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                IDataResult<AnaDal> result = await _anaDalService.GetByUuidAsync(request.AnaDalUuid);
                if (!result.Success || result.Data == null)
                    return BaseResponse<AnaDalQueryResponseDTO>.Failure("Ana dal bulunamadı.", statusCode: 404);

                return BaseResponse<AnaDalQueryResponseDTO>.Success(new AnaDalQueryResponseDTO
                {
                    AnaDalUuid = result.Data.AnaDalUuid,
                    AnaDalAdi = result.Data.AnaDalAdi,
                    BolumUuid = result.Data.BolumUuid,
                    KurulusTarihi = result.Data.KurulusTarihi,
                    OlusturmaTarihi = result.Data.OlusturmaTarihi,
                    GuncellemeTarihi = result.Data.GuncellemeTarihi
                }, "Ana dal başarıyla getirildi.", 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<AnaDalQueryResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}