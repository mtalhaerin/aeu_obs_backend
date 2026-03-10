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
using Entities.Concrete.FakulteEntities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Command
{
    public class BolumAddCommand : ISecuredCommand<BaseResponse<BolumAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string BolumAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class BolumAddCommandHandler : ICommandHandler<BolumAddCommand, BaseResponse<BolumAddCommandResponseDTO>>
    {
        private readonly IBolumService _bolumService;
        private readonly IUserContext _userContext;

        public BolumAddCommandHandler(IBolumService bolumService, IUserContext userContext)
        {
            _bolumService = bolumService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<BolumAddCommandResponseDTO>> Handle(BolumAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<BolumAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Check if bolum with same name already exists
                var existingBolum = await _bolumService.GetAllByPaged(request.BolumAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                if (existingBolum.Data.Any())
                    return BaseResponse<BolumAddCommandResponseDTO>.Failure("Bu isimde bir bölüm zaten mevcut.", statusCode: 400);

                Bolum newBolum = new Bolum
                {
                    BolumUuid = Guid.NewGuid(),
                    BolumAdi = request.BolumAdi,
                    FakulteUuid = request.FakulteUuid,
                    KurulusTarihi = request.KurulusTarihi,
                    OlusturmaTarihi = DateTime.UtcNow,
                    GuncellemeTarihi = DateTime.UtcNow
                };

                IResult result = await _bolumService.AddBolumAsync(newBolum);

                if (!result.Success)
                    return BaseResponse<BolumAddCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<BolumAddCommandResponseDTO>.Success(new BolumAddCommandResponseDTO
                {
                    BolumUuid = newBolum.BolumUuid,
                    BolumAdi = newBolum.BolumAdi,
                    FakulteUuid = newBolum.FakulteUuid,
                    KurulusTarihi = newBolum.KurulusTarihi,
                    OlusturmaTarihi = newBolum.OlusturmaTarihi,
                    GuncellemeTarihi = newBolum.GuncellemeTarihi
                }, result.Message, 201);
            }
            catch (Exception ex)
            {
                return BaseResponse<BolumAddCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}