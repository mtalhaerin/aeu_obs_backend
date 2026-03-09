using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.CommandDTOs
{
    public class DersDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid DersUuid { get; set; } = Guid.Empty;
    }
}