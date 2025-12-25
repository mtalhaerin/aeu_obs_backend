using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.AuthDTOs.Command
{
    public class ValidateCommandResponseDTO : CommandResponseDTOBase
    {
        public bool IsValid { get; set; } = false;
    }
    
}
