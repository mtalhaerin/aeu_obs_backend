using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.QueryDTOs
{
    public class DersGetAllResponseDTO : QueryResponseDTOBase
    {
        public List<DersResponseDTO> Dersler { get; set; } = new List<DersResponseDTO>();
    }
}