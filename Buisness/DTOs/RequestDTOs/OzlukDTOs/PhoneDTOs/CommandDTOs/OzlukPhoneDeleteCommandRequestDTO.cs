using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.CommandDTOs
{
    public class OzlukPhoneDeleteCommandRequestDTO : RequestDTOBase
    {
        public Guid TelefonUuid { get; set; } = Guid.Empty;
    }
}
