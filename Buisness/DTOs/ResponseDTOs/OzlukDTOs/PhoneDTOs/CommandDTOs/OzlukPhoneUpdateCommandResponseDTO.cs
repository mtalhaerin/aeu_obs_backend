using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.PhoneDTOs.CommandDTOs
{
    public class OzlukPhoneUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid TelefonUuid { get; set; } = Guid.Empty;
        public string UlkeKodu { get; set; } = string.Empty;
        public string TelefonNo { get; set; } = string.Empty;
        public TelefonTipi TelefonTipi { get; set; } = TelefonTipi.CEP;
        public bool Oncelikli { get; set; } = false;
    }
}
