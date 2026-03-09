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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.DersHandlers.Command
{
    public class DersDeleteCommand : ISecuredCommand<BaseResponse<DersDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public DersDeleteCommandRequestDTO Ders { get; set; } = new DersDeleteCommandRequestDTO();
    }

    public class DersDeleteCommandHandler : ICommandHandler<DersDeleteCommand, BaseResponse<DersDeleteCommandResponseDTO>>
    {
        private readonly IDersService _dersService;
        private readonly IUserContext _userContext;

        public DersDeleteCommandHandler(IDersService dersService, IUserContext userContext)
        {
            _dersService = dersService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<DersDeleteCommandResponseDTO>> Handle(DersDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<DersDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IResult result = await _dersService.DeleteDersAsync(request.Ders.DersUuid);

                if (!result.Success)
                    return BaseResponse<DersDeleteCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<DersDeleteCommandResponseDTO>.Success(new DersDeleteCommandResponseDTO
                {
                    DersUuid = request.Ders.DersUuid
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<DersDeleteCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}