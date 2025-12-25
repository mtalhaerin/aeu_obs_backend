using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs
{
    public class OzlukEmailsQueryRequestDTO : QueryRequestDTOBase
    {
        public string? Token { get; set; } = null;
    }
}
