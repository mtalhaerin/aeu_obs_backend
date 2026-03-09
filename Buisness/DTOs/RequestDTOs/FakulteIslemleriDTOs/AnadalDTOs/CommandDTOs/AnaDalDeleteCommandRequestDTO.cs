using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs
{
    public class AnaDalDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
    }
}