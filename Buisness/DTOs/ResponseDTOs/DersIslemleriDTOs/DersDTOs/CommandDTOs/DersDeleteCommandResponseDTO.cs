using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.CommandDTOs
{
    public class DersDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid DersUuid { get; set; } = Guid.Empty;
    }
}