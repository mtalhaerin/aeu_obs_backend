using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.PhoneDTOs.CommandDTOs
{
    public class OzlukPhoneDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid TelefonUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}
