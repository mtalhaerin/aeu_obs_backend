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
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.DersHandlers.Command
{
    public class DersAddCommand : ISecuredCommand<BaseResponse<DersAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public DersAddCommandRequestDTO Ders { get; set; } = new DersAddCommandRequestDTO();
    }

    public class DersAddCommandHandler : ICommandHandler<DersAddCommand, BaseResponse<DersAddCommandResponseDTO>>
    {
        private readonly IDersService _dersService;
        private readonly IUserContext _userContext;

        public DersAddCommandHandler(IDersService dersService, IUserContext userContext)
        {
            _dersService = dersService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<DersAddCommandResponseDTO>> Handle(DersAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<DersAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                // Check if DersKodu already exists
                var existingDers = await _dersService.GetAllByPaged(request.Ders.DersKodu, null, null, null, DateTime.MinValue, DateTime.MaxValue, null);
                if (existingDers.Data.Any())
                    return BaseResponse<DersAddCommandResponseDTO>.Failure("A course with the same DersKodu already exists.", statusCode: 400);

                Ders newDers = new Ders
                {
                    DersUuid = Guid.NewGuid(),
                    DersKodu = request.Ders.DersKodu,
                    DersAdi = request.Ders.DersAdi,
                    Aciklama = request.Ders.Aciklama,
                    HaftalikDersSaati = request.Ders.HaftalikDersSaati,
                    Kredi = request.Ders.Kredi,
                    Akts = request.Ders.Akts
                };
                // Add the new Ders
                IResult result = await _dersService.AddDersAsync(newDers);

                if (!result.Success)
                    return BaseResponse<DersAddCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<DersAddCommandResponseDTO>.Success(new DersAddCommandResponseDTO
                {
                    DersUuid = newDers.DersUuid,
                    DersKodu = newDers.DersKodu,
                    DersAdi = newDers.DersAdi,
                    Aciklama = newDers.Aciklama,
                    HaftalikDersSaati = newDers.HaftalikDersSaati,
                    Kredi = newDers.Kredi,
                    Akts = newDers.Akts,
                    OlusturmaTarihi = newDers.OlusturmaTarihi,
                    GuncellemeTarihi = newDers.GuncellemeTarihi
                }, result.Message, 201);
            }
            catch (Exception ex)
            {
                return BaseResponse<DersAddCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}