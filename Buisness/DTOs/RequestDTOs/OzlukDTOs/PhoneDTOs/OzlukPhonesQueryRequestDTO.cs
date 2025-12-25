using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs
{
    public class OzlukPhonesQueryRequestDTO : QueryRequestDTOBase
    {
        public string? Token { get; set; } = null;
    }
}
