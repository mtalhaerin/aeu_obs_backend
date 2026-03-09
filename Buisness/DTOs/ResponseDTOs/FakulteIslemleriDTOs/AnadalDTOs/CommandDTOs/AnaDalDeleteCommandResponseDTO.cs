using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs
{
    public class AnaDalDeleteCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
    }
}