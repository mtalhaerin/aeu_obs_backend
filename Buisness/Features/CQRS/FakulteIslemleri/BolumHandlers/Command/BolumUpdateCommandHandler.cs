using Buisness.DTOs.ResponseDTOs.FakulteDTOs.CommandDTOs;
using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.BolumDTOs.CommandDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Command
{
    public class BolumUpdateCommand : ISecuredCommand<BaseResponse<BolumUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public string BolumAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class BolumUpdateCommandHandler : ICommandHandler<BolumUpdateCommand, BaseResponse<BolumUpdateCommandResponseDTO>>
    {
        private readonly IBolumService _bolumService;
        private readonly IUserContext _userContext;

        public BolumUpdateCommandHandler(IBolumService bolumService, IUserContext userContext)
        {
            _bolumService = bolumService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<BolumUpdateCommandResponseDTO>> Handle(BolumUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<BolumUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Retrieve the existing Bolum
                var existingBolum = await _bolumService.GetByUuidAsync(request.BolumUuid);
                if (!existingBolum.Success || existingBolum.Data == null)
                    return BaseResponse<BolumUpdateCommandResponseDTO>.Failure("Bölüm bulunamadı.", statusCode: 404);

                var bolumToUpdate = existingBolum.Data;

                // Check if bolum name is being changed and if new name already exists
                if (!string.Equals(bolumToUpdate.BolumAdi, request.BolumAdi, StringComparison.OrdinalIgnoreCase))
                {
                    var bolumWithSameName = await _bolumService.GetAllByPaged(request.BolumAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                    if (bolumWithSameName.Data.Any(b => b.BolumUuid != request.BolumUuid))
                        return BaseResponse<BolumUpdateCommandResponseDTO>.Failure("Bu isimde bir bölüm zaten mevcut.", statusCode: 400);
                }

                // Update the Bolum properties
                bolumToUpdate.BolumAdi = request.BolumAdi;
                bolumToUpdate.FakulteUuid = request.FakulteUuid;
                bolumToUpdate.KurulusTarihi = request.KurulusTarihi;
                bolumToUpdate.GuncellemeTarihi = DateTime.UtcNow;

                IResult result = await _bolumService.UpdateBolumAsync(bolumToUpdate);

                if (!result.Success)
                    return BaseResponse<BolumUpdateCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<BolumUpdateCommandResponseDTO>.Success(new BolumUpdateCommandResponseDTO
                {
                    BolumUuid = bolumToUpdate.BolumUuid,
                    BolumAdi = bolumToUpdate.BolumAdi,
                    FakulteUuid = bolumToUpdate.FakulteUuid,
                    KurulusTarihi = bolumToUpdate.KurulusTarihi,
                    OlusturmaTarihi = bolumToUpdate.OlusturmaTarihi,
                    GuncellemeTarihi = bolumToUpdate.GuncellemeTarihi
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<BolumUpdateCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}