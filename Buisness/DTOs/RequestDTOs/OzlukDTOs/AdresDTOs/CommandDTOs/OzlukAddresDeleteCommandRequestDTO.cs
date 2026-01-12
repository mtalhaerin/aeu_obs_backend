using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.CommandDTOs
{
    public class OzlukAddresDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid AdresUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}
