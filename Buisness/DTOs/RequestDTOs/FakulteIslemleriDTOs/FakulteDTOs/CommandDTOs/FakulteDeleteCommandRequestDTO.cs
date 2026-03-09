using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs
{
    public class FakulteDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid FakulteUuid { get; set; } = Guid.Empty;
    }
}