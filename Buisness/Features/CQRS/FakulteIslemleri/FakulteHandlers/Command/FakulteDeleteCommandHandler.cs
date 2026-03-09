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
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.FakulteHandlers.Command
{
    public class FakulteDeleteCommand : ISecuredCommand<BaseResponse<FakulteDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
    }

    public class FakulteDeleteCommandHandler : ICommandHandler<FakulteDeleteCommand, BaseResponse<FakulteDeleteCommandResponseDTO>>
    {
        private readonly IFakulteService _fakulteService;
        private readonly IUserContext _userContext;

        public FakulteDeleteCommandHandler(IFakulteService fakulteService, IUserContext userContext)
        {
            _fakulteService = fakulteService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<FakulteDeleteCommandResponseDTO>> Handle(FakulteDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<FakulteDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IResult result = await _fakulteService.DeleteFakulteAsync(request.FakulteUuid);

                if (!result.Success)
                    return BaseResponse<FakulteDeleteCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<FakulteDeleteCommandResponseDTO>.Success(new FakulteDeleteCommandResponseDTO
                {
                    FakulteUuid = request.FakulteUuid
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<FakulteDeleteCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}