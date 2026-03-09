using Business.DTOs._Generic;
using Core.Utilities.Paging;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs
{
    public class BolumGetAllResponseDTO : QueryResponseDTOBase
    {
        public List<BolumQueryResponseDTO> Bolumler { get; set; } = new List<BolumQueryResponseDTO>();
        //public Pager? Pager { get; set; } = null;
    }
}