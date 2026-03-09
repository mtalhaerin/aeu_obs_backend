using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.FakulteEntities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Command
{
    public class FakulteAddCommand : ISecuredCommand<BaseResponse<FakulteAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class FakulteAddCommandHandler : ICommandHandler<FakulteAddCommand, BaseResponse<FakulteAddCommandResponseDTO>>
    {
        private readonly IFakulteService _fakulteService;
        private readonly IUserContext _userContext;

        public FakulteAddCommandHandler(IFakulteService fakulteService, IUserContext userContext)
        {
            _fakulteService = fakulteService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<FakulteAddCommandResponseDTO>> Handle(FakulteAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<FakulteAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Check if fakulte with same name already exists
                var existingFakulte = await _fakulteService.GetAllByPaged(request.FakulteAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                if (existingFakulte.Data.Any())
                    return BaseResponse<FakulteAddCommandResponseDTO>.Failure("Bu isimde bir fakulte zaten mevcut.", statusCode: 400);

                Fakulte newFakulte = new Fakulte
                {
                    FakulteUuid = Guid.NewGuid(),
                    FakulteAdi = request.FakulteAdi,
                    WebAdres = request.WebAdres,
                    KurulusTarihi = request.KurulusTarihi,
                    OlusturmaTarihi = DateTime.UtcNow,
                    GuncellemeTarihi = DateTime.UtcNow
                };

                IResult result = await _fakulteService.AddFakulteAsync(newFakulte);

                if (!result.Success)
                    return BaseResponse<FakulteAddCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<FakulteAddCommandResponseDTO>.Success(new FakulteAddCommandResponseDTO
                {
                    FakulteUuid = newFakulte.FakulteUuid,
                    FakulteAdi = newFakulte.FakulteAdi,
                    WebAdres = newFakulte.WebAdres,
                    KurulusTarihi = newFakulte.KurulusTarihi,
                    OlusturmaTarihi = newFakulte.OlusturmaTarihi,
                    GuncellemeTarihi = newFakulte.GuncellemeTarihi
                }, result.Message, 201);
            }
            catch (Exception ex)
            {
                return BaseResponse<FakulteAddCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}