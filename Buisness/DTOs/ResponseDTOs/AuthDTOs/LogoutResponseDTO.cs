using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.AuthDTOs
{
    public class LogoutResponseDTO : ResponseDTOBase
    {
        public Guid UserUuid { get; set; } = Guid.Empty;
    }
}
