using Business.Concrete.DersManagers;
using Business.ContextCarrier;
using Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Buisiness.Features.CQRS;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.DersEntities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.DersHandlers.Command
{
    public class DersUpdateCommand : ISecuredCommand<BaseResponse<DersUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public DersUpdateCommandRequestDTO Ders { get; set; } = new DersUpdateCommandRequestDTO();
    }

    public class DersUpdateCommandHandler : ICommandHandler<DersUpdateCommand, BaseResponse<DersUpdateCommandResponseDTO>>
    {
        private readonly IDersService _dersService;
        private readonly IUserContext _userContext;

        public DersUpdateCommandHandler(IDersService dersService, IUserContext userContext)
        {
            _dersService = dersService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<DersUpdateCommandResponseDTO>> Handle(DersUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<DersUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Retrieve the existing Ders
                var existingDers = await _dersService.GetByUuidAsync(request.Ders.DersUuid);
                if (!existingDers.Success || existingDers.Data == null)
                    return BaseResponse<DersUpdateCommandResponseDTO>.Failure("Ders not found.", statusCode: 404);

                var dersToUpdate = existingDers.Data;

                // Check if DersKodu is being changed
                if (!string.Equals(dersToUpdate.DersKodu, request.Ders.DersKodu, StringComparison.OrdinalIgnoreCase))
                {
                    // Verify that the new DersKodu does not already exist
                    var dersWithSameKodu = await _dersService.GetAllByPaged(request.Ders.DersKodu, null, null, null, DateTime.MinValue, DateTime.MaxValue, null);
                    if (dersWithSameKodu.Data.Any(d => d.DersUuid != request.Ders.DersUuid))
                        return BaseResponse<DersUpdateCommandResponseDTO>.Failure("A course with the same DersKodu already exists.", statusCode: 400);
                }

                // Update the Ders properties
                dersToUpdate.DersKodu = request.Ders.DersKodu;
                dersToUpdate.DersAdi = request.Ders.DersAdi;
                dersToUpdate.Aciklama = request.Ders.Aciklama;
                dersToUpdate.HaftalikDersSaati = request.Ders.HaftalikDersSaati;
                dersToUpdate.Kredi = request.Ders.Kredi;
                dersToUpdate.Akts = request.Ders.Akts;

                // Perform the update
                IResult result = await _dersService.UpdateDersAsync(dersToUpdate);

                if (!result.Success)
                    return BaseResponse<DersUpdateCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<DersUpdateCommandResponseDTO>.Success(new DersUpdateCommandResponseDTO
                {
                    DersUuid = dersToUpdate.DersUuid,
                    DersKodu = dersToUpdate.DersKodu,
                    DersAdi = dersToUpdate.DersAdi,
                    Aciklama = dersToUpdate.Aciklama,
                    HaftalikDersSaati = dersToUpdate.HaftalikDersSaati,
                    Kredi = dersToUpdate.Kredi,
                    Akts = dersToUpdate.Akts,
                    OlusturmaTarihi = dersToUpdate.OlusturmaTarihi,
                    GuncellemeTarihi = dersToUpdate.GuncellemeTarihi
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<DersUpdateCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}