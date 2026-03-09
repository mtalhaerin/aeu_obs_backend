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
using System.Threading;
using System.Threading.Tasks;

namespace Business.Features.CQRS.FakulteIslemleri.BolumHandlers.Command
{
    public class BolumDeleteCommand : ISecuredCommand<BaseResponse<BolumDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid BolumUuid { get; set; } = Guid.Empty;
    }

    public class BolumDeleteCommandHandler : ICommandHandler<BolumDeleteCommand, BaseResponse<BolumDeleteCommandResponseDTO>>
    {
        private readonly IBolumService _bolumService;
        private readonly IUserContext _userContext;

        public BolumDeleteCommandHandler(IBolumService bolumService, IUserContext userContext)
        {
            _bolumService = bolumService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<BolumDeleteCommandResponseDTO>> Handle(BolumDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<BolumDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IResult result = await _bolumService.DeleteBolumAsync(request.BolumUuid);

                if (!result.Success)
                    return BaseResponse<BolumDeleteCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<BolumDeleteCommandResponseDTO>.Success(new BolumDeleteCommandResponseDTO
                {
                    BolumUuid = request.BolumUuid
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<BolumDeleteCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}