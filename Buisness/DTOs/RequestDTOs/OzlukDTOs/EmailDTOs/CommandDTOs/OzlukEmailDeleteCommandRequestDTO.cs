using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.CommandDTOs
{
    public class OzlukEmailDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid EpostaUuid { get; set; } = Guid.Empty;
    }
}
