using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs
{
    public class OzlukAdresDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid AdresUuid { get; set; } = Guid.Empty;
    }
}
