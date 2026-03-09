using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs
{
    public class FakulteQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid FakulteUuid { get; set; } = Guid.Empty;
    }
}