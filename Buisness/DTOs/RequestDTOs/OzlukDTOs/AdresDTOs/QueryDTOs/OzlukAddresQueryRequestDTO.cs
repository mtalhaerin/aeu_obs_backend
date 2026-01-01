using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.QueryDTOs
{
    public class OzlukAddresQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid? AddressUuid { get; set; } = null;
    }
}
