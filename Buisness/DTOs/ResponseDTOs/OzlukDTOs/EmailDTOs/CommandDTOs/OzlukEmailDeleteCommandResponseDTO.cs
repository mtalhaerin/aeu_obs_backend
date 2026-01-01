using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.CommandDTOs
{
    public class OzlukEmailDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid EpostaUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}
