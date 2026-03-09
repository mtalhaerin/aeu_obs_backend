using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs
{
    public class FakulteDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid FakulteUuid { get; set; } = Guid.Empty;
    }
}