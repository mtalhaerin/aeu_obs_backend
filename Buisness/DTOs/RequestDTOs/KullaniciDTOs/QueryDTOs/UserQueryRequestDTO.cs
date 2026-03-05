using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.RequestDTOs.KullaniciDTOs.QueryDTOs
{
    public class UserQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}
