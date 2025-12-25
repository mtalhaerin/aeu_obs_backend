using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs
{
    public class OzlukAddresQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid? AddressUuid { get; set; } = null;
    }
}
