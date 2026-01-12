using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.CommandDTOs
{
    public class OzlukEmailUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid EpostaUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
    }
}
