using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.KullaniciDTOs
{
    public class UserDeleteCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
    }
}

