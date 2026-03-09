using Business.DTOs._Generic;

namespace Buisness.DTOs.ResponseDTOs.FakulteDTOs.CommandDTOs
{
    public class BolumDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid BolumUuid { get; set; } = Guid.Empty;
    }
}