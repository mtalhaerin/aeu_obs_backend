using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs
{
    public class AnaDalQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
    }
}