using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.QueryDTOs
{
    public class DersQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid DersUuid { get; set; } = Guid.Empty;
    }
}