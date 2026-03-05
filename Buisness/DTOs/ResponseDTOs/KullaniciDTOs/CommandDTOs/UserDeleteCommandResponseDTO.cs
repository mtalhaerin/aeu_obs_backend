using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs
{
    public class UserDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}
