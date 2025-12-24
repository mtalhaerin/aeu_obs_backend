using Business.DTOs.ResponseDTOs.AuthDTOs;
using Business.DTOs.ResponseDTOs.Dashboard.Profile.Command;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Response;
using Core.Buisiness.Features.CQRS;

namespace Business.Features.CQRS.Dashboard.Profile.Command
{
    public class ProfileUpdateCommand : ICommand<BaseResponse<ProfileUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
    }

    public class ProfileUpdateCommandHandler : ICommandHandler<ProfileUpdateCommand, BaseResponse<ProfileUpdateCommandResponseDTO>>
    {
        public Task<BaseResponse<ProfileUpdateCommandResponseDTO>> Handle(ProfileUpdateCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
