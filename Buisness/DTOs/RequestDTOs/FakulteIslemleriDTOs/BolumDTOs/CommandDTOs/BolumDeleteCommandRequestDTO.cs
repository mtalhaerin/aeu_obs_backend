using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.CommandDTOs
{
    public class BolumDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid BolumUuid { get; set; } = Guid.Empty;
    }
}