using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.CommandDTOs
{
    public class OzlukEmailUpdateCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid EpostaUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
    }
}
