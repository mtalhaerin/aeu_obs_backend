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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Command
{
    public class FakulteUpdateCommand : ISecuredCommand<BaseResponse<FakulteUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class FakulteUpdateCommandHandler : ICommandHandler<FakulteUpdateCommand, BaseResponse<FakulteUpdateCommandResponseDTO>>
    {
        private readonly IFakulteService _fakulteService;
        private readonly IUserContext _userContext;

        public FakulteUpdateCommandHandler(IFakulteService fakulteService, IUserContext userContext)
        {
            _fakulteService = fakulteService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<FakulteUpdateCommandResponseDTO>> Handle(FakulteUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<FakulteUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Retrieve the existing Fakulte
                var existingFakulte = await _fakulteService.GetByUuidAsync(request.FakulteUuid);
                if (!existingFakulte.Success || existingFakulte.Data == null)
                    return BaseResponse<FakulteUpdateCommandResponseDTO>.Failure("Fakulte bulunamadı.", statusCode: 404);

                var fakulteToUpdate = existingFakulte.Data;

                // Check if fakulte name is being changed and if new name already exists
                if (!string.Equals(fakulteToUpdate.FakulteAdi, request.FakulteAdi, StringComparison.OrdinalIgnoreCase))
                {
                    var fakulteWithSameName = await _fakulteService.GetAllByPaged(request.FakulteAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                    if (fakulteWithSameName.Data.Any(f => f.FakulteUuid != request.FakulteUuid))
                        return BaseResponse<FakulteUpdateCommandResponseDTO>.Failure("Bu isimde bir fakulte zaten mevcut.", statusCode: 400);
                }

                // Update the Fakulte properties
                fakulteToUpdate.FakulteAdi = request.FakulteAdi;
                fakulteToUpdate.WebAdres = request.WebAdres;
                fakulteToUpdate.KurulusTarihi = request.KurulusTarihi;
                fakulteToUpdate.GuncellemeTarihi = DateTime.UtcNow;

                IResult result = await _fakulteService.UpdateFakulteAsync(fakulteToUpdate);

                if (!result.Success)
                    return BaseResponse<FakulteUpdateCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<FakulteUpdateCommandResponseDTO>.Success(new FakulteUpdateCommandResponseDTO
                {
                    FakulteUuid = fakulteToUpdate.FakulteUuid,
                    FakulteAdi = fakulteToUpdate.FakulteAdi,
                    WebAdres = fakulteToUpdate.WebAdres,
                    KurulusTarihi = fakulteToUpdate.KurulusTarihi,
                    OlusturmaTarihi = fakulteToUpdate.OlusturmaTarihi,
                    GuncellemeTarihi = fakulteToUpdate.GuncellemeTarihi
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<FakulteUpdateCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}