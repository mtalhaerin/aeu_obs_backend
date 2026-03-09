using Business.Concrete.FakulteManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs;
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

namespace Business.Features.CQRS.FakulteIslemleri.AnaDalHandlers.Command
{
    public class AnaDalDeleteCommand : ISecuredCommand<BaseResponse<AnaDalDeleteCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
    }

    public class AnaDalDeleteCommandHandler : ICommandHandler<AnaDalDeleteCommand, BaseResponse<AnaDalDeleteCommandResponseDTO>>
    {
        private readonly IAnaDalService _anaDalService;
        private readonly IUserContext _userContext;

        public AnaDalDeleteCommandHandler(IAnaDalService anaDalService, IUserContext userContext)
        {
            _anaDalService = anaDalService;
            _userContext = userContext;
        }

        public async Task<BaseResponse<AnaDalDeleteCommandResponseDTO>> Handle(AnaDalDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<AnaDalDeleteCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IResult result = await _anaDalService.DeleteAnaDalAsync(request.AnaDalUuid);

                if (!result.Success)
                    return BaseResponse<AnaDalDeleteCommandResponseDTO>.Failure(result.Message, statusCode: 400);

                return BaseResponse<AnaDalDeleteCommandResponseDTO>.Success(new AnaDalDeleteCommandResponseDTO
                {
                    AnaDalUuid = request.AnaDalUuid
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                return BaseResponse<AnaDalDeleteCommandResponseDTO>.Failure($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}