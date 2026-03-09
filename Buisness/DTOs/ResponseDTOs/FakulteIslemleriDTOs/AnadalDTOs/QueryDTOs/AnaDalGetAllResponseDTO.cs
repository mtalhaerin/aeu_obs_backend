using Business.DTOs._Generic;
using Core.Utilities.Paging;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs
{
    public class AnaDalGetAllResponseDTO : QueryResponseDTOBase
    {
        public List<AnaDalQueryResponseDTO> AnaDallar { get; set; } = new List<AnaDalQueryResponseDTO>();
        public Pager? Pager { get; set; } = null;
    }
}