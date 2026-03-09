using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Command
{
    public class AnaDalAddCommand : ISecuredCommand<BaseResponse<AnaDalAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public string AnaDalAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class AnaDalAddCommandHandler : ICommandHandler<AnaDalAddCommand, BaseResponse<AnaDalAddCommandResponseDTO>>
    {
        private readonly IAnaDalService _anaDalService;
        private readonly IUserContext _userContext;

        public AnaDalAddCommandHandler(IAnaDalService anaDalService, IUserContext userContext)
        {
            _anaDalService = anaDalService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<AnaDalAddCommandResponseDTO>> Handle(AnaDalAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<AnaDalAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Check if anaDal with same name already exists
                var existingAnaDal = await _anaDalService.GetAllByPaged(request.AnaDalAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                if (existingAnaDal.Data.Any())
                    return BaseResponse<AnaDalAddCommandResponseDTO>.Failure("Bu isimde bir ana dal zaten mevcut.", statusCode: 400);

                AnaDal newAnaDal = new AnaDal
                {
                    AnaDalUuid = Guid.NewGuid(),
                    AnaDalAdi = request.AnaDalAdi,
                    FakulteUuid = request.FakulteUuid,
                    KurulusTarihi = request.KurulusTarihi,
                    OlusturmaTarihi = DateTime.UtcNow,
                    GuncellemeTarihi = DateTime.UtcNow
                };

                IResult result = await _anaDalService.AddAnaDalAsync(newAnaDal);

                if (!result.Success)
                    return BaseResponse<AnaDalAddCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<AnaDalAddCommandResponseDTO>.Success(new AnaDalAddCommandResponseDTO
                {
                    AnaDalUuid = newAnaDal.AnaDalUuid,
                    AnaDalAdi = newAnaDal.AnaDalAdi,
                    FakulteUuid = newAnaDal.FakulteUuid,
                    KurulusTarihi = newAnaDal.KurulusTarihi,
                    OlusturmaTarihi = newAnaDal.OlusturmaTarihi,
                    GuncellemeTarihi = newAnaDal.GuncellemeTarihi
                }, result.Message, 201);
            }
            catch (Exception ex)
            {
                return BaseResponse<AnaDalAddCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}