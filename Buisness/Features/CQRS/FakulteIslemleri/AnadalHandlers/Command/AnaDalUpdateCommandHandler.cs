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
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Command
{
    public class AnaDalUpdateCommand : ISecuredCommand<BaseResponse<AnaDalUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
        public string AnaDalAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }

    public class AnaDalUpdateCommandHandler : ICommandHandler<AnaDalUpdateCommand, BaseResponse<AnaDalUpdateCommandResponseDTO>>
    {
        private readonly IAnaDalService _anaDalService;
        private readonly IUserContext _userContext;

        public AnaDalUpdateCommandHandler(IAnaDalService anaDalService, IUserContext userContext)
        {
            _anaDalService = anaDalService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<AnaDalUpdateCommandResponseDTO>> Handle(AnaDalUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<AnaDalUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Retrieve the existing AnaDal
                var existingAnaDal = await _anaDalService.GetByUuidAsync(request.AnaDalUuid);
                if (!existingAnaDal.Success || existingAnaDal.Data == null)
                    return BaseResponse<AnaDalUpdateCommandResponseDTO>.Failure("Ana dal bulunamadı.", statusCode: 404);

                var anaDalToUpdate = existingAnaDal.Data;

                // Check if anaDal name is being changed and if new name already exists
                if (!string.Equals(anaDalToUpdate.AnaDalAdi, request.AnaDalAdi, StringComparison.OrdinalIgnoreCase))
                {
                    var anaDalWithSameName = await _anaDalService.GetAllByPaged(request.AnaDalAdi, null, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, null);
                    if (anaDalWithSameName.Data.Any(a => a.AnaDalUuid != request.AnaDalUuid))
                        return BaseResponse<AnaDalUpdateCommandResponseDTO>.Failure("Bu isimde bir ana dal zaten mevcut.", statusCode: 400);
                }

                // Update the AnaDal properties
                anaDalToUpdate.AnaDalAdi = request.AnaDalAdi;
                anaDalToUpdate.FakulteUuid = request.FakulteUuid;
                anaDalToUpdate.KurulusTarihi = request.KurulusTarihi;
                anaDalToUpdate.GuncellemeTarihi = DateTime.UtcNow;

                IResult result = await _anaDalService.UpdateAnaDalAsync(anaDalToUpdate);

                if (!result.Success)
                    return BaseResponse<AnaDalUpdateCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<AnaDalUpdateCommandResponseDTO>.Success(new AnaDalUpdateCommandResponseDTO
                {
                    AnaDalUuid = anaDalToUpdate.AnaDalUuid,
                    AnaDalAdi = anaDalToUpdate.AnaDalAdi,
                    FakulteUuid = anaDalToUpdate.FakulteUuid,
                    KurulusTarihi = anaDalToUpdate.KurulusTarihi,
                    OlusturmaTarihi = anaDalToUpdate.OlusturmaTarihi,
                    GuncellemeTarihi = anaDalToUpdate.GuncellemeTarihi
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<AnaDalUpdateCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}