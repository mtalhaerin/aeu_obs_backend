using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.AuthDTOs.Command
{
    public class LogoutCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid UserUuid { get; set; } = Guid.Empty;
    }
}
