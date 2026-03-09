using Business.DTOs._Generic;
using Core.Utilities.Paging;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs
{
    public class FakulteGetAllResponseDTO : QueryResponseDTOBase
    {
        public List<FakulteResponseDTO> Fakulteler { get; set; } = new List<FakulteResponseDTO>();
        //public Pager? Pager { get; set; } = null;
    }
}